using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Underline", true);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Underline", false);    
    }


    void Update()
    {
        
    }

    public void OnExit(){
        Application.Quit();
    }

    public void OnStory(){
        Debug.Log("Loading Story Scene");
    }

    public void OnQuickMatch(){
        Debug.Log("Loading Quick Match");
    }

    public void OnSettings(){
        Debug.Log("settings");
    }
}
