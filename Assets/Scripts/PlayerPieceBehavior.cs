using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPieceBehavior : MonoBehaviour
{
    public Animator anim;
    public Sprite[] sprites;
    private SpriteRenderer spRender;

    private int poise;
    public int health;

    void Start(){
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(string piece, bool white = false){
        if(white){
            switch(piece){
                case "Pawn":
                    spRender.sprite = sprites[0];
                    break;
                case "Rook":
                    spRender.sprite = sprites[1];
                    break;
                case "Horsey":
                    spRender.sprite = sprites[2];
                    break;
                case "Bishop":
                    spRender.sprite = sprites[3];
                    break;
                case "Queen":
                    spRender.sprite = sprites[4];
                    break;
                case "King":
                    spRender.sprite = sprites[5];
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
                    break;
                case "Rook":
                    spRender.sprite = sprites[7];
                    break;
                case "Horsey":
                    spRender.sprite = sprites[8];
                    break;
                case "Bishop":
                    spRender.sprite = sprites[9];
                    break;
                case "Queen":
                    spRender.sprite = sprites[10];
                    break;
                case "King":
                    spRender.sprite = sprites[11];
                    break;
                default:
                    spRender.sprite = sprites[6];
                    Debug.Log("Setting Sprite Failed");
                    break;
            }
        }
        
    }

    public void SetHealth(int hp){
        health = health - hp;
    }
    public void SetInitialHealth(int hp){
        health = hp;
    }

    public void SpawnPlayer(){
        anim.SetBool("Spawn", true);

    }


    private void FixedUpdate(){

    }








#region Animation Called Events

    public void OnSpawnAnim(){
        transform.position = Vector3.zero;
        anim.SetBool("Spawn", false);
    }

    public void OnAttackAnim(){

        anim.SetBool("Attack", false);
    }

    public void OnHitAnim(){
        anim.SetBool("Hit", false);
    }

    public void OnDeathAnim(){

    }

#endregion

}
