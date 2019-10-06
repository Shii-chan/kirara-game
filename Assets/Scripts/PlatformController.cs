using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastCollision
{
    public LayerMask passengerMask;
    public Vector3 move;
    List<PassengerMovement> passengers;
    Dictionary<Transform,CharacterController2D> passengerDictionary = new Dictionary<Transform, CharacterController2D>();
    public float timer = 0;
    public int turnaround;
    public override void Start()
    {
        base.Start();

    }

    void Update()
    {

        timer += Time.deltaTime;
        if (timer > turnaround) { 
            move = -move;
            timer = 0;
        }
        UpdateRaycastOrigins();
        Vector3 velocity = move * Time.deltaTime;
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
    void CalculateMovement(Vector3 velocity){

        HashSet<Transform> movedPassangers = new HashSet<Transform> ();
        passengers = new List<PassengerMovement>();
        float dirX = Mathf.Sign(velocity.x);
        float dirY = Mathf.Sign(velocity.y);

        if (velocity.y != 0) {
            float rayLenght = Mathf.Abs(velocity.y)+skin;
		    Vector2 rayOrigin;
		

		    for (int i = 0; i < vertRayCount; i++){
                if (dirY == -1) rayOrigin = origins.bottomLeft;
                else rayOrigin = origins.topLeft;
                rayOrigin +=Vector2.right * (vertRaySpacing * i);
			    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLenght, passengerMask);

                if (hit) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = 0;
                        if (dirY == 1) pushX = velocity.x;
                        float pushY = velocity.y - (hit.distance - skin) * dirY;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),dirY == 1, true));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector2.up * hit.distance * dirY, Color.red);

                    }
                }
            }
        }

        if (velocity.x != 0){
            float rayLenght = Mathf.Abs(velocity.x)+skin;
		    Vector2 rayOrigin;
		

		    for (int i = 0; i < horiRayCount; i++){
                if (dirX == -1) rayOrigin = origins.bottomLeft;
                else rayOrigin = origins.bottomRight;
                rayOrigin +=Vector2.up * (horRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLenght, passengerMask);
                
                if (hit) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = velocity.x - (hit.distance - skin) * dirX;
                        float pushY = -skin;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),false, true));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector2.up * hit.distance * dirY, Color.red);

                    }
                }
            }
        }

        if (dirY == -1 || velocity.y ==0 && velocity.x != 0){
             float rayLenght = skin * 2;
		    Vector2 rayOrigin;
		

		    for (int i = 0; i < vertRayCount; i++){
                rayOrigin = origins.topLeft + Vector2.right * (vertRaySpacing * i);
			    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLenght, passengerMask);

                if (hit) {
                    if (!movedPassangers.Contains(hit.transform)) {
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengers.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY),true, false));
                        movedPassangers.Add(hit.transform);
                        Debug.DrawRay(rayOrigin, Vector2.up * hit.distance * dirY, Color.red);
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
}
