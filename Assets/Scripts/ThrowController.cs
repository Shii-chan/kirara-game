using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour
{
    public float thrust;
    private bool throwRight;

    private bool isRotating = false;
    Rigidbody2D rb;
    PolygonCollider2D polyCol;

    public enum State
    {
        Thrown,
        Returning
    }

    private float resetTime = 3;
    private float resetTimer = 0;
    private float returnRotationSpeed = 10;
    private float returnFlySpeed = 10;

    private State currentState;

    public GameObject player;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.polyCol = this.GetComponent<PolygonCollider2D>();

        if (this.throwRight)
        {
            rb.AddForce(new Vector2(150, 400));
        }
        else
        {
            rb.AddForce(new Vector2(-150, 400));
        }
        rb.SetRotation(Random.Range(0, 360));
        rb.AddTorque(Random.Range(5, 30));

        this.currentState = State.Thrown;
        this.rb.bodyType = UnityEngine.RigidbodyType2D.Dynamic;

        this.resetTimer = 0;
    }

    void FixedUpdate()
    {

        if (this.currentState == State.Thrown)
        {
            this.resetTimer += Time.fixedDeltaTime;
            if (this.resetTimer >= this.resetTime)
            {
                returnMatch();
            }
        }

        if (this.currentState == State.Returning)
        {
            float rotation = this.rb.rotation;

            player = GameObject.FindWithTag ("Player");

            // Move our position a step closer to the target.
            float step = returnFlySpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

            if (rotation != 0)
            {
                if (rotation > 0)
                {
                    this.rb.rotation = this.rb.rotation - this.returnRotationSpeed;
                }
                else
                {
                    this.rb.rotation = this.rb.rotation + this.returnRotationSpeed;
                }

                // if(rotation<=180 && rotation>0){
                //     if(rotation>returnRotationSpeed*2){
                //         this.rb.rotation = this.rb.rotation - 1;
                //     } else {
                //         this.rb.rotation = this.rb.rotation - this.returnRotationSpeed;
                //     }

                // } else {
                //     if(rotation<-(returnRotationSpeed*2)){
                //         this.rb.rotation = this.rb.rotation + 1;
                //     } else {
                //         this.rb.rotation = this.rb.rotation + this.returnRotationSpeed;
                //     }

                // }
            }
        }
    }

    private void returnMatch()
    {
        this.polyCol.enabled = false;
        this.currentState = State.Returning;
        this.resetTimer = 0;
        this.rb.bodyType = UnityEngine.RigidbodyType2D.Kinematic;
        this.rb.velocity = new Vector2(0,0);
    }

    public void setThrowLeft()
    {
        this.throwRight = false;
    }

    public void setThrowRight()
    {
        this.throwRight = true;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        if (this.currentState == State.Returning)
        {
            if (col.gameObject.tag == "Player")
            {
                Destroy(this.gameObject);
                col.GetComponent<PlayerMovement>().resetMatch();
            }
        }

    }

    public State getState(){
        return this.currentState;
    }


}
