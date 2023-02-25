using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPlay(){
        SceneManager.LoadScene("PlayScene");
    }

    public void OnQuit(){
        Application.Quit();
    }
}
