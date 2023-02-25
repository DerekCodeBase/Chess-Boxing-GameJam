using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using UnityEngine;
using System.IO;
using System;

public class BoardManager : MonoBehaviour
{
    public Transform WhiteKingR;
    public SquareScript WhiteRookR;
    public SquareScript WhiteRookRStart;

    public Transform WhiteKingL;
    public SquareScript WhiteRookL;
    public SquareScript WhiteRookLStart;

    public Transform BlackKingR;
    public SquareScript BlackRookR;
    public SquareScript BlackRookRStart;

    public Transform BlackKingL;
    public SquareScript BlackRookL;
    public SquareScript BlackRookLStart;

    public bool MoveColor;
    private GameObject[] AllSquares;
    public static List<GameObject> legalMoves = new List<GameObject>();
    private SquareScript squareChecker;

    private List<string> FENNotation = new List<string>();

    private PieceBehavior whiteKingPiece;
    private PieceBehavior whiteRookPieceQ;
    private PieceBehavior whiteRookPieceK;

    private PieceBehavior blackKingPiece;
    private PieceBehavior blackRookPieceK;
    private PieceBehavior blackRookPieceQ;

    private bool breakFromCalculations;

    private bool wCastleK;
    private bool wCastleQ;
    private bool bCastleK;
    private bool bCastleQ;

    private string EnPaasantSquare;
    public int DepthLevel;

    private bool lastPawnCaptureCount = false;
    private int pawnCaptureCount;
    private int moveTotalCount = 60;

    public bool PlayerColor;
    private string NotationString;

    private bool playerMoveAgain = false;
    private bool botMoving = false;
    private bool fightOccurring = false;

    private PieceBehavior pieceChecker;
    private SquareScript nestedSquareChecker;

    private PieceBehavior movePiece;
    private SquareScript moveSquare;

    private int colLimiterLeft;
    private int colLimiterRight;
    private int rowLimiterBottom;
    private int rowLimiterTop;

    private int diagLimitBL;
    private int diagLimitBR;
    private int diagLimitTL;
    private int diagLimitTR;

    private bool allowCastle = false;




    void Start()
    {
        

    }

    public void FindAllSquares(GameObject[] arr){
        AllSquares = arr;
    }

    void FixedUpdate(){
        if(!fightOccurring){
            if(PlayerColor != MoveColor && !botMoving){
                BotMove();
                botMoving = true;
            }
        }
    }

    public void SetFightOccuring(bool fight){
        fightOccurring = fight;
    }

    public void NextMove(){
        MoveColor = !MoveColor;
        botMoving = false;
        if(MoveColor){
            moveTotalCount++;
        }
    }

