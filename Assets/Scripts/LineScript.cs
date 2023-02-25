using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineScript : MonoBehaviour
{
    public Camera fightCam;
    public GameObject[] FightLines;

    private Animator anim;
    private EnemyPieceScript relatedPiece;
    private GameObject thisLine;

    private FightManager fightManager;

    private Vector4 alphaColor = new Vector4(0f, 0f, 0f, 0f);
    private Vector4 normalColor;
    private SpriteRenderer spRender;
    private Vector3 transPos;

    private Collider2D col;
    private List<Collider2D> results = new List<Collider2D>();
    private List<Collider2D> firstResults = new List<Collider2D>();
    private string animBoolName;

    private ContactFilter2D filter = new ContactFilter2D();

    private bool shouldMoveUp = false;
    private bool shouldMoveDown = false;
    private bool shouldMoveRight = false;
    private bool shouldMoveLeft = false;
    private bool goodToDrawX = false;
    private bool goodToDrawY = false;
    private bool hasBeenDrawn = false;

    private bool doneWithIntro = false;

    private int counter = 0;
    

    void Awake(){
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();

        normalColor = spRender.color;
        spRender.color = alphaColor;

        filter.NoFilter();
        Debug.Log("Line Spawned");
        
    }

    public void SetUpLine(string piece, EnemyPieceScript enemy){
        animBoolName = piece;
        relatedPiece = enemy;
        float spawnY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        
        transPos = new Vector3(spawnX, spawnY, 0f);
        transform.position = transPos;
        Debug.Log(transPos);
        switch(piece){
            case "Pawn":
                thisLine = FightLines[0];
                break;
            case "Rook":
                thisLine = FightLines[1];
                break;
            case "Knight":
                thisLine = FightLines[2];
                break;
            case "Bishop":
                thisLine = FightLines[3];
                break;
        }
        thisLine.GetComponent<SpriteRenderer>().color = alphaColor;
        thisLine.SetActive(true);
        col = thisLine.GetComponent<Collider2D>();
        counter = col.OverlapCollider(filter, firstResults);
        if(counter > 0){
            for(int i = 0; i < counter; i++ ){
                if(firstResults[i].tag == "TopEdge"){
                    shouldMoveDown = true;
                }
                else if(firstResults[i].tag == "BottomEdge"){
                    shouldMoveUp = true;
                }
                else if(firstResults[i].tag == "RightEdge"){
                    shouldMoveLeft = true;
                }
                else if(firstResults[i].tag == "LeftEdge"){
                    shouldMoveRight = true;
                }
            }
        }
        else{
            goodToDrawX = true;
            goodToDrawY = true;
        }
        doneWithIntro = true;

    }

    void Update(){
        if(fightManager.DestroyAllAttacks)
        {
            Destroy(this.gameObject);
        }
        if(doneWithIntro){
            if(shouldMoveRight){
                shouldMoveRight = false;
                transform.position = new Vector3 (transform.position.x + .01f, transform.position.y, 0f);
                results.Clear();
                counter = col.OverlapCollider(filter, results);
                if(counter > 0){
                    for(int i = 0; i < counter; i++ ){
                        if(results[i].tag == "LeftEdge"){
                            shouldMoveRight = true;
                            Debug.Log(shouldMoveDown + "" + shouldMoveLeft +""+ shouldMoveUp +""+ shouldMoveRight);

                        }
                    }
                }
            }
            else if(shouldMoveLeft){
                shouldMoveLeft = false;
                transform.position = new Vector3 (transform.position.x - .01f, transform.position.y, 0f);
                results.Clear();
                counter = col.OverlapCollider(filter, results);
                if(counter > 0){
                    for(int i = 0; i < counter; i++ ){
                        if(results[i].tag == "RightEdge"){
                            shouldMoveLeft = true;
                                    Debug.Log(shouldMoveDown + "" + shouldMoveLeft +""+ shouldMoveUp +""+ shouldMoveRight);

                        }
                    }
                }
            }
            if(shouldMoveUp){
                shouldMoveUp = false;
                transform.position = new Vector3 (transform.position.x, transform.position.y + .01f, 0f);
                results.Clear();
                counter = col.OverlapCollider(filter, results);
                if(counter > 0){
                    for(int i = 0; i < counter; i++ ){
                        if(results[i].tag == "BottomEdge"){
                            shouldMoveUp = true;
                                    Debug.Log(shouldMoveDown + "" + shouldMoveLeft +""+ shouldMoveUp +""+ shouldMoveRight);

                        }
                    }
                }
            }
            else if(shouldMoveDown){
                shouldMoveDown = false;
                transform.position = new Vector3 (transform.position.x, transform.position.y - .01f, 0f);
                results.Clear();
                counter = col.OverlapCollider(filter, results);
                if(counter > 0){
                    for(int i = 0; i < counter; i++ ){
                        if(results[i].tag == "TopEdge"){
                            shouldMoveDown = true;
                            Debug.Log(shouldMoveDown + "" + shouldMoveLeft +""+ shouldMoveUp +""+ shouldMoveRight);

                        }
                    }
                }
            }

            if(!hasBeenDrawn && !shouldMoveDown && !shouldMoveRight && !shouldMoveUp && !shouldMoveLeft){
                counter = col.OverlapCollider(filter, firstResults);
                int shouldMoveCounter = 0;
                if(counter > 0){
                    for(int i = 0; i < counter; i++ ){
                        if(firstResults[i].tag == "TopEdge"){
                            shouldMoveDown = true;
                            shouldMoveCounter++;
                        }
                        else if(firstResults[i].tag == "BottomEdge"){
                            shouldMoveUp = true;
                            shouldMoveCounter++;
                        }
                        else if(firstResults[i].tag == "RightEdge"){
                            shouldMoveLeft = true;
                            shouldMoveCounter++;
                        }
                        else if(firstResults[i].tag == "LeftEdge"){
                            shouldMoveRight = true;
                            shouldMoveCounter++;
                        }
                    }
                }
                if(shouldMoveCounter == 0){
                    goodToDrawX = true;
                    goodToDrawY = true;
                }

            }


            if(!hasBeenDrawn && goodToDrawX == true && goodToDrawY == true){
                hasBeenDrawn = true;
                spRender.color = normalColor;
                anim.SetBool(animBoolName, true);
                Debug.Log("Set the anim and color");
            }
        }
    }

    void EnableLineFromAnim(){
        thisLine.GetComponent<SpriteRenderer>().color = normalColor;
        thisLine.transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = alphaColor;

    }



    public void DestroyThisLine(bool hurtBot, int damage){
        if(hurtBot){
            fightManager.HurtBotPiece(damage);
        }
        else{
            fightManager.HurtPlayerPiece(damage);
        }

        relatedPiece.AttackCooldown();

        Debug.Log("Destroying Line");
        Destroy(this.gameObject);
    }
}
