using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    public TextMeshProUGUI TextComponent;
    private string[] lines;
    public float textSpeed;
    public SpriteSwitcher Switcher;
    private InputHandler input;

    public GameObject TextBox;

    public Animator Vision;

    private bool intro;
    public string whichLines;
    private bool doorText;

    public FightManager fightManager;

    public DialogueText storedDialogue = new DialogueText();

    private int index;

    private int[] whenToSwitchSprites;

    private int[] spriteToSwitchTo;


    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
        input.SetTypeWriter(this);

        switch(whichLines){
            case "Intro":
                lines = storedDialogue.introArray;
                intro = true;
                StartDialogue(lines);
                break; 
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(intro){
            if(index < 3 || index == 4 || index == 6 || index == 11 || index == 17){
                Switcher.SetSprite(1);
            }
            else if(index == 10 || index == 12){
                Switcher.SetSprite(2);
            }
            else if(intro && index == 15){
                Vision.SetTrigger("FadeOut");
            }
            else if(index == 18){
                Switcher.SetSprite(3);
            }
            else{
                Switcher.SetSprite(0);
            }
        }
        else{
            if(whenToSwitchSprites.Length > 0){
                if(index < whenToSwitchSprites[0]){
                    Switcher.SetSprite(spriteToSwitchTo[0]);
                }
                else if(index < whenToSwitchSprites[1]){
                    Switcher.SetSprite(spriteToSwitchTo[1]);
                }
                else if(index < whenToSwitchSprites[2]){
                    Switcher.SetSprite(spriteToSwitchTo[2]);
                }
                else if(index < whenToSwitchSprites[3]){
                    Switcher.SetSprite(spriteToSwitchTo[3]);
                }
            }

        }
    }
    public void StartDialogue(string[] passedLines, int[] whenToSwitch = null, int[] spriteToSwitch = null){
        whenToSwitchSprites = whenToSwitch;
        spriteToSwitchTo = spriteToSwitch;
        lines = passedLines;
        input.SetTypeWriter(this);
        TextComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        foreach(char c in lines[index].ToCharArray()){
            TextComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine(){
        if(TextComponent.text == lines[index]){
            if(index < lines.Length - 1){
                index++;
                TextComponent.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else{
                TextComponent.text = string.Empty;
                TextBox.gameObject.SetActive(false);

                if(intro){
                    StartCoroutine(RoomToChessBoard());
                }
            }
        }
        else{
            StopAllCoroutines();
            TextComponent.text = lines[index];
        }
    }

    private IEnumerator RoomToChessBoard(){
        input.EnableDialogueControls(false);
        input.EnableBoardControls(true);
        yield return new WaitForSeconds(2f);
        Vision.SetTrigger("Blackout");
        this.gameObject.SetActive(false);

    }

    public void SetBoardInteractive(){

    }

}
