using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using QuickOutline = Outline;

public class HighlightSelectableURP : MonoBehaviour
{

    public LookingAt lookAt;
    public Selection selection;

    // Start is called before the first frame update
    void Start()
    {


        if(lookAt==null){
            lookAt=gameObject.AddComponent<LookingAt>();
        }

        
        
        lookAt.useMousePosition=true;
        

        lookAt.onLookAtStart.Add(delegate(GameObject obj){

            if(IsSelected(obj)){
                return;
            }

            QuickOutline o=obj.GetComponent<QuickOutline>();
            if(o==null){
                try{
                    o=obj.AddComponent<QuickOutline>();
                }catch(Exception e){
                    Debug.LogError(e);
                }
            }
            if(o==null){
                //Terrain?
                return;
            }
            o.enabled=true;
            //o.color=0;
        });

        lookAt.onLookAtEnd.Add(delegate(GameObject obj){

            if(IsSelected(obj)){
                return;
            }

            QuickOutline o=obj.GetComponent<QuickOutline>();
            if(o!=null){
                o.enabled=false;
            }


        });
    }

    bool IsSelected(GameObject obj){
        if(selection==null){
            return false;
        }

        return selection.Contains(obj);
    }

    void Update(){

        if(selection==null){

            selection=gameObject.GetComponent<Selection>();

            if(selection==null){
                selection=Selection.Main;
            }

            if(selection!=null){

                List<GameObject> lastSelection=new List<GameObject>();
                selection.OnSelectionChanged(delegate(List<GameObject> list){

                    foreach(GameObject obj in lastSelection){
                        if(obj!=null&&!list.Contains(obj)){
                            QuickOutline o=obj.GetComponent<QuickOutline>();
                            if(o!=null){
                                o.enabled=false;
                            }
                        }
                    }

                    foreach(GameObject obj in list){
                       
                            QuickOutline o=obj.GetComponent<QuickOutline>();
                            if(o==null){
                                try{
                                    o=obj.AddComponent<QuickOutline>();
                                }catch(Exception e){
                                    Debug.LogError(e);
                                }
                            }
                            if(o==null){
                                //Terrain?
                                return;
                            }
                            o.enabled=true;
                            //o.color=list.IndexOf(obj)==0?1:2;
                        
                    }



                    lastSelection=new List<GameObject>(list); //copy
                });
            }

        }


    }

}
