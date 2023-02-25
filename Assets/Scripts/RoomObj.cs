using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObj : MonoBehaviour
{
    public bool ServiceAgent;
    public bool Doorway;
    public bool Opponent;
    public GameObject TextBox;
    public DialogueScript Dialogue;

    public RewardGiver reward;
    public RewardGiver altReward;

    public string[] LinesToPass;
    public string[] AltLinesToPass;

    public int[] WhenToSwitch;
    public int[] SpriteToSwitchTo;

    public int[] AltWhenToSwitch;
    public int[] AltSpriteToSwitchTo;

    public bool isDoor;

    public FightManager fightManager;

    public Animator Vision;


    public void ClickObject(){
        if(fightManager.ShouldMoveOn){
            if(isDoor){
                fightManager.ResetMoveOn();
                MoveToNextRoom();
            }
            else{
                Dialogue.transform.parent.gameObject.SetActive(true);
                TextBox.SetActive(true);
                Dialogue.StartDialogue(AltLinesToPass, AltWhenToSwitch, AltSpriteToSwitchTo);
            }
        }
        else{
            Debug.Log("Door Clicked");
            Dialogue.transform.parent.gameObject.SetActive(true);
            Dialogue.StartDialogue(AltLinesToPass, AltWhenToSwitch, AltSpriteToSwitchTo);
            TextBox.SetActive(true);
        }
    }
    
    void MoveToNextRoom(){
        reward.RewardPlayer();
        
        Vision.SetTrigger("SwitchRooms");

        reward.ResetReward();
        altReward.ResetReward();

    }
}