    public void ToFEN(){
        //CheckCastle();

        breakFromCalculations = false;


        List<string> Rank1 = new List<string>();
        bool lastRank1Empty = false;
        int row1EmptyCounter1 = 0;
        int row1EmptyCounter2 = 0;
        int row1EmptyCounter3 = 0;
        int row1EmptyCounter4 = 0;

        List<string> Rank2 = new List<string>();
        bool lastRank2Empty = false;
        int row2EmptyCounter1 = 0;
        int row2EmptyCounter2 = 0;
        int row2EmptyCounter3 = 0;
        int row2EmptyCounter4 = 0;

        List<string> Rank3 = new List<string>();
        bool lastRank3Empty = false;
        int row3EmptyCounter1 = 0;
        int row3EmptyCounter2 = 0;
        int row3EmptyCounter3 = 0;
        int row3EmptyCounter4 = 0;

        List<string> Rank4 = new List<string>();
        bool lastRank4Empty = false;
        int row4EmptyCounter1 = 0;
        int row4EmptyCounter2 = 0;
        int row4EmptyCounter3 = 0;
        int row4EmptyCounter4 = 0;

        List<string> Rank5 = new List<string>();
        bool lastRank5Empty = false;
        int row5EmptyCounter1 = 0;
        int row5EmptyCounter2 = 0;
        int row5EmptyCounter3 = 0;
        int row5EmptyCounter4 = 0;

        List<string> Rank6 = new List<string>();
        bool lastRank6Empty = false;
        int row6EmptyCounter1 = 0;
        int row6EmptyCounter2 = 0;
        int row6EmptyCounter3 = 0;
        int row6EmptyCounter4 = 0;

        List<string> Rank7 = new List<string>();
        bool lastRank7Empty = false;
        int row7EmptyCounter1 = 0;
        int row7EmptyCounter2 = 0;
        int row7EmptyCounter3 = 0;
        int row7EmptyCounter4 = 0;

        List<string> Rank8 = new List<string>();
        bool lastRank8Empty = false;
        int row8EmptyCounter1 = 0;
        int row8EmptyCounter2 = 0;
        int row8EmptyCounter3 = 0;
        int row8EmptyCounter4 = 0;


        List<int> Empties = new List<int>();

        foreach(GameObject square in AllSquares){
            squareChecker = square.GetComponent<SquareScript>();
            UnityEngine.Debug.Log(squareChecker.column + "/" + squareChecker.row);
            if(squareChecker.row == 1){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank1Empty){
                    if(row1EmptyCounter4 == 0){
                        if(row1EmptyCounter3 == 0){
                            if(row1EmptyCounter2 == 0){
                                row1EmptyCounter1++;
                            }
                            else{
                                row1EmptyCounter2++;
                            }
                        }
                        else{
                            row1EmptyCounter3++;
                        }
                    }
                    else{
                        row1EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank1Empty = true;
                    
                    if(row1EmptyCounter3 == 0){
                        if(row1EmptyCounter2 == 0){
                            if(row1EmptyCounter1 == 0){
                                row1EmptyCounter1++;
                            }
                            else{
                                row1EmptyCounter2++;
                            }
                        }
                        else{
                            row1EmptyCounter3++;
                        }
                    }
                    else{
                        row1EmptyCounter4++;
                    }
                }

                else if(lastRank1Empty)
                {
                    lastRank1Empty = false;
                    Rank1.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank1.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank1.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank1.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank1.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank1Empty)
                {
                    Rank1.Add("empty");
                }
            }

            else if(squareChecker.row == 2){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank2Empty){
                    if(row2EmptyCounter4 == 0){
                        if(row2EmptyCounter3 == 0){
                            if(row2EmptyCounter2 == 0){
                                row2EmptyCounter1++;
                            }
                            else{
                                row2EmptyCounter2++;
                            }
                        }
                        else{
                            row2EmptyCounter3++;
                        }
                    }
                    else{
                        row2EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank2Empty = true;
                    
                    if(row2EmptyCounter3 == 0){
                        if(row2EmptyCounter2 == 0){
                            if(row2EmptyCounter1 == 0){
                                row2EmptyCounter1++;
                            }
                            else{
                                row2EmptyCounter2++;
                            }
                        }
                        else{
                            row2EmptyCounter3++;
                        }
                    }
                    else{
                        row2EmptyCounter4++;
                    }
                }

                else if(lastRank2Empty)
                {
                    lastRank2Empty = false;
                    Rank2.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank2.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank2.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank2.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank2.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank2Empty)
                {
                    Rank2.Add("empty");
                }
            }

            else if(squareChecker.row == 3){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank3Empty){
                    if(row3EmptyCounter4 == 0){
                        if(row3EmptyCounter3 == 0){
                            if(row3EmptyCounter2 == 0){
                                row3EmptyCounter1++;
                            }
                            else{
                                row3EmptyCounter2++;
                            }
                        }
                        else{
                            row3EmptyCounter3++;
                        }
                    }
                    else{
                        row3EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank3Empty = true;
                    
                    if(row3EmptyCounter3 == 0){
                        if(row3EmptyCounter2 == 0){
                            if(row3EmptyCounter1 == 0){
                                row3EmptyCounter1++;
                            }
                            else{
                                row3EmptyCounter2++;
                            }
                        }
                        else{
                            row3EmptyCounter3++;
                        }
                    }
                    else{
                        row3EmptyCounter4++;
                    }
                }

                else if(lastRank3Empty)
                {
                    lastRank3Empty = false;
                    Rank3.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank3.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank3.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank3.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank3.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank3Empty)
                {
                    Rank3.Add("empty");
                }

            }

            else if(squareChecker.row == 4){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank4Empty){
                    if(row4EmptyCounter4 == 0){
                        if(row4EmptyCounter3 == 0){
                            if(row4EmptyCounter2 == 0){
                                row4EmptyCounter1++;
                            }
                            else{
                                row4EmptyCounter2++;
                            }
                        }
                        else{
                            row4EmptyCounter3++;
                        }
                    }
                    else{
                        row4EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank4Empty = true;
                    
                    if(row4EmptyCounter3 == 0){
                        if(row4EmptyCounter2 == 0){
                            if(row4EmptyCounter1 == 0){
                                row4EmptyCounter1++;
                            }
                            else{
                                row4EmptyCounter2++;
                            }
                        }
                        else{
                            row4EmptyCounter3++;
                        }
                    }
                    else{
                        row4EmptyCounter4++;
                    }
                }

                else if(lastRank4Empty)
                {
                    lastRank4Empty = false;
                    Rank4.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank4.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank4.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank4.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank4.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank4Empty)
                {
                    Rank4.Add("empty");
                }

            }

            else if(squareChecker.row == 5){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank5Empty){
                    if(row5EmptyCounter4 == 0){
                        if(row5EmptyCounter3 == 0){
                            if(row5EmptyCounter2 == 0){
                                row5EmptyCounter1++;
                            }
                            else{
                                row5EmptyCounter2++;
                            }
                        }
                        else{
                            row5EmptyCounter3++;
                        }
                    }
                    else{
                        row5EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank5Empty = true;
                    
                    if(row5EmptyCounter3 == 0){
                        if(row5EmptyCounter2 == 0){
                            if(row5EmptyCounter1 == 0){
                                row5EmptyCounter1++;
                            }
                            else{
                                row5EmptyCounter2++;
                            }
                        }
                        else{
                            row5EmptyCounter3++;
                        }
                    }
                    else{
                        row5EmptyCounter4++;
                    }
                }

                else if(lastRank5Empty)
                {
                    lastRank5Empty = false;
                    Rank5.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank5.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank5.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank5.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank5.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank5Empty)
                {
                    Rank5.Add("empty");
                }

            }

            else if(squareChecker.row == 6){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank6Empty){
                    if(row6EmptyCounter4 == 0){
                        if(row6EmptyCounter3 == 0){
                            if(row6EmptyCounter2 == 0){
                                row6EmptyCounter1++;
                            }
                            else{
                                row6EmptyCounter2++;
                            }
                        }
                        else{
                            row6EmptyCounter3++;
                        }
                    }
                    else{
                        row6EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank6Empty = true;
                    
                    if(row6EmptyCounter3 == 0){
                        if(row6EmptyCounter2 == 0){
                            if(row6EmptyCounter1 == 0){
                                row6EmptyCounter1++;
                            }
                            else{
                                row6EmptyCounter2++;
                            }
                        }
                        else{
                            row6EmptyCounter3++;
                        }
                    }
                    else{
                        row6EmptyCounter4++;
                    }
                }

                else if(lastRank6Empty)
                {
                    lastRank6Empty = false;
                    Rank6.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank6.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank6.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank6.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank6.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank6Empty)
                {
                    Rank6.Add("empty");
                }
            }

            else if(squareChecker.row == 7){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank7Empty){
                    if(row7EmptyCounter4 == 0){
                        if(row7EmptyCounter3 == 0){
                            if(row7EmptyCounter2 == 0){
                                row7EmptyCounter1++;
                            }
                            else{
                                row7EmptyCounter2++;
                            }
                        }
                        else{
                            row7EmptyCounter3++;
                        }
                    }
                    else{
                        row7EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank7Empty = true;
                    
                    if(row7EmptyCounter3 == 0){
                        if(row7EmptyCounter2 == 0){
                            if(row7EmptyCounter1 == 0){
                                row7EmptyCounter1++;
                            }
                            else{
                                row7EmptyCounter2++;
                            }
                        }
                        else{
                            row7EmptyCounter3++;
                        }
                    }
                    else{
                        row7EmptyCounter4++;
                    }
                }

                else if(lastRank7Empty)
                {
                    lastRank7Empty = false;
                    Rank7.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank7.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank7.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank7.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank7.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank7Empty)
                {
                    Rank7.Add("empty");
                }
            }

            else if(squareChecker.row == 8){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() == null && lastRank8Empty){
                    if(row8EmptyCounter4 == 0){
                        if(row8EmptyCounter3 == 0){
                            if(row8EmptyCounter2 == 0){
                                row8EmptyCounter1++;
                            }
                            else{
                                row8EmptyCounter2++;
                            }
                        }
                        else{
                            row8EmptyCounter3++;
                        }
                    }
                    else{
                        row8EmptyCounter4++;
                    }
                }
                else if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                    lastRank8Empty = true;
                    
                    if(row8EmptyCounter3 == 0){
                        if(row8EmptyCounter2 == 0){
                            if(row8EmptyCounter1 == 0){
                                row8EmptyCounter1++;
                            }
                            else{
                                row8EmptyCounter2++;
                            }
                        }
                        else{
                            row8EmptyCounter3++;
                        }
                    }
                    else{
                        row8EmptyCounter4++;
                    }
                }

                else if(lastRank8Empty)
                {
                    lastRank8Empty = false;
                    Rank8.Add("empty");
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank8.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank8.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                    
                }
                else{
                    if(squareChecker.GetComponentInChildren<PieceBehavior>().PieceColor){
                        Rank8.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName.ToUpper());
                    }
                    else{
                        Rank8.Add(squareChecker.GetComponentInChildren<PieceBehavior>().PieceName);
                    }
                }

