using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
   void OnTriggerExit2D(Collider2D col){
        Debug.Log("TRIGGEREXIT");
        col.GetComponent<HandleManager>().CountPenalty(true);
        Debug.Log("Started Penalty Count");
    }

    void OnTriggerEnter2D(Collider2D col){
        col.GetComponent<HandleManager>().CountPenalty(false);
        Debug.Log("Stopping Penalty Count");
    }
}
