using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{

    public static Selection Main;
    public delegate void SelectionEvent(List<GameObject> selection);
    List<SelectionEvent> onSelectionEventListeners=new List<SelectionEvent>();


    public List<GameObject> list=new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Selection.Main=this;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public bool Contains(GameObject obj){
        return list.Contains(obj);
    }

    public List<GameObject> Get(){
        return new List<GameObject>(list);
    }

    public int IndexOf(GameObject obj){
        return list.IndexOf(obj);
    }


    public void SetSelected(GameObject obj){
        if(list.Contains(obj)&&list.Count==1){
            return;
        }

        list=new List<GameObject>();
        list.Add(obj);
        foreach(SelectionEvent listener in onSelectionEventListeners){
            listener(new List<GameObject>(list));
        }

    }


    public bool IsAlreadySelected(List<GameObject> newList){
        if(list.Count==newList.Count){

            bool identical=true;
            foreach(GameObject obj in newList){
                identical=identical&&list.Contains(obj)&&list.IndexOf(obj)==newList.IndexOf(obj);
            }

            if(identical){
                //Debug.Log("Identical");
                return true;
            }
        }


        return false;

    }

    public void SetSelected(List<GameObject> newList){
        
        if(IsAlreadySelected(newList)){
            return;
        }

        list=new List<GameObject>(newList);
    
        foreach(SelectionEvent listener in onSelectionEventListeners){
            listener(new List<GameObject>(list));
        }

    }

    public void ClearSelected(){
        if(list.Count==0){
            return;
        }

        list=new List<GameObject>();
        foreach(SelectionEvent listener in onSelectionEventListeners){
            listener(new List<GameObject>());
        }

    }



    public void OnSelectionChanged(SelectionEvent listener){
        onSelectionEventListeners.Add(listener);
    }
}
