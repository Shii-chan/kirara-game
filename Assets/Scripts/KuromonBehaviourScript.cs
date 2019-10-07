using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class KuromonBehaviourScript : MonoBehaviour
{
    private enum State{
        Idle,
        IdleMoving,
        Aggroed,
        Dead
    }

    private State curState = State.Idle;

    private enum Movement{
        Left,
        Right
    }

    private Movement lastMovement;
    private Movement currentMovement;

    private bool m_FacingRight = false;
    public Vector3 m_Velocity = Vector3.zero;
    float gravity = -33f;

    [Header("Movementspeed")]
    [SerializeField]
    public float runSpeed = 2f;

    [Range(0, .3f)] 
    [SerializeField]
    private float m_MovementSmoothing = .05f;

    CharacterController2D controller;

    [Header("Movement time")]
    [SerializeField]
    private float moveTime = 2;
    private float timeMoved = 0;

    private float idleTime = 1;
    private float timeIdled = 0;

    private int steps = 0;
    
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        this.curState = State.Idle;
        this.lastMovement = Movement.Left;
    }

    void Update()
    {
        
        if(curState==State.Idle){
            this.timeIdled = this.timeIdled + Time.deltaTime;
            if(this.timeIdled > this.idleTime){
                InitiateMovement();
            }            
        }

        if(curState==State.IdleMoving){
            if(this.timeMoved > this.moveTime){
                this.InitiateIdling();
            } else {
                if(this.currentMovement==Movement.Right){
                    this.MoveRight();
                } else{
                    this.MoveLeft();
                }
                
            }            
        }
    }

    private void InitiateIdling(){
        this.timeIdled = 0;
        curState = State.Idle;
    }

    private void InitiateMovement(){        
        this.timeMoved = 0;
        this.steps = 0;
        this.curState = State.IdleMoving;
        if(this.lastMovement==Movement.Right){
            this.currentMovement = Movement.Left;
        } else{
            this.currentMovement = Movement.Right;
        }
            
    }

    private void MoveLeft(){
    
        this.currentMovement = Movement.Left;
        this.lastMovement = Movement.Left;

        if(this.m_FacingRight){
            this.Flip();
        }
        m_Velocity.x = -runSpeed*Time.deltaTime;
        if (controller.collisions.below) m_Velocity.y = 0;
        else m_Velocity.y += gravity * Time.deltaTime;
        controller.Move(m_Velocity * Time.deltaTime, false);

        this.timeMoved = this.timeMoved + Time.deltaTime;
    }

    private void MoveRight(){

        this.currentMovement = Movement.Right;
        this.lastMovement = Movement.Right;

        if(!this.m_FacingRight){
            this.Flip();
        }

        m_Velocity.x = runSpeed * Time.deltaTime;
        if (controller.collisions.below) m_Velocity.y = 0;
        else m_Velocity.y += gravity * Time.deltaTime;

        controller.Move(m_Velocity * Time.deltaTime, false);
        
        this.timeMoved = this.timeMoved + Time.deltaTime;
    }

    private void Flip()
	{
		m_FacingRight = !m_FacingRight;
        
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
