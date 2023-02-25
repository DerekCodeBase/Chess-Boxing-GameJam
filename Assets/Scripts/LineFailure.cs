using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFailure : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col){
        col.GetComponent<HandleManager>().SetFailure();
        
        Debug.Log("Failure Set");
    }
}
