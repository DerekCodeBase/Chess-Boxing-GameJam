using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullseyeScript : MonoBehaviour
{
    private FightManager fightManager;
    private Animator anim;
    private EnemyPieceScript relatedPiece;
    private Collider2D col;
    private SaveData playerSettings;

    private ContactFilter2D filter = new ContactFilter2D();
    private List<Collider2D> firstResults = new List<Collider2D>();
    private List<Collider2D> results = new List<Collider2D>();


    private bool shouldMoveUp = false;
    private bool shouldMoveDown = false;
    private bool shouldMoveRight = false;
    private bool shouldMoveLeft = false;
    private bool goodToDrawX = false;
    private bool goodToDrawY = false;
    private bool hasBeenDrawn = false;
    private bool doneWithIntro = false;

    private int counter = 0;

    private SpriteRenderer spRender;
    private Vector4 normalColor = new Vector4(255f, 255f, 255f, 255f);

    private Vector3 transPos;

    void Awake()
    {
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        filter.NoFilter();

        playerSettings = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveData>();
        anim.speed = playerSettings.AnimatorSpeed;

        
    }

    public void DrawBullseye(EnemyPieceScript enemy){
        relatedPiece = enemy;


        float spawnY = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        float spawnX = Random.Range
            (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
        transPos = new Vector3(spawnX, spawnY, 0f);
        transform.position = transPos;

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

    void Update()
    {
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
                anim.SetBool("Spawn", true);
                relatedPiece.BullseyeCooldown();
                Debug.Log("Bullseye Animating");
            }
        }
    }

    public void DestroyThisBullseye(bool hurtBot = false){
        if(hurtBot){
            fightManager.HurtBotPiece(1);
            Destroy(this.gameObject);
        }
        else{
            fightManager.HurtPlayerPiece(1);
            Destroy(this.gameObject);
        }
    }

    public void DestroyFromAnim(){
        DestroyThisBullseye();
    }

}

