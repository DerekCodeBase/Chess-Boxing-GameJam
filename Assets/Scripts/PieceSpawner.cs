using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject[] Squares;
    public GameObject[] Pieces;

    private SquareScript SquareScript;


    void Start()
    {
        foreach(GameObject square in Squares){
            SquareScript = square.GetComponent<SquareScript>();
            if(SquareScript.InitialPiece != null){
                Instantiate(SquareScript.InitialPiece, square.transform.position, Quaternion.identity);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
