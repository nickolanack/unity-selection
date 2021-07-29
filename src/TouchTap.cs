using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTap : MonoBehaviour
{
    
    public float delay=0.3f;
    public float delayAfter=0.5f;
    public bool throttle;

    
    Dictionary<int, float> touchStart=new Dictionary<int, float>();

    public delegate void OnTap(Touch touch);
    public OnTap onTap=delegate(Touch touch){};


    public delegate bool IsValid(Touch touch);
    public IsValid isValid=delegate(Touch touch){return true;};

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount==0){
            return;
        }


        foreach(Touch touch in Input.touches){
            if(touch.phase==TouchPhase.Began&&isValid(touch)){
                touchStart.Add(touch.fingerId, Time.time);
            }

            if(touch.phase==TouchPhase.Ended){
                if(touchStart.ContainsKey(touch.fingerId)){

                    float start=touchStart[touch.fingerId];
                    touchStart.Remove(touch.fingerId);

                    if(throttle){
                        return;
                    }


                    if(Time.time-start<delay){
                        throttle=true;
                        Invoke("ClearWaiting", delayAfter);
                        onTap(touch);
                    }

                   
                }
            }

        }


    }

    void ClearWaiting(){
        throttle=false;
    }
}
