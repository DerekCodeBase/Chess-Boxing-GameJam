using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardGiver : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spRender;
    private SaveData saveData;

    private Vector4 normalColor = new Vector4(255f, 255f, 255f, 255f);
    private Vector4 totalAlpha = new Vector4(0f, 0f, 0f, 0f);

    private int reward;

    void Start(){
        spRender = GetComponent<SpriteRenderer>();
        SetRewardActive(false);
        saveData = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveData>();

        reward = Random.Range(0, 3);
        
    }

    void Update(){

    }
    public void ResetReward(){
        SetRewardActive(false);
        reward = Random.Range(0, 3);

    }
    public void SetRewardActive(bool setActive){
        if(setActive){
            spRender.color = normalColor;
        }
        else{
            spRender.color = totalAlpha;
        }
    }

    public void RewardPlayer(){
        saveData.PassReward(reward);
    }
}
