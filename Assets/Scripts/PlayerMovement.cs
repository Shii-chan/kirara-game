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

    public GameObject matchPrefab;
    public bool matchSpawned = false;

    private bool jump = false;

    public GameObject kiraraContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetKeyDown(KeyCode.X))
        {
            throwMatch();
        }

        if(Input.GetButtonDown("Jump")){
            this.jump = true;
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            kiraraContainer.GetComponent<KiraraContainerScript>().faceIn();
        } else {
            kiraraContainer.GetComponent<KiraraContainerScript>().faceOut();
        }

    }

    private void FixedUpdate()
    {
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
    }
}
