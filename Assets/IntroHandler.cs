using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    public GameObject Intro;
    public InputHandler input;
    public Camera boardCam;
    public Camera roomCam;

    public Animator boardAnim;



    public void CallFromAnim(){
        roomCam.enabled = false;
        boardCam.enabled = true;
        boardAnim.SetTrigger("ToBoard");
    }
}
