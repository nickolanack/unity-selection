using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAt : MonoBehaviour
{
    

    public static LookingAt Main;

    public GameObject lookingAt;
    float lastLookAtTime=-1;

    float debounce=0.05f;


    GameObject peekObject;
    float peekTimeStart=-1;

    public delegate bool ValidTarget(GameObject obj);
    public ValidTarget targetFilter=delegate(GameObject obj){
        return true;
    };

    public delegate GameObject ResolveTarget(GameObject obj);
    public ResolveTarget targetResolve=delegate(GameObject obj){
        return obj;
    };


    public delegate void LookAt(GameObject obj);
    public List<LookAt> onLookAtStart=new List<LookAt>();
    public List<LookAt> onLookAtEnd=new List<LookAt>();


    public float distance=Mathf.Infinity;
    public LayerMask layerMask = Physics.DefaultRaycastLayers;


    public bool useMousePosition=false;

    void Start(){

        if(LookingAt.Main==null){
            LookingAt.Main=this;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(ScreenPoint());
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;

            GameObject obj=targetResolve(objectHit.gameObject);

            if(obj!=null&&targetFilter(obj)){


                if(lookingAt==obj){
                    lastLookAtTime=Time.time;
                }

                if(lookingAt!=obj){

                    if(peekObject==obj&&Time.time-peekTimeStart>debounce){

                        if(lookingAt!=null){
                            GameObject last=lookingAt;
                            ClearLookingAt(last);
                        }

                        lookingAt=peekObject;
                        lastLookAtTime=Time.time;
                        peekObject=null;
                        peekTimeStart=-1;
                        SetLookingAt(obj);
                        return;
                    }

                    if(peekObject!=obj){
                        peekObject=obj;
                        peekTimeStart=Time.time;
                    }

                }
            }
            
        }




        if(lookingAt!=null&&Time.time-lastLookAtTime>debounce){
            GameObject last=lookingAt;
            lookingAt=null;
            lastLookAtTime=-1;
            ClearLookingAt(last);
        }


    }

    Vector2 ScreenPoint(){

        if(useMousePosition&&Input.mousePresent){
            return Input.mousePosition;
        }

        return new Vector2(Screen.width/2, Screen.height/2);
    }

    void SetLookingAt(GameObject gameObject){
        foreach(LookAt lookAt in onLookAtStart){
            lookAt(gameObject);
        }
    }

    void ClearLookingAt(GameObject gameObject){
        foreach(LookAt offLookAt in onLookAtEnd){
            offLookAt(gameObject);
        }
    }
}
