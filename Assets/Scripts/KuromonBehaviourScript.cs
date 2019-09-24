using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Movementspeed")]
    [SerializeField]
    public float runSpeed = 1f;

    [Range(0, .3f)] 
    [SerializeField]
    private float m_MovementSmoothing = .05f;

    private Rigidbody2D m_Rigidbody2D;

    [Header("Movement time")]
    [SerializeField]
    private float moveTime = 1000;
    private float timeMoved = 0;

    private float idleTime = 1;
    private float timeIdled = 0;

    private int steps = 0;
    
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        this.curState = State.Idle;
        this.lastMovement = Movement.Left;
    }

    void Update()
    {
        
        if(curState==State.Idle){
            this.timeIdled = this.timeIdled + Time.fixedDeltaTime;
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

        Vector3 targetVelocity = new Vector2(-runSpeed, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        this.timeMoved = this.timeMoved + Time.fixedDeltaTime;
    }

    private void MoveRight(){

        this.currentMovement = Movement.Right;
        this.lastMovement = Movement.Right;

        if(!this.m_FacingRight){
            this.Flip();
        }

        Vector3 targetVelocity = new Vector2(runSpeed, m_Rigidbody2D.velocity.y);
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        this.timeMoved = this.timeMoved + Time.fixedDeltaTime;
    }

    private void Flip()
	{
		m_FacingRight = !m_FacingRight;
        
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
