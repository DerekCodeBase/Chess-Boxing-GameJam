using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public bool Tutorial = true;
    public int Difficulty = 2;


    public float BullseyeBuffer = 1.5f;
    public float AnimatorSpeed = 1f;

    public GameObject Room1Grid;
    public GameObject[] Room2Grids;
    public GameObject[] Room3Grids;
    public GameObject[] Room4Grids;

    public Transform GridTransform;


    void Awake(){
        SetBoard(Room1Grid);
    }
    public void SetBoard(GameObject grid){
        Instantiate(grid, GridTransform.position, Quaternion.identity, GridTransform);
    }

    public void SetTutorial(bool shouldEnable){
        if(shouldEnable){
            Tutorial = true;
        }
        else{
            Tutorial = false;
        }
    }

    public void SetDifficulty(int num){
        Difficulty = num;
    }

    public void SetAnimatorSpeed(float speed){
        AnimatorSpeed = speed;
    }


    public void PassReward(int reward){
        if(reward == 0){

        }
        else if(reward == 1){

        }
        else if(reward == 2){
            
        }
    }
}
