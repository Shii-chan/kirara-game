using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movementspeed")]
    [SerializeField]
    public float runSpeed = 5.625f;
    public float jumpHeight = 5f;
    public float jumpTime = 0.5f;
    public float gravity;
    public float jumpVelocity;
    Vector3 velocity;
    Vector3 faceRight = new Vector3(1,1,1);
    Vector3 faceLeft = new Vector3(-1,1,1);
    
    public CharacterController2D controller;

    float horizontalMove = 0f;

    public GameObject matchPrefab;
    public bool matchSpawned = false;

    private bool jump = false;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -(2*jumpHeight)/Mathf.Pow(jumpTime, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumpTime;
        controller = GetComponent<CharacterController2D>();
    }
    
    void Update()
    {
        

        velocity.x = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (velocity.x < 0) transform.localScale = faceLeft;
        if (velocity.x > 0) transform.localScale = faceRight;

        /*if (Input.GetKeyDown(KeyCode.X))
        {
            throwMatch();
        }*/
        if (controller.collisions.below || controller.collisions.above) velocity.y = 0;
        if(Input.GetButtonDown("Jump") && controller.collisions.below) {
            velocity.y += jumpVelocity;
        }
        
        velocity.y += gravity *Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, false);

    }

    private void FixedUpdate()
    {

    }

    /*private void throwMatch()
    {
       / if(!this.matchSpawned){
            Vector2 startLocation = new Vector2();
            if(controller.isFacingRight()){
                startLocation = new Vector2(this.transform.position.x+0.5f,this.transform.position.y+0.5f);
            } else {
                startLocation = new Vector2(this.transform.position.x-0.5f,this.transform.position.y+0.5f);
            }

            
            
            
                GameObject newFlask = GameObject.Instantiate(matchPrefab, startLocation, Quaternion.identity);
            
                if(this.isFacingRight()){
                    newFlask.GetComponent<ThrowController>().setThrowRight();
                } else{
                    newFlask.GetComponent<ThrowController>().setThrowLeft();
                }   

                this.matchSpawned = true;

            }

            
        
    
    }

    public bool isFacingRight(){
        return controller.isFacingRight();
    }

    public void resetMatch(){
        this.matchSpawned = false;
    }*/
}
