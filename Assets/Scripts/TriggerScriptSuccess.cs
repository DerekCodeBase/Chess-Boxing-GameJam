using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScriptSuccess : MonoBehaviour
{
    public Collider2D siblingCollider;


    void OnTriggerEnter2D(Collider2D col){
        if(col == siblingCollider){
            col.GetComponent<HandleManager>().SetSuccess();
        }
        Debug.Log("Success Set");
    }
}
