using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movementspeed")]
    [SerializeField]
    public float runSpeed = 40f;

    public CharacterController2D controller;

    float horizontalMove = 0f;

<<<<<<< HEAD
    public GameObject flask;
=======
    public GameObject matchPrefab;
    public bool matchSpawned = false;

    private bool jump = false;
>>>>>>> new_master

    // Start is called before the first frame update
    void Start()
    {
        
    }
<<<<<<< HEAD

    // Update is called once per frame
    void Update()
    {
        //Time.fixedDeltaTime;
        //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        //Vector2 currentPos = this.transform.position;
        //this.transform.position = new Vector2(this.transform.position.x + horizontalMove, this.transform.position.y);

=======
    
    void Update()
    {
>>>>>>> new_master
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.X))
        {
<<<<<<< HEAD
            throwFlask();
        }
=======
            throwMatch();
        }

        if(Input.GetButtonDown("Jump")){
            this.jump = true;
        }

>>>>>>> new_master
    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
    }

    private void throwFlask()
    {
        Vector2 startLocation = this.transform.position;
        Instantiate(flask, startLocation, Quaternion.identity);
=======
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
            this.jump = false;
            // controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
            
        
    }

    private void throwMatch()
    {
        if(!this.matchSpawned){
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
>>>>>>> new_master
    }
}
