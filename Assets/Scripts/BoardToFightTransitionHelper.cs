using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardToFightTransitionHelper : MonoBehaviour
{
    private FightManager fightManager;


    void Start()
    {
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallFromToFightAnimation(){
        fightManager.SwitchCamsToFight();
    }

    public void CallFromEndFightAnim(){
        fightManager.BackToBoard();

    }

    public void CallFromStartFightAnim(){
        fightManager.SpawnEnemy();
        GetComponent<Animator>().SetBool("FadeOff", false);

    }

    public void CallToStartUpBoard(){
        fightManager.StartUpBoard();
    }

    public void CallToStartUpRoom(){
        fightManager.CalledByToIntro();
    }
}
