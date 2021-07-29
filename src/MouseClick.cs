using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    public int buttonIndex;
    public float delay=0.3f;
    public float delayAfter=0.3f;
    public bool throttle=false;
    float clickStart=-1;

    public delegate void OnClick();
    public OnClick onClick=delegate(){};

    public delegate bool IsValid();
    public IsValid isValid=delegate(){ return true; };



    // Update is called once per frame
    void Update()
    {
        if(!Input.mousePresent){
            return;
        }

        if(Input.GetMouseButtonDown(0)&&isValid()){
            clickStart=Time.time;
        }
        if(Input.GetMouseButtonUp(0)&&Time.time-clickStart<delay){

            if(throttle){
                return;
            }

            throttle=true;
            Invoke("ClearWaiting", delayAfter);
            onClick();
        }
    }

     void ClearWaiting(){
        throttle=false;
    }
}
