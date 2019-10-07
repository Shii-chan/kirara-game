using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movementspeed")]
    [SerializeField]
    public float runSpeed = 5.625f;
    public float maxJumpHeight = 5f;
    public float jumpTime = 0.5f;
    public float gravity;
    public float maxJumpVelocity;
    Vector2 velocity;
    Vector2 faceRight = new Vector2(1,1);
    Vector2 faceLeft = new Vector2(-1,1);
    
    public CharacterController2D controller;

    float horizontalMove = 0f;

    public GameObject matchPrefab;
    public bool matchSpawned = false;

    private bool jump = false;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -(2*maxJumpHeight)/Mathf.Pow(jumpTime, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * jumpTime;
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
        if(Input.GetButtonDown("Jump") && controller.collisions.below && Input.GetAxisRaw("Vertical")!=-1) {
            velocity.y += maxJumpVelocity;
        }
        if (Input.GetButtonUp("Jump")){
            if (velocity.y > 0){
                velocity.y = 0;
            }
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime,(Input.GetButtonDown("Jump")&&Input.GetAxisRaw("Vertical")<0),false);
        if (controller.collisions.below || controller.collisions.above) velocity.y = 0;
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
