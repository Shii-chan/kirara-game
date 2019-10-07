using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastCollision
{
    public LayerMask passengerMask;
    List<PassengerMovement> passengers;
    Dictionary<Transform,CharacterController2D> passengerDictionary = new Dictionary<Transform, CharacterController2D>();
    public Vector3 localDestination;
    Vector3 startPoint;
    Vector3 globalDestination;
    public float speed;
    public float waitTime;
    float nextMoveTime;

    float distanceToDestination;
    float percentToDestination;

    public override void Start()
    {
        base.Start();
        startPoint = transform.position;
        globalDestination = localDestination + transform.position;
    }

    void Update()
    {

        UpdateRaycastOrigins();
        Vector3 velocity = CalculatePlatformMovement();
        CalculateMovement(velocity);
        MovePassengers(true);
        transform.Translate (velocity);
        MovePassengers(false);
    }
    void MovePassengers(bool moveBefore){
        foreach (PassengerMovement passenger in passengers){
            if (!passengerDictionary.ContainsKey(passenger.transform)){
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<CharacterController2D>());
            }
            if (passenger.moveBefore == moveBefore){
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
            }
        }
    }

    Vector3 CalculatePlatformMovement(){ //move the platform itself

        Vector3 newPos;
        if (globalDestination != null && speed != 0 && Time.time > nextMoveTime){
            distanceToDestination = Vector3.Distance(startPoint, globalDestination);
            percentToDestination += Time.deltaTime * speed/distanceToDestination;
            newPos = Vector3.Lerp(startPoint,globalDestination,percentToDestination);
            newPos -= transform.position;
            if (percentToDestination >= 1){
                Vector3 oldDest = globalDestination;
                globalDestination = startPoint;
                startPoint = oldDest;
                percentToDestination = 0;
                nextMoveTime = Time.time + waitTime;
            }
        }
        else newPos = Vector3.zero;
        return newPos;
    }

    void CalculateMovement(Vector3 velocity){ //move characters on the platform

        HashSet<Transform> movedPassangers = new HashSet<Transform> ();
        passengers = new List<PassengerMovement>();
        float dirX = Mathf.Sign(velocity.x);
        float dirY = Mathf.Sign(velocity.y);

        if (velocity.y != 0) {
            float rayLenght = Mathf.Abs(velocity.y)+skin;
		    Vector3 rayOrigin;
		

		    for (int i = 0; i < vertRayCount; i++){
                if (dirY == -1) rayOrigin = origins.bottomLeft;
                else rayOrigin = origins.topLeft;
                rayOrigin +=Vector3.right * (vertRaySpacing * i);
			    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.up * dirY, rayLenght, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = 0;
                        if (dirY == 1) pushX = velocity.x;
                        float pushY = velocity.y - (hit.distance - skin) * dirY;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),dirY == 1, true));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector3.up * hit.distance * dirY, Color.red);

                    }
                }
            }
        }

        if (velocity.x != 0){
            float rayLenght = Mathf.Abs(velocity.x)+skin;
		    Vector3 rayOrigin;
		

		    for (int i = 0; i < horiRayCount; i++){
                if (dirX == -1) rayOrigin = origins.bottomLeft;
                else rayOrigin = origins.bottomRight;
                rayOrigin +=Vector3.up * (horRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.right * dirX, rayLenght, passengerMask);
                
                if (hit && hit.distance != 0) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = velocity.x - (hit.distance - skin) * dirX;
                        float pushY = -skin;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),false, true));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector3.up * hit.distance * dirY, Color.red);

                    }
                }
            }
        }

        if (dirY == -1 || velocity.y ==0 && velocity.x != 0){
             float rayLenght = skin * 2;
		    Vector3 rayOrigin;
		

		    for (int i = 0; i < vertRayCount; i++){
                rayOrigin = origins.topLeft + Vector2.right * (vertRaySpacing * i);
			    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector3.up, rayLenght, passengerMask);

                if (hit && hit.distance != 0) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),true, false));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector3.up * hit.distance * dirY, Color.red);
                    }
                }
            }    
        }
    }

    struct PassengerMovement{
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBefore;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOn, bool _moveBefore){
            transform = _transform; 
            velocity = _velocity;
            standingOnPlatform = _standingOn;
            moveBefore = _moveBefore;
        }
    }

    void OnDrawGizmos(){
        if (localDestination != null){
            Gizmos.color = Color.red;
            float size = 0.3f;

            Vector3 globalDestinationPos;
            if (Application.isPlaying){
                globalDestinationPos = globalDestination;
            }
            else globalDestinationPos = localDestination + transform.position;
            Gizmos.DrawLine(globalDestinationPos - Vector3.up * size, globalDestinationPos + Vector3.up * size);
            Gizmos.DrawLine(globalDestinationPos - Vector3.left * size, globalDestinationPos + Vector3.left * size);

        }
    }
}
