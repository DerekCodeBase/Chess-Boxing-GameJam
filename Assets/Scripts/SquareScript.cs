using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    public GameObject InitialPiece;
    public static PieceBehavior SelectedPiece;
    public static SquareScript highlighter;
    public int row;
    public int column;
    public bool isEmpty;
    public bool Active;

    private bool prioritizeCastle;
    private bool castleDir; //right is true

    private SpriteRenderer spRender;
    private FightManager fightManager;
    private BoardManager boardManager;

    private Color greenAlpha = new Vector4(0f, 1f, 0f, 0.2f);
    private Color totalAlpha = new Vector4(0f, 0f, 0f, 0f);

    private bool EPTarget = false;

    private InputHandler input;


    void Start()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
        if(InitialPiece != null){
            Instantiate(InitialPiece, this.transform.position, Quaternion.identity, this.transform);
            isEmpty = false;
        }
        else{
            isEmpty = true;
        }
        spRender = GetComponent<SpriteRenderer>();
        boardManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<BoardManager>();
    }


    public void SetActive(bool active){
        if(active){
            Active = true;
            spRender.color = greenAlpha;
        }
        else{
            Active = false;
            spRender.color = totalAlpha;
            Debug.Log("should be colorless");
        }

    }


    public void OnClick(){
        if(highlighter != null){
            highlighter.GetComponent<SpriteRenderer>().color = totalAlpha;
        }
        
        if(SelectedPiece == null){
            if(GetComponentInChildren<PieceBehavior>() != null && GetComponentInChildren<PieceBehavior>().PieceColor == boardManager.PlayerColor){
                SelectedPiece = GetComponentInChildren<PieceBehavior>();
                SelectedPiece.SelectPiece();
                highlighter = this;
                highlighter.GetComponent<SpriteRenderer>().color = greenAlpha;
            }
        }
        else if(Active){
            if(SelectedPiece != null && GetComponentInChildren<PieceBehavior>() == null){
                SelectedPiece.UpdatePosition(this);
                SelectedPiece = null;
            }
            else if(SelectedPiece != null && GetComponentInChildren<PieceBehavior>() == SelectedPiece){
                Debug.Log("Deselect the piece");
                StartCoroutine(SelectedPiece.ClearActive());
                SelectedPiece = null;
            }
            else if(SelectedPiece != null && GetComponentInChildren<PieceBehavior>() != null){
                Debug.Log(SelectedPiece + "/" + GetComponentInChildren<PieceBehavior>() + "/" + row + "/" + column);
                boardManager.SetFightOccuring(true);
                SelectedPiece.PositionRoutine();
                SelectedPiece.animatePieceMove(this, true);
                //fightManager.GetFightingPieces(SelectedPiece, GetComponentInChildren<PieceBehavior>(), this, true);
            }
        }
        else{
            Debug.Log("Deselect the piece");
            StartCoroutine(SelectedPiece.ClearActive());
            SelectedPiece = null;
            
        }






      /*  if(prioritizeCastle){
            SelectedPiece.GetComponent<PieceBehavior>().Castle(castleDir);
        }
        else if(EPTarget){
            SelectedPiece.GetComponent<PieceBehavior>().UpdatePosition(this, true);
        }*/
    }

    public void CheckIfEmpty(bool empty){
        isEmpty = empty;
    }

    public void SetCastlePriority(bool castle, bool dir = true){
        if(castle){
            prioritizeCastle = true;
            castleDir = dir;
        }
        else{
            prioritizeCastle = false;
        }
    }

    public void SetEPTarget(){
        EPTarget = true;
    }
    public void UnsetEPTarget(){
        EPTarget = false;
    }
}