                if(squareChecker.column == 8 && lastRank8Empty)
                {
                    Rank8.Add("empty");
                }
            }
        }


        int[] emptyArrays = {row8EmptyCounter1, row8EmptyCounter2, row8EmptyCounter3, row8EmptyCounter4, row7EmptyCounter1, row7EmptyCounter2, row7EmptyCounter3, row7EmptyCounter4, row6EmptyCounter1, row6EmptyCounter2, row6EmptyCounter3, row6EmptyCounter4, row5EmptyCounter1, row5EmptyCounter2, row5EmptyCounter3, row5EmptyCounter4, row4EmptyCounter1, row4EmptyCounter2, row4EmptyCounter3, row4EmptyCounter4, row3EmptyCounter1, row3EmptyCounter2, row3EmptyCounter3, row3EmptyCounter4, row2EmptyCounter1, row2EmptyCounter2, row2EmptyCounter3, row2EmptyCounter4, row1EmptyCounter1, row1EmptyCounter2, row1EmptyCounter3, row1EmptyCounter4};

        Empties.Clear();

        int emptyIT = 0;
        foreach(int empty in emptyArrays){
            emptyIT++;
            if(empty != 0){
                Empties.Add(empty);
            }
        }

        FENNotation.Clear();

        List<List<string>> Ranks = new List<List<string>>();
        Ranks.Add(Rank8);
        Ranks.Add(Rank7);
        Ranks.Add(Rank6);
        Ranks.Add(Rank5);
        Ranks.Add(Rank4);
        Ranks.Add(Rank3);
        Ranks.Add(Rank2);
        Ranks.Add(Rank1);
        
        int emptyPos = 0;
        int counter = 0;
        foreach(List<string> rank in Ranks){

            foreach(string name in rank)
            {
                if(name == "empty"){
                    FENNotation.Add(Convert(name, Empties[emptyPos]));
                    emptyPos++;
                }
                else{
                    FENNotation.Add(Convert(name));
                }
            }
            counter++;
            if(counter <= 7){
                FENNotation.Add("/");
            }
            
        }

        if(!MoveColor){
            FENNotation.Add(" w ");
        }
        else{
            FENNotation.Add(" b ");
        }

        if(wCastleQ || wCastleK || bCastleK || bCastleQ && allowCastle){
            if(wCastleQ){
                FENNotation.Add("K");
            }
            if(wCastleK){
                FENNotation.Add("Q");
            }
            if(bCastleK){
                FENNotation.Add("k");
            }
            if(bCastleQ){
                FENNotation.Add("q");
            }

        }
        else{
            FENNotation.Add("-");
        }

        FENNotation.Add(" -");

        FENNotation.Add(" 8");

        FENNotation.Add(" " + moveTotalCount.ToString());

        NotationString = "";
        foreach(string name in FENNotation){
           NotationString = NotationString + name;
        }

        UnityEngine.Debug.Log(FENNotation);
        NextMove();

        
    }

    string Convert(string conversion, int spaces = 0){
        switch(conversion){
            case "empty":
                return spaces.ToString();
            case "PAWN":
                return "P";
            case "Pawn":
                return "p";
            case "ROOK":
                return "R";
            case "Rook":
                return "r";
            case "BISHOP":
                return "B";
            case "Bishop":
                return "b";
            case "HORSEY":
                return "N";
            case "Horsey":
                return "n";
            case "QUEEN":
                return "Q";
            case "Queen":
                return "q";
            case "KING":
                return "K";
            case "King":
                return "k";
            default:
                return conversion;
            
        }
    }

    private IEnumerator Finder(){
        yield return new WaitForSeconds(1);
        foreach(GameObject rook in GameObject.FindGameObjectsWithTag("WhiteRook")){
            if(rook.GetComponentInParent<SquareScript>().column == 1){
                whiteRookPieceK = rook.GetComponent<PieceBehavior>();
            }
            else if(rook.GetComponentInParent<SquareScript>().column == 8){
                whiteRookPieceQ = rook.GetComponent<PieceBehavior>();
            }
        }
        foreach(GameObject rook in GameObject.FindGameObjectsWithTag("BlackRook")){
            if(rook.GetComponentInParent<SquareScript>().column == 1){
                blackRookPieceQ = rook.GetComponent<PieceBehavior>();
            }
            else if(rook.GetComponentInParent<SquareScript>().column == 8){
                blackRookPieceK = rook.GetComponent<PieceBehavior>();
            }
        }
        whiteKingPiece = GameObject.FindGameObjectWithTag("WhiteKing").GetComponent<PieceBehavior>();
        blackKingPiece = GameObject.FindGameObjectWithTag("BlackKing").GetComponent<PieceBehavior>();
    }
