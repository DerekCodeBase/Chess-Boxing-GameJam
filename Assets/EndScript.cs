using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public void OnQuit(){
        Application.Quit();
    }
}
