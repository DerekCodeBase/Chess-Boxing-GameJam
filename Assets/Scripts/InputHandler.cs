using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerControls playerControls;


    private PieceBehavior chosenPiece;
    private SquareScript chosenSquare;

    private HandleManager activeHandle;

    private DialogueScript TypeWriter;

    private FightManager fightManager;

    public Camera MainCam;
    public Camera FightCam;
    public Camera RoomCam;

    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerControls();
        EnableFightControls(false);
        EnableBoardControls(false);
        EnableDialogueControls(true);
        fightManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<FightManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerControls.BasicControls.Click.performed += _ => MyMouseClick(true);

        playerControls.FightControls.Click.performed += _ => FightMouseClick(true);
        playerControls.FightControls.Click.canceled += _ => FightMouseClick(false);

        playerControls.DialogueControls.Click.performed += _ => DialogueMouseClick();

        playerControls.RoomControls.Click.performed += _ => RoomMouseClick();


    }

    public void EnableBoardControls(bool toEnable){
        if(toEnable){
            Debug.Log("Enabled Board Controls");
            playerControls.BasicControls.Enable();
        }
        else{
            playerControls.BasicControls.Disable();
        }
    }

    public void EnableFightControls(bool toEnable){
        if(toEnable){
            playerControls.FightControls.Enable();
            Debug.Log("fight controls enabled");
        }
        else{
            playerControls.FightControls.Disable();
        }
    }

    public void EnableDialogueControls(bool toEnable){
        if(toEnable){
            playerControls.DialogueControls.Enable();
            Debug.Log("Dialogue controls enabled");
        }
        else{
            playerControls.DialogueControls.Disable();
        }
    }

    public void EnableRoomControls(bool toEnable){
        if(toEnable){
            playerControls.RoomControls.Enable();
            Debug.Log("Room controls enabled");
        }
        else{
            playerControls.RoomControls.Disable();
        }
    }



    private void MyMouseClick(bool isDown)
    {  
        Vector2 mousePos = MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero);
        chosenPiece = null;
        chosenSquare = null;

        if(hit.Length > 0){
            foreach(RaycastHit2D obj in hit){
                if(obj.collider.GetComponent<PieceBehavior>() != null){
                    chosenPiece = obj.collider.GetComponent<PieceBehavior>();
                    Debug.Log("Pieeecee");
                }
                else if(obj.collider.GetComponent<SquareScript>() != null){
                    chosenSquare = obj.collider.GetComponent<SquareScript>();
                    Debug.Log("Squaressss");
                }
            }
            if (chosenSquare != null){
                chosenSquare.OnClick();
            }
        }

        
    }

    private void FightMouseClick(bool isDown){
        Debug.Log("Heard Mouse Click");
        Vector2 mousePos = FightCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero);
        bool handleClicked = false;
        bool bullseyeClicked = false;
        activeHandle = null;


        if(hit.Length > 0){
            foreach(RaycastHit2D obj in hit){
                Debug.Log(obj.collider.name);
                if(obj.collider.GetComponent<HandleManager>() != null){
                    activeHandle = obj.collider.GetComponent<HandleManager>();
                    Debug.Log("Pieeecee");
                    handleClicked = true;
                }
                else if(obj.collider.GetComponent<BullseyeScript>()){
                    obj.collider.GetComponent<BullseyeScript>().DestroyThisBullseye(true);
                    bullseyeClicked = true;
                }
            }
            if(activeHandle != null && handleClicked){
                activeHandle.SetToMouse(isDown);
            }
            else if(!handleClicked && !bullseyeClicked && isDown){
                fightManager.HurtPlayerPiece(1);
                Debug.Log("HurtPlayerFromInput");
            }
        }


        

    }

    private void DialogueMouseClick(){
        TypeWriter.NextLine();
    }

    private void RoomMouseClick(){
        Debug.Log("RoomClick");
        Vector2 mousePos = RoomCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if(hit.Length > 0){
            foreach(RaycastHit2D obj in hit){
                Debug.Log(obj);
                if(obj.collider.GetComponent<RoomObj>() != null){
                    obj.collider.GetComponent<RoomObj>().ClickObject();
                }
            }
        }
    }

    public void SetTypeWriter(DialogueScript dialogue){
        TypeWriter = dialogue;
    }
}
