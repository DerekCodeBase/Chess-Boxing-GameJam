using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject[] HealthPieces;
    public GameObject PoisePrefab;

    private Vector4 totalAlpha = Vector4.zero; 
    private Vector4 normalColor = new Vector4(255f, 255f, 255f, 255f);




    public void SetInitialHealth(int health){
        int i = 0;
        foreach(GameObject piece in HealthPieces){
            if(i + 1 > health){
                piece.GetComponent<SpriteRenderer>().color = totalAlpha;
            }
            else{
                piece.GetComponent<SpriteRenderer>().color = normalColor;
            }
            i++;
        }
    }

    public void SetInitialPoise(int poise){
        int i = 0;
        foreach(GameObject piece in HealthPieces){
            if(i + 1 <= poise && piece.transform.childCount == 0){
                Instantiate(PoisePrefab, piece.transform.position, Quaternion.identity, piece.transform);
            }
            else if(piece.transform.childCount > 0 && i + 1 > poise){
                Destroy(this.transform.GetChild(i).gameObject);
            }
            i++;
        }
    }

    public void RemovePoise(int poise){
        int i = 0;
        foreach(GameObject piece in HealthPieces){
            if(piece.transform.childCount > 0 && i + 1 > poise){
                Destroy(piece.transform.GetChild(0).gameObject);
            }
            i++;
        }
    }

    public void RemoveHealth(int health){
        int i = 0;
        foreach(GameObject piece in HealthPieces){
            if(i + 1 > health){
                piece.GetComponent<SpriteRenderer>().color = totalAlpha;
            }
            else{
                piece.GetComponent<SpriteRenderer>().color = normalColor;
            }
            i++;
        }
    }

}