/*
    void CheckCastle(){
        if(!whiteKingPiece.hasMoved){
            try{
                if(!whiteRookPieceK.hasMoved){
                    wCastleK = true;
                }
                else if(whiteRookPieceK.hasMoved){
                    wCastleK = false;
                }
            }
            catch{
                wCastleK = false;
            }

            try{
                if(!whiteRookPieceQ.hasMoved){
                    wCastleQ = true;
                }
                else if(whiteRookPieceQ.hasMoved){
                    wCastleQ = false;
                }
            }
            catch{
                wCastleQ = false;
            }
        }
        else{
            wCastleK = false;
            wCastleQ = false;
        }

        if(!blackKingPiece.hasMoved){
            try{
                if(!blackRookPieceK.hasMoved){
                    bCastleK = true;
                }
                else if(blackRookPieceK.hasMoved){
                    bCastleK = false;
                }
            }
            catch{
                bCastleK = false;
            }

            try{
                if(!blackRookPieceQ.hasMoved){
                    bCastleQ = true;
                }
                else if(blackRookPieceQ.hasMoved){
                    bCastleQ = false;
                }
            }
            catch{
                bCastleQ = false;
            }
        }
        else{
            bCastleK = false;
            bCastleQ = false;
        }
    }
*/
    public void SetEnPaasant(bool color = true, int col = 0) {
        string pCol = "";
        string pRow = "";
        switch(color){
            case true:
                pRow = "3";
                break;
            case false:
                pRow = "6";
                break;
        }
        switch(col){
            case 1:
                pCol = " a";
                break;
            case 2:
                pCol = " b";
                break;
            case 3:
                pCol = " c";
                break;
            case 4:
                pCol = " d";
                break;
            case 5:
                pCol = " e";
                break;
            case 6:
                pCol = " f";
                break;
            case 7:
                pCol = " g";
                break;
            case 8:
                pCol = " h";
                break;
            default:
                pCol = " -";
                pRow = "";
                break;
        }


        EnPaasantSquare = pCol + pRow;

    }

    public void PawnMove(int move){
        if(lastPawnCaptureCount != MoveColor){
            if(move == 0){
                pawnCaptureCount = 0;
            }
            else{
                pawnCaptureCount++;
            }
            lastPawnCaptureCount = !lastPawnCaptureCount;
        }
    }

    public void BotMove(){
        playerMoveAgain = false;
        string bestMove = GetBestMove(NotationString);
        UnityEngine.Debug.Log(bestMove.Substring(bestMove.Length - 4));
        int startingPoint = bestMove.IndexOf(" pv ");
        string toConvert = bestMove.Substring(startingPoint + 4);
        UnityEngine.Debug.Log(toConvert);
        if(toConvert.Contains("mate")){
            UnityEngine.Debug.Log("I am mated");
            playerMoveAgain = true;
        }
        else{
            char columnStart = toConvert[0];
            int colStart = 0;
            int rowStart = toConvert[1] - '0';

            char columnEnd = toConvert[2];
            int colEnd = 0;
            int rowEnd = toConvert[3] - '0';


            UnityEngine.Debug.Log( toConvert[0] + "/" + toConvert[1] + "/" + toConvert[2] + "/" + toConvert[3]);


            switch(columnStart){
                case 'a':
                    colStart = 1;
                    break;
                case 'b':
                    colStart = 2;
                    break;
                case 'c':
                    colStart = 3;
                    break;
                case 'd':
                    colStart = 4;
                    break;
                case 'e':
                    colStart = 5;
                    break;
                case 'f':
                    colStart = 6;
                    break;
                case 'g':
                    colStart = 7;
                    break;
                case 'h':
                    colStart = 8;
                    break;
                default:
                    UnityEngine.Debug.Log("This Shouldn't be happening, converting bot move.");
                    break;
            }

            
            switch(columnEnd){
                case 'a':
                    colEnd = 1;
                    break;
                case 'b':
                    colEnd = 2;
                    break;
                case 'c':
                    colEnd = 3;
                    break;
                case 'd':
                    colEnd = 4;
                    break;
                case 'e':
                    colEnd = 5;
                    break;
                case 'f':
                    colEnd = 6;
                    break;
                case 'g':
                    colEnd = 7;
                    break;
                case 'h':
                    colEnd = 8;
                    break;
                default:
                    UnityEngine.Debug.Log("This Shouldn't be happening, converting bot move.");
                    bool playerMoveAgain = true;
                    break;
            }

            UnityEngine.Debug.Log(colStart + "/" + rowStart + "/" + colEnd + "/" + rowEnd);

            MovePiece(colStart, rowStart, colEnd, rowEnd);

        }

    }



    void MovePiece(int col1, int row1, int col2, int row2){
        foreach(GameObject square in AllSquares){

            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.column == col1 && squareChecker.row == row1){
                movePiece = squareChecker.GetComponentInChildren<PieceBehavior>();
            }
            else if(squareChecker.column == col2 && squareChecker.row == row2)
            {
                moveSquare = squareChecker;
            }
        }
        if(movePiece != null && moveSquare != null){
            movePiece.UpdatePosition(moveSquare);
        }
        else{
            UnityEngine.Debug.Log("piece or square not found");
        }
        
    }

