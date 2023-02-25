using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class FightManager : MonoBehaviour
{
    private PieceBehavior playerFightingPiece;
    private PieceBehavior botFightingPiece;
    private BoardManager boardManager;

    private InputHandler input;

    public Camera boardCam;
    public Camera ringCam;
    public Camera introCam;

    private int playerPieceHealth;
    private int botPieceHealth;
    private int botPoise;

    private Vector2 square;

    private SquareScript squareChecker;

    private string playerPieceStyle;
    private string botPieceStyle;

    public Animator BoardBlackoutAnim;
    public Animator RingBlackoutAnim;

    public EnemyPieceScript Enemy;
    public PlayerPieceBehavior Player;

    public GameObject tutorialObj;
    public Transform tutorialTransform;
    private GameObject createdTutorial;
    private Animator tutorialAnim;

    public GameObject CountdownAnim;
    public EnemyHealthBar enemyHealthBar;
    public EnemyHealthBar playerHealthBar;

    private SaveData playerSettings;

    private bool trackHealth;
    public bool DestroyAllAttacks = false;

    private bool PoiseBroken = false;

    private bool endThisGame = false;

    private bool botLose = false;

    public bool ShouldMoveOn = false;

    public AudioSource mainAudio;
    public AudioSource fightAudio;


    void Awake(){
        ringCam.enabled = false;
        playerSettings = GetComponent<SaveData>();
        input = GetComponent<InputHandler>();
        boardManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<BoardManager>();
        Physics2D.IgnoreLayerCollision(9, 9);
        mainAudio.Play();
    }

    void Start(){

    }


    public void GetFightingPieces(PieceBehavior player, PieceBehavior bot, SquareScript contestedSquare, bool playerAttacking){
        input.EnableBoardControls(false);
        DestroyAllAttacks = false;
        mainAudio.Pause();
        fightAudio.Play();

        playerFightingPiece = player;
        botFightingPiece = bot;

        squareChecker = contestedSquare;


        playerPieceHealth = playerFightingPiece.health;
        botPieceHealth = botFightingPiece.health;
        
        enemyHealthBar.SetInitialHealth(botPieceHealth);
        playerHealthBar.SetInitialHealth(playerPieceHealth);

        playerPieceStyle = playerFightingPiece.PieceName;
        botPieceStyle = botFightingPiece.PieceName;

        Enemy.SetSprite(botPieceStyle, botFightingPiece.PieceColor);
        Player.SetSprite(playerPieceStyle, playerFightingPiece.PieceColor);


        if(playerSettings.Difficulty > 0){
            botPoise = botPieceHealth;
            enemyHealthBar.SetInitialPoise(botPoise);
            if(botPoise > 0){
                PoiseBroken = false;
            }
        }


        Debug.Log(playerPieceHealth + "/" + botPieceHealth + "/" + square + "/" + botPieceStyle + "/" + playerPieceStyle + "/" + playerAttacking);
        AnimateTransition();

    }



    void AnimateTransition(){
        BoardBlackoutAnim.SetBool("FightEnd", false);
        BoardBlackoutAnim.SetBool("FightStart", true);
    }

    public void SwitchCamsToFight(){
        Debug.Log("Cams switched");
        boardCam.enabled = false;
        ringCam.enabled = true;
        StartCoroutine(UnfadeRing());
    }

    IEnumerator UnfadeRing(){
        RingBlackoutAnim.SetBool("FadeOn", false);
        RingBlackoutAnim.SetBool("FadeOff", true);
        Debug.Log("Unfading");
        yield return null;
    }

    IEnumerator Tutorial(){
        yield return new WaitForSeconds(1f);
        createdTutorial = Instantiate(tutorialObj, tutorialTransform.position, Quaternion.identity, tutorialTransform);
        yield return null;
    
    }

    public void SpawnEnemy(){
        
        Enemy.SetInitialHealth(botPieceHealth);
        Enemy.SpawnEnemy();

        Player.SetInitialHealth(playerPieceHealth);
        Player.SpawnPlayer();

        if(playerSettings.Tutorial){
            playerSettings.SetTutorial(false);
            StartCoroutine(Tutorial());
        }
        else{
            Countdown();
        }

    }

    public void Countdown(){
        Instantiate(CountdownAnim, tutorialTransform.position, Quaternion.identity, tutorialTransform);
    }

    public void StartFight(){
        Debug.Log("Starting Fight");
        input.EnableFightControls(true);
        Enemy.AttackCooldown();
        Enemy.BullseyeCooldown();
    }

    public void EndFight(bool playerWin){
        input.EnableFightControls(false);
        Enemy.EndFight();
        DestroyAllAttacks = true;
        fightAudio.Pause();
        mainAudio.Play();

        if(playerWin){
            Enemy.anim.SetBool("Death", true);
            if(botPieceStyle == "King"){
                Debug.Log("WinSceenLoading");
                SceneManager.LoadScene("WinScene");
                /*Debug.Log("MatchIsOver");
                botLose = true;
                endThisGame = true;*/
            }
            if(botPieceStyle != "Pawn" && botPieceStyle != "King"){
                botFightingPiece.DestroyThisPiece(true);
            }
            else{
                botFightingPiece.DestroyThisPiece(false);
            }
            playerFightingPiece.SetHealth(playerPieceHealth);




        }
        else{
            Player.anim.SetBool("Death", true);
            playerFightingPiece.DestroyThisPiece(false);
            botFightingPiece.SetHealth(botPieceHealth);

            if(playerPieceStyle == "King"){
                Debug.Log("LoseSceneLoading");
                SceneManager.LoadScene("LoseScene");
               /* Debug.Log("MatchIsOver");
                botLose = false;
                endThisGame = true; */
            }

        }

        RingBlackoutAnim.SetBool("FadeOff", false);
        RingBlackoutAnim.SetBool("FadeOn", true);
    }

    public void HurtBotPiece(int hp){
        if(playerSettings.Difficulty < 1 || PoiseBroken){
            botPieceHealth = botPieceHealth - hp;
            enemyHealthBar.RemoveHealth(botPieceHealth);
            if(botPieceHealth <= 0){
                EndFight(true);
            }
            else{
                Enemy.anim.SetBool("Hit", true);
            }
        }
        else{
            botPoise = botPoise - hp;
            enemyHealthBar.RemovePoise(botPoise);
            if(botPoise <= 0){
                PoiseBroken = true;
            }
        }
    }

    public void HurtPlayerPiece(int hp){
        playerPieceHealth = playerPieceHealth - hp;
        playerHealthBar.RemoveHealth(playerPieceHealth);
        if(playerPieceHealth <= 0){
            EndFight(false);
        }
        else{
            Player.anim.SetBool("Hit", true);
        }
    }

    public void BackToBoard(){
        ringCam.enabled = false;
        boardCam.enabled = true;

        BoardBlackoutAnim.SetBool("FightStart", false);
        BoardBlackoutAnim.SetBool("FightEnd", true);
    }

    public void StartUpBoard(){
        if(!endThisGame){
            input.EnableBoardControls(true);
            boardManager.SetFightOccuring(false);
            boardManager.ToFEN();
        }
        else{
            if(botLose){
                BoardBlackoutAnim.SetBool("FightEnd", false);
                BoardBlackoutAnim.SetBool("ToIntro", true);
            }
        }
    }

    public void CalledByToIntro(){
        boardCam.enabled = false;
        introCam.enabled = true;
    }

    public void ResetMoveOn(){
        ShouldMoveOn = false;
    }
}
