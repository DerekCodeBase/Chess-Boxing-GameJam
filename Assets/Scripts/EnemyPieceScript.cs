using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPieceScript : MonoBehaviour
{
    public Animator anim;
    public Sprite[] sprites;
    private SpriteRenderer spRender;
    public PlayerPieceBehavior player;
    private SaveData playerSettings;
    private FightManager fightManager;

    private bool pawn = false;
    private bool rook = false;
    private bool knight = false;
    private bool bishop = false;
    private bool queen = false;
    private bool king = false;
    private bool fightOccurring = false;

    private bool CanAttack;
    private bool BullseyeAttack = false;

    private float bullseyeBuffer;


    public GameObject linePrefab;
    public GameObject bullseyePrefab;
    public Transform lineParent;
    public Transform bullseyeParent;
    private Transform[] points;



    private int poise;
    private int health;

    private GameObject tempLine;
    private GameObject tempBullseye;

    private int[] rookAngles = {0, 90};
    private int[] bishopAngles = {0, 180};
    private int[] knightAngles = {0, 90, 180, 270};
    private int[] kingAngles = {0, 45, 90, 135, 180, 225, 270, 315};


    private float coolDownTime = 1f;

    void Start(){
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("PlayerPiece").GetComponent<PlayerPieceBehavior>();
        playerSettings = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveData>();
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();

        bullseyeBuffer = playerSettings.BullseyeBuffer;
    }

    public void SetSprite(string piece, bool white = false){
        if(!white){
            switch(piece){
                case "Pawn":
                    spRender.sprite = sprites[0];
                    pawn = true;
                    break;
                case "Rook":
                    spRender.sprite = sprites[1];
                    rook = true;
                    break;
                case "Horsey":
                    spRender.sprite = sprites[2];
                    knight = true;
                    break;
                case "Bishop":
                    spRender.sprite = sprites[3];
                    bishop = true;
                    break;
                case "Queen":
                    spRender.sprite = sprites[4];
                    queen = true;
                    break;
                case "King":
                    spRender.sprite = sprites[5];
                    king = true;
                    break;
                default:
                    spRender.sprite = sprites[1];
                    Debug.Log("Setting Sprite Failed");
                    break;
            }
        }
        else{
            switch(piece){
                case "Pawn":
                    spRender.sprite = sprites[6];
                    pawn = true;
                    break;
                case "Rook":
                    spRender.sprite = sprites[7];
                    rook = true;
                    break;
                case "Horsey":
                    spRender.sprite = sprites[8];
                    knight = true;
                    break;
                case "Bishop":
                    spRender.sprite = sprites[9];
                    bishop = true;
                    break;
                case "Queen":
                    spRender.sprite = sprites[10];
                    queen = true;
                    break;
                case "King":
                    spRender.sprite = sprites[11];
                    king = true;
                    break;
                default:
                    spRender.sprite = sprites[6];
                    Debug.Log("Setting Sprite Failed");
                    break;
            }
        }
        
    }

    public void SetInitialHealth(int hp){
        health = hp;
    }

    public void SetHealth(int hp){
        health = health - hp;
    }

    public void SetPlayerHealth(int hp){
        player.health = player.health - hp;
    }

    public void SpawnEnemy(){
        anim.SetBool("Spawn", true);
    }

    private void FixedUpdate(){
        if(fightOccurring){


            if(health <= 0){
                //Debug.Log("Fight is over, enemy piece has died.");
            }

            if(CanAttack){
                CanAttack = false;
                SpawnAttack();
            }
            if(BullseyeAttack){
                BullseyeAttack = false;
                BullseyeSpawn();
            }
        }
    }




    public void EndFight(){
        fightOccurring = false;
        CanAttack = false;
        BullseyeAttack = false;
        pawn = false;
        rook = false;
        knight = false;
        queen = false;
        king = false;
        bishop = false;
    }

    #region Attacking Functions

    void SpawnAttack(){

        if(pawn){
            PawnBehavior();
        }
        else if(knight){
            KnightBehavior();
        }
        else if(rook){
            RookBehavior();
        }
        else if(bishop){
            BishopBehavior();
        }
        else if(queen){
            QueenBehavior();
        }
        else if(king){
            KingBehavior();
        }
    }
    void BullseyeSpawn(){
        tempBullseye = Instantiate(bullseyePrefab, bullseyeParent.position, Quaternion.identity, bullseyeParent);
        tempBullseye.GetComponent<BullseyeScript>().DrawBullseye(this);
    }

    public void AttackCooldown(){
        if(!fightOccurring && !fightManager.DestroyAllAttacks){
            fightOccurring = true;
        }
        StartCoroutine(EnableAttack());
    }

    public void BullseyeCooldown(){
        StartCoroutine(ResetBullseye());
    }

    IEnumerator EnableAttack(){
        yield return new WaitForSeconds(coolDownTime);
        CanAttack = true;
    }

    public IEnumerator ResetBullseye(){
        yield return new WaitForSeconds(.8f);
        if(fightOccurring){
            BullseyeAttack = true;
        }
        yield return null;
    }


    #endregion

    #region Piece Based Behaviors
    void PawnBehavior(){
        tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.identity, lineParent);
        tempLine.GetComponent<LineScript>().SetUpLine("Pawn", this);
    }
    void RookBehavior(){
        int rando = Random.Range(0, 2);
        float rotation = rookAngles[rando];
        Vector3 quat = new Vector3(0f, 0f, rotation);
        tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
        tempLine.GetComponent<LineScript>().SetUpLine("Rook", this);
    }
    void KnightBehavior(){
        int rando = Random.Range(0, 4);
        float rotation = knightAngles[rando];
        Debug.Log(knightAngles[rando]);
        Vector3 quat = new Vector3(0f, 0f, rotation);
        tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
        tempLine.GetComponent<LineScript>().SetUpLine("Knight", this);
    }
    void BishopBehavior(){
        int rando = Random.Range(0, 2);
        float rotation = bishopAngles[rando];
        Vector3 quat = new Vector3(0f, 0f, rotation);
        tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
        tempLine.GetComponent<LineScript>().SetUpLine("Bishop", this);
    }
    void QueenBehavior(){
        int behavior = Random.Range(0, 2);
        if(behavior == 0){
            int rando = Random.Range(0, 2);
            float rotation = rookAngles[rando];
            Vector3 quat = new Vector3(0f, 0f, rotation);
            tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
            tempLine.GetComponent<LineScript>().SetUpLine("Rook", this);
        }
        else{
            int rando = Random.Range(0, 2);
            float rotation = bishopAngles[rando];
            Vector3 quat = new Vector3(0f, 0f, rotation);
            tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
            tempLine.GetComponent<LineScript>().SetUpLine("Bishop", this);
        }

    }
    void KingBehavior(){
        int rando = Random.Range(0, 8);
        float rotation = kingAngles[rando];
        Vector3 quat = new Vector3(0f, 0f, rotation);
        tempLine = Instantiate(linePrefab, lineParent.position, Quaternion.Euler(quat), lineParent);
        tempLine.GetComponent<LineScript>().SetUpLine("Pawn", this);
    }


    #endregion


    #region Animation Called Events

    public void OnSpawnAnim(){
        transform.position = Vector3.zero;
        anim.SetBool("Spawn", false);
        Debug.Log("okaydokay");
    }

    public void OnAttackAnim(){
        SpawnAttack();
        anim.SetBool("Attack", false);
    }

    public void OnHitAnim(){
        anim.SetBool("Hit", false);
    }

    public void OnDeathAnim(){

    }

#endregion

}
