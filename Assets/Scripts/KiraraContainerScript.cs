using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiraraContainerScript : MonoBehaviour
{

    CanvasGroup canvasGroup;

    private enum State {
        FadingIn,
        FadingOut,
        Neutral
    }

    private State curState;

    public float fadeStep = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        curState = State.Neutral;
        // faceIn();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(curState);
        if(curState==State.FadingIn)   {
            if(this.canvasGroup.alpha<1){
                this.canvasGroup.alpha = this.canvasGroup.alpha + fadeStep;
            }
        }

        if(curState==State.FadingOut)   {
            if(this.canvasGroup.alpha>0){
                this.canvasGroup.alpha = this.canvasGroup.alpha - fadeStep;
            }
        }
    }

    public void faceIn(){
        this.curState = State.FadingIn;     
    }

    public void faceOut(){
        this.curState = State.FadingOut;     
    }
}