string GetBestMove(string forsythEdwardsNotationString){
    var p = new System.Diagnostics.Process();
    p.StartInfo.FileName = Application.dataPath + "/stockfish_15.1_win_x64_popcnt/stockfish_15.1_win_x64_popcnt/stockfishExecutable.exe";
    p.StartInfo.UseShellExecute = false;
    p.StartInfo.RedirectStandardInput = true;
    p.StartInfo.RedirectStandardOutput = true;
    p.StartInfo.CreateNoWindow = true;

    p.Start();  
    string setupString = "position fen "+forsythEdwardsNotationString;
    p.StandardInput.WriteLine(setupString);
    UnityEngine.Debug.Log(setupString);
    // Process for 5 seconds
    string processString = "go movetime 5000";
    
    // Process 20 deep
    //string processString = "go depth 20";

    try{p.StandardInput.WriteLine(processString);
    UnityEngine.Debug.Log("did write");}
    catch{UnityEngine.Debug.Log("didn't write");}
    
    string bestMoveInAlgebraicNotation = p.StandardOutput.ReadLine();
    UnityEngine.Debug.Log(bestMoveInAlgebraicNotation);
    string bestMoveStr = "";

    for(int i = 1; i<= DepthLevel; i++){
        if(i == DepthLevel){
            bestMoveStr = p.StandardOutput.ReadLine();
        }
        else{
            string throwaway = p.StandardOutput.ReadLine();
        }
    }
   
    UnityEngine.Debug.Log(bestMoveStr);

    p.Close();

    

    return bestMoveStr;
}


}


