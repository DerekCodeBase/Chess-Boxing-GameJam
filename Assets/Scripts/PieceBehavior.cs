using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PieceBehavior : MonoBehaviour
{
    public string PieceName;
    public static GameObject SelectedPiece;
    public static bool IsPieceSelected = false;
    public bool PieceColor;
    private BoardManager boardManager;
    private InputHandler input;
    private SaveData playerSettings;

    private SquareScript currentSquare;
    private SquareScript targetSquare;
    private SquareScript squareChecker;

    private GameObject[] AllSquares;
    public static List<GameObject> legalMoves = new List<GameObject>();


    private int colLimiterRight = 0;
    private int colLimiterLeft = 0;
    private int rowLimiterTop = 0;
    private int rowLimiterBottom = 0;

    private int diagLimitBR = 0; 
    private int diagLimitBL = 0;
    private int diagLimitTR = 0;
    private int diagLimitTL = 0;

    

    private bool pawn;
    private bool rook;
    private bool horsey;
    private bool bishop;
    private bool queen;
    private bool king;

    private bool shouldMovePiece = false;
    public bool hasMoved = false;
    public static bool Captured;
    private bool playerAttacking;

    private KingScript WhiteKing;
    private KingScript BlackKing;

    private bool PlayerKingAttacked = false;


    List<SquareScript> toColorLeft = new List<SquareScript>();
    List<SquareScript> toColorRight = new List<SquareScript>();



    private Color greenAlpha = new Vector4(0f, 1f, 0f, 0.2f);
    private Color totalAlpha = new Vector4(0f, 0f, 0f, 0f);


    private FightManager fightManager;

    private PieceBehavior playerFightingPiece;
    private PieceBehavior botFightingPiece;

    public int health;




    void Awake()
    {
        Debug.Log(health);
        switch (PieceName){
            case "Pawn":
                pawn = true;
                break;
            case "Rook":
                rook = true;
                break;
            case "Horsey":
                horsey = true;
                break;
            case "Bishop":
                bishop = true;
                break;
            case "Queen":
                queen = true;
                break;
            case "King":
                king = true;
                break;

        }
        currentSquare = transform.parent.GetComponent<SquareScript>();
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
        playerSettings = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveData>();

        boardManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<BoardManager>();

        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();

        if(playerSettings.Difficulty > 1){

        }
    }
    public void SelectPiece()
    {
        transform.parent.GetComponent<SpriteRenderer>().color = greenAlpha;
        if(pawn){
            PawnBehavior();
        }
        else if(rook){
            RookBehavior();
        }
        else if(bishop){
            BishopBehavior();
        }
        else if(horsey){
            HorseyBehavior();
        }
        else if(queen){
            QueenBehavior();
        }
        else if(king){
            KingBehavior();
        }
    }

    public void SetAllSquares(GameObject[] arr){
        AllSquares = arr;
    }


    public IEnumerator ClearActive(){
        foreach(GameObject square in legalMoves){
            squareChecker = square.GetComponent<SquareScript>();
            squareChecker.SetActive(false);
        }
        foreach(SquareScript square in toColorLeft){
            square.GetComponent<SpriteRenderer>().color = totalAlpha;
        }
        foreach(SquareScript square in toColorRight){
            square.GetComponent<SpriteRenderer>().color = totalAlpha;
        }
        legalMoves.Clear();
        yield return null;
    }

    public void UpdatePosition(SquareScript newSquare, bool enpaasant = false){
        Debug.Log("No problemo good sir");
        StartCoroutine(ClearActive());

        
        if(PieceName == "Pawn"){
            boardManager.PawnMove(0);
            Debug.Log("First");
        }
        else if(Captured){
            boardManager.PawnMove(0);
            Captured = false;
            Debug.Log("Second");
        }
        else{
            boardManager.PawnMove(1);
            Debug.Log("Third");
        }


        if(newSquare.GetComponentInChildren<PieceBehavior>() != null){
            PositionRoutine();
            boardManager.SetFightOccuring(true);
            animatePieceMove(newSquare, false, true);
            Debug.Log("This Shouldn't Happen on Player Capture");
        }
        else{
            animatePieceMove(newSquare, false, false);
            currentSquare.CheckIfEmpty(false);
            hasMoved = true;
        }





        if(enpaasant){
            boardManager.SetEnPaasant(PieceColor, currentSquare.column);
        }
        else{
            boardManager.SetEnPaasant();
        }
        

        SetToDefault();

    }

    void Update(){
        if(shouldMovePiece){
            transform.position = Vector2.MoveTowards(transform.position, targetSquare.transform.position, 8f * Time.deltaTime);
        }
        if(targetSquare != null && transform.position == targetSquare.transform.position && shouldMovePiece){
            shouldMovePiece = false;
            input.EnableBoardControls(true);
            if(playerFightingPiece != null && botFightingPiece != null){
                fightManager.GetFightingPieces(playerFightingPiece, botFightingPiece, targetSquare, playerAttacking);
                Debug.Log("Setting pieces to fight");
            }
            else{
                boardManager.ToFEN();
                Debug.Log("Setting Fen from Update");
            }
        }
    }

    public void animatePieceMove(SquareScript newSquare, bool playerToFight = false, bool botToFight = false){
        input.EnableBoardControls(false);
        currentSquare = newSquare;
        playerFightingPiece = null;
        botFightingPiece = null;
        targetSquare = newSquare;

        shouldMovePiece = true;

        if(!playerToFight && botToFight){
            playerAttacking = false;
            playerFightingPiece = newSquare.GetComponentInChildren<PieceBehavior>();
            botFightingPiece = this;
        }
        else if(playerToFight && !botToFight){
            playerAttacking = true;
            playerFightingPiece = this;
            botFightingPiece = newSquare.GetComponentInChildren<PieceBehavior>();
        }

        transform.parent = newSquare.transform;
    }

    public void PositionRoutine(){
        IsPieceSelected = false;
        currentSquare.CheckIfEmpty(true);
        currentSquare.UnsetEPTarget();
        StartCoroutine(ClearActive());
    }

    public void Castle(bool right){
        if(right && PieceColor){
            IsPieceSelected = false;
            currentSquare.GetComponent<SpriteRenderer>().color = totalAlpha;
            currentSquare.CheckIfEmpty(true);
            StartCoroutine(ClearActive());
            currentSquare = boardManager.WhiteKingR.GetComponent<SquareScript>();
            hasMoved = true;
            currentSquare.CheckIfEmpty(false);


            transform.parent = boardManager.WhiteKingR.transform;
            transform.position = boardManager.WhiteKingR.transform.position;

            boardManager.WhiteRookRStart.GetComponentInChildren<PieceBehavior>().UpdatePosition(boardManager.WhiteRookR);

            foreach(GameObject square in AllSquares){
                square.GetComponent<SquareScript>().SetCastlePriority(false);
            }
            SetToDefault();


        }
        else if(right)
        {
            IsPieceSelected = false;
            currentSquare.GetComponent<SpriteRenderer>().color = totalAlpha;
            currentSquare.CheckIfEmpty(true);
            StartCoroutine(ClearActive());
            currentSquare = boardManager.BlackKingR.GetComponent<SquareScript>();
            hasMoved = true;
            currentSquare.CheckIfEmpty(false);


            transform.parent = boardManager.BlackKingR.transform;
            transform.position = boardManager.BlackKingR.transform.position;

            boardManager.BlackRookRStart.GetComponentInChildren<PieceBehavior>().UpdatePosition(boardManager.BlackRookR);

            foreach(GameObject square in AllSquares){
                square.GetComponent<SquareScript>().SetCastlePriority(false);
            }
            SetToDefault();
        }
        else if(!right && PieceColor){
            IsPieceSelected = false;
            currentSquare.GetComponent<SpriteRenderer>().color = totalAlpha;
            currentSquare.CheckIfEmpty(true);
            StartCoroutine(ClearActive());
            currentSquare = boardManager.WhiteKingL.GetComponent<SquareScript>();
            hasMoved = true;
            currentSquare.CheckIfEmpty(false);


            transform.parent = boardManager.WhiteKingL.transform;
            transform.position = boardManager.WhiteKingL.transform.position;

            boardManager.WhiteRookLStart.GetComponentInChildren<PieceBehavior>().UpdatePosition(boardManager.WhiteRookL);

            foreach(GameObject square in AllSquares){
                square.GetComponent<SquareScript>().SetCastlePriority(false);
            }
            SetToDefault();        
        }
        else{
            IsPieceSelected = false;
            currentSquare.GetComponent<SpriteRenderer>().color = totalAlpha;
            currentSquare.CheckIfEmpty(true);
            StartCoroutine(ClearActive());
            currentSquare = boardManager.BlackKingL.GetComponent<SquareScript>();
            hasMoved = true;
            currentSquare.CheckIfEmpty(false);


            transform.parent = boardManager.BlackKingL.transform;
            transform.position = boardManager.BlackKingL.transform.position;

            boardManager.BlackRookLStart.GetComponentInChildren<PieceBehavior>().UpdatePosition(boardManager.BlackRookL);

            foreach(GameObject square in AllSquares){
                square.GetComponent<SquareScript>().SetCastlePriority(false);
            }
            SetToDefault();  
        }
    }

    private void SetToDefault(){
        colLimiterRight = 0;
        colLimiterLeft = 0;
        rowLimiterTop = 0;
        rowLimiterBottom = 0;

        diagLimitBL = 0;
        diagLimitBR = 0;
        diagLimitTL = 0;
        diagLimitTR = 0;


    }


    void AllowCastle(){
        bool rookClearLeft = false;
        bool rookClearRight = false;
        int emptySpacesLeft = 0;
        int emptySpacesRight = 0;
        toColorLeft.Clear();
        toColorRight.Clear();
        foreach(GameObject square in AllSquares){
            squareChecker = square.GetComponent<SquareScript>();
            int difference = squareChecker.column - currentSquare.column;
            if(squareChecker.row == currentSquare.row && difference > 0)
            {
                switch(difference){
                    case 1:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            emptySpacesRight++;
                        }
                        break;
                    case 2:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            emptySpacesRight++;
                            toColorRight.Add(squareChecker);                            
                        }
                        break;
                    case 3:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && !square.GetComponentInChildren<PieceBehavior>().hasMoved){
                            rookClearRight = true;
                            toColorRight.Add(squareChecker);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if(squareChecker.row == currentSquare.row && difference < 0){
                switch(difference){
                    case -1:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            emptySpacesLeft++;
                        }
                        break;
                    case -2:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            emptySpacesLeft++;
                            toColorLeft.Add(squareChecker);
                        }
                        break;
                    case -3:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            emptySpacesLeft++;
                            toColorLeft.Add(squareChecker);

                        }
                        break;
                    case -4:
                        if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && !square.GetComponentInChildren<PieceBehavior>().hasMoved){
                            toColorLeft.Add(squareChecker);
                            rookClearLeft = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        if(rookClearLeft && emptySpacesLeft == 3){
            foreach(SquareScript square in toColorLeft){
                square.SetCastlePriority(true, false);
            }

        }
        if(rookClearRight && emptySpacesRight == 2){
            foreach(SquareScript square in toColorRight){
                square.SetCastlePriority(true, true);
            }
        }
    }

    public void SetHasMoved(){
        hasMoved = true;
    }

    void StartFight(PieceBehavior attackingPiece, PieceBehavior defendingPiece, SquareScript contestedSquare){
        Debug.Log(attackingPiece + "/" + defendingPiece + "/" + contestedSquare.row + "/" + contestedSquare.column);
    }

    public void SetHealth(int hp){
        health = hp;
    }


    public void DestroyThisPiece(bool hurtKing){
        if(hurtKing){
        BlackKing = GameObject.FindGameObjectWithTag("BlackKing").GetComponent<KingScript>();
        WhiteKing = GameObject.FindGameObjectWithTag("WhiteKing").GetComponent<KingScript>();
            if(PieceColor){
                WhiteKing.LowerHealth();
            }
            else{
                BlackKing.LowerHealth();
            }
        }
        Destroy(this.gameObject);
    }



    //a feeble attempt to detangle some spaghetti
    #region Piece based logic

    void PawnBehavior(){
            if(!hasMoved){
                if(!PieceColor){
                    foreach(GameObject square in AllSquares){
                        squareChecker = square.GetComponent<SquareScript>();
                        if(squareChecker.column == currentSquare.column && squareChecker.GetComponentInChildren<PieceBehavior>() == null && (squareChecker.row == currentSquare.row - 1 || squareChecker.row == currentSquare.row - 2)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                            if(squareChecker.row == currentSquare.row - 2){
                                squareChecker.SetEPTarget();
                            }
                        }
                        else if(squareChecker.row == currentSquare.row - 1 && (squareChecker.column == currentSquare.column + 1 || squareChecker.column == currentSquare.column - 1) && (squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor != PieceColor)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                    }
                }
                else{
                    foreach(GameObject square in AllSquares){
                        squareChecker = square.GetComponent<SquareScript>();
                        if(squareChecker.column == currentSquare.column && squareChecker.GetComponentInChildren<PieceBehavior>() == null && (squareChecker.row == currentSquare.row + 1 || squareChecker.row == currentSquare.row + 2)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                            if(squareChecker.row == currentSquare.row + 2){
                                squareChecker.SetEPTarget();
                            }
                        }
                        else if(squareChecker.row == currentSquare.row + 1 && (squareChecker.column == currentSquare.column + 1 || squareChecker.column == currentSquare.column - 1) && (squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor != PieceColor)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                    }
                }
            }
            else{
                if(!PieceColor){
                    foreach(GameObject square in AllSquares){
                        squareChecker = square.GetComponent<SquareScript>();
                        Debug.Log(squareChecker.row + "//" + squareChecker.column);
                        if(squareChecker.row == currentSquare.row - 1 && squareChecker.column == currentSquare.column && squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                        else if(squareChecker.row == currentSquare.row - 1 && (squareChecker.column == currentSquare.column + 1 || squareChecker.column == currentSquare.column - 1) && (squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor != PieceColor)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                    }
                }
                else{
                    foreach(GameObject square in AllSquares){
                        squareChecker = square.GetComponent<SquareScript>();
                        if(squareChecker.row == currentSquare.row + 1 && squareChecker.column == currentSquare.column && squareChecker.GetComponentInChildren<PieceBehavior>() == null){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                        else if(squareChecker.row == currentSquare.row + 1 && (squareChecker.column == currentSquare.column + 1 || squareChecker.column == currentSquare.column - 1) && (squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor != PieceColor)){
                            squareChecker.SetActive(true);
                            legalMoves.Add(square);
                        }
                    }
                }
            }
    }

    void RookBehavior(){
        foreach(GameObject square in AllSquares){
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.column == currentSquare.column && squareChecker.row != currentSquare.row){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    int difference = squareChecker.row - currentSquare.row;
                    if( difference < 0)
                    {
                        if(rowLimiterBottom == 0){
                            rowLimiterBottom = difference;
                        }
                        else if(rowLimiterBottom < difference){
                            rowLimiterBottom = difference;
                        }
                    }
                    else if(difference > 0){
                        if(rowLimiterTop == 0){
                            rowLimiterTop = difference;
                        }
                        else if(rowLimiterTop > difference){
                            rowLimiterTop = difference;
                        }
                    }
                }
            }
            else if(squareChecker.row == currentSquare.row && squareChecker.column != currentSquare.column){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    int difference = squareChecker.column - currentSquare.column;
                    if( difference < 0)
                    {
                        if(colLimiterLeft == 0){
                            colLimiterLeft = difference;
                        }
                        else if(colLimiterLeft < difference){
                            colLimiterLeft = difference;
                        }
                    }
                    else if(difference > 0){
                        if(colLimiterRight == 0){
                            colLimiterRight = difference;
                        }
                        else if(colLimiterRight > difference){
                            colLimiterRight = difference;
                        }
                    }
                }
            }
        }
        foreach(GameObject square in legalMoves){
            squareChecker = square.GetComponent<SquareScript>();
            if(colLimiterLeft != 0 && squareChecker.column < colLimiterLeft + currentSquare.column){
            }
            else if(colLimiterRight != 0 && squareChecker.column > colLimiterRight + currentSquare.column){
            }
            else if(rowLimiterTop != 0 && squareChecker.row > rowLimiterTop + currentSquare.row){
            }
            else if(rowLimiterBottom != 0 && squareChecker.row < rowLimiterBottom + currentSquare.row){
            }
            else if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){

            }
            else{
                squareChecker.SetActive(true);
            }

        }
    }

    void BishopBehavior(){
        foreach (GameObject square in AllSquares)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.row - currentSquare.row == squareChecker.column - currentSquare.column && squareChecker.row -currentSquare.row != 0){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column < 0)
                    {
                        Debug.Log("BL");
                        if(diagLimitBL < squareChecker.row - currentSquare.row || diagLimitBL == 0){
                            diagLimitBL = squareChecker.row - currentSquare.row;
                        }
                    }
                    else if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column > 0){
                        Debug.Log("TR");
                        if(diagLimitTR > squareChecker.row - currentSquare.row || diagLimitTR == 0){
                            diagLimitTR = squareChecker.row - currentSquare.row;
                        }
                    }

                }

            }
            else if(Mathf.Abs(currentSquare.row - squareChecker.row) == Mathf.Abs(currentSquare.column - squareChecker.column) && squareChecker.row != currentSquare.row){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    Debug.Log("HA");
                    if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column < 0){
                        Debug.Log("TL");
                        if(diagLimitTL > squareChecker.row - currentSquare.row || diagLimitTL == 0){
                            diagLimitTL = squareChecker.row - currentSquare.row;
                            Debug.Log(diagLimitTL + "TL");
                        }
                    }
                    else if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column > 0)
                    {
                        Debug.Log("BR");
                        if((diagLimitBR < squareChecker.row - currentSquare.row) || diagLimitBR == 0){
                            diagLimitBR = squareChecker.row - currentSquare.row;
                            Debug.Log(diagLimitBR + "BR");
                        }
                    }

                }
            }
        }
        foreach (GameObject square in legalMoves)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.row - currentSquare.row == squareChecker.column - currentSquare.column && squareChecker.row -currentSquare.row != 0)
            {
                    if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column < 0)
                    {
                        if(diagLimitBL <= squareChecker.row - currentSquare.row || diagLimitBL == 0){
                            squareChecker.SetActive(true);
                        }
                    }
                    else if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column > 0){
                        if(diagLimitTR >= squareChecker.row - currentSquare.row || diagLimitTR == 0){
                            squareChecker.SetActive(true);
                        }
                    }

            }
            
            else if(Mathf.Abs(currentSquare.row - squareChecker.row) == Mathf.Abs(currentSquare.column - squareChecker.column) && squareChecker.row != currentSquare.row){
                    if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column < 0){
                        if(diagLimitTL >= squareChecker.row - currentSquare.row || diagLimitTL == 0){
                            squareChecker.SetActive(true);
                        }
                    }
                    else if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column > 0)
                    {
                        if(diagLimitBR <= squareChecker.row - currentSquare.row || diagLimitBR == 0){
                            squareChecker.SetActive(true);
                        }
                    }   
            }
            if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){
                squareChecker.SetActive(false);  
            }
        }
    }

    void HorseyBehavior(){
        foreach(GameObject square in AllSquares){
            squareChecker = square.GetComponent<SquareScript>();

            if((Mathf.Abs(squareChecker.column - currentSquare.column) == 1 && Mathf.Abs(squareChecker.row - currentSquare.row) == 2) || (Mathf.Abs(squareChecker.column - currentSquare.column) == 2 && Mathf.Abs(squareChecker.row-currentSquare.row) == 1)){
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){
                }
                else{
                    legalMoves.Add(square);
                    squareChecker.SetActive(true);
                }
            }
        }
    }

    void QueenBehavior(){
        foreach (GameObject square in AllSquares)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.row - currentSquare.row == squareChecker.column - currentSquare.column && squareChecker.row -currentSquare.row != 0){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column < 0)
                    {
                        Debug.Log("BL");
                        if(diagLimitBL < squareChecker.row - currentSquare.row || diagLimitBL == 0){
                            diagLimitBL = squareChecker.row - currentSquare.row;
                        }
                    }
                    else if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column > 0){
                        Debug.Log("TR");
                        if(diagLimitTR > squareChecker.row - currentSquare.row || diagLimitTR == 0){
                            diagLimitTR = squareChecker.row - currentSquare.row;
                        }
                    }

                }

            }
            else if(Mathf.Abs(currentSquare.row - squareChecker.row) == Mathf.Abs(currentSquare.column - squareChecker.column) && squareChecker.row != currentSquare.row){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    Debug.Log("HA");
                    if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column < 0){
                        Debug.Log("TL");
                        if(diagLimitTL > squareChecker.row - currentSquare.row || diagLimitTL == 0){
                            diagLimitTL = squareChecker.row - currentSquare.row;
                            Debug.Log(diagLimitTL + "TL");
                        }
                    }
                    else if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column > 0)
                    {
                        Debug.Log("BR");
                        if((diagLimitBR < squareChecker.row - currentSquare.row) || diagLimitBR == 0){
                            diagLimitBR = squareChecker.row - currentSquare.row;
                            Debug.Log(diagLimitBR + "BR");
                        }
                    }

                }
            }
        }
        foreach(GameObject square in AllSquares)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.column == currentSquare.column && squareChecker.row != currentSquare.row){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    int difference = squareChecker.row - currentSquare.row;
                    if( difference < 0)
                    {
                        if(rowLimiterBottom == 0){
                            rowLimiterBottom = difference;
                        }
                        else if(rowLimiterBottom < difference){
                            rowLimiterBottom = difference;
                        }
                    }
                    else if(difference > 0){
                        if(rowLimiterTop == 0){
                            rowLimiterTop = difference;
                        }
                        else if(rowLimiterTop > difference){
                            rowLimiterTop = difference;
                        }
                    }
                }
            }
            else if(squareChecker.row == currentSquare.row && squareChecker.column != currentSquare.column){
                legalMoves.Add(square);
                if(squareChecker.GetComponentInChildren<PieceBehavior>() != null){
                    int difference = squareChecker.column - currentSquare.column;
                    if( difference < 0)
                    {
                        if(colLimiterLeft == 0){
                            colLimiterLeft = difference;
                        }
                        else if(colLimiterLeft < difference){
                            colLimiterLeft = difference;
                        }
                    }
                    else if(difference > 0){
                        if(colLimiterRight == 0){
                            colLimiterRight = difference;
                        }
                        else if(colLimiterRight > difference){
                            colLimiterRight = difference;
                        }
                    }
                }
            }
        }
        foreach(GameObject square in legalMoves)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(colLimiterLeft != 0 && squareChecker.column < colLimiterLeft + currentSquare.column){
            }
            else if(colLimiterRight != 0 && squareChecker.column > colLimiterRight + currentSquare.column){
            }
            else if(rowLimiterTop != 0 && squareChecker.row > rowLimiterTop + currentSquare.row){
            }
            else if(rowLimiterBottom != 0 && squareChecker.row < rowLimiterBottom + currentSquare.row){
            }
            else if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){

            }
            else{
                squareChecker.SetActive(true);
            }

        }
        foreach (GameObject square in legalMoves)
        {
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.row - currentSquare.row == squareChecker.column - currentSquare.column && squareChecker.row -currentSquare.row != 0)
            {
                    if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column < 0)
                    {
                        if(diagLimitBL <= squareChecker.row - currentSquare.row || diagLimitBL == 0){
                            squareChecker.SetActive(true);
                        }
                    }
                    else if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column > 0){
                        if(diagLimitTR >= squareChecker.row - currentSquare.row || diagLimitTR == 0){
                            squareChecker.SetActive(true);
                        }
                    }

            }

            
            else if(Mathf.Abs(currentSquare.row - squareChecker.row) == Mathf.Abs(currentSquare.column - squareChecker.column) && squareChecker.row != currentSquare.row){
                    if(squareChecker.row - currentSquare.row > 0 && squareChecker.column - currentSquare.column < 0){
                        if(diagLimitTL >= squareChecker.row - currentSquare.row || diagLimitTL == 0){
                            squareChecker.SetActive(true);
                        }
                    }
                    else if(squareChecker.row - currentSquare.row < 0 && squareChecker.column - currentSquare.column > 0)
                    {
                        if(diagLimitBR <= squareChecker.row - currentSquare.row || diagLimitBR == 0){
                            squareChecker.SetActive(true);
                        }
                    }

                
            }

            if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){
                squareChecker.SetActive(false);
            }
        
        }
    }

    void KingBehavior(){
        if(!hasMoved){
            AllowCastle();
        }
        foreach(GameObject square in AllSquares){
            squareChecker = square.GetComponent<SquareScript>();
            if(Mathf.Abs(squareChecker.row - currentSquare.row) <= 1 && Mathf.Abs(squareChecker.column - currentSquare.column) <= 1){
                if(Mathf.Abs(squareChecker.row - currentSquare.row) + Mathf.Abs(squareChecker.column - currentSquare.column) != 0){
                    legalMoves.Add(square);
                }
            }
        }
        foreach(GameObject square in legalMoves){
            squareChecker = square.GetComponent<SquareScript>();
            if(squareChecker.GetComponentInChildren<PieceBehavior>() != null && square.GetComponentInChildren<PieceBehavior>().PieceColor == PieceColor){

            }
            else{
                squareChecker.SetActive(true);
            }
        }
    }

    #endregion

}
