using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPasser : MonoBehaviour
{
    public GameObject[] AllSquares;
    private BoardManager boardManager;

    void Awake(){
        boardManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<BoardManager>();
        boardManager.FindAllSquares(AllSquares);
        StartCoroutine(PassAllSquares());
    }

    IEnumerator PassAllSquares(){
        yield return new WaitForSeconds(1);
        foreach(GameObject square in AllSquares){
            if(square.GetComponentInChildren<PieceBehavior>() != null){
                square.GetComponentInChildren<PieceBehavior>().SetAllSquares(AllSquares);
            }
        }
    }


}
