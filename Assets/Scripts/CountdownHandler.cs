using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownHandler : MonoBehaviour
{
    private FightManager fightManager;

    void Awake(){
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
    }

    void AnimFinishedT(){
        fightManager.StartFight();
        Destroy(this.gameObject);
    }
}
