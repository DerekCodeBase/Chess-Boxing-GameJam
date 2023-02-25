/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
	TMP_Text _tmpProText;
	string writer;
	public bool writing;
    public int disableAfter;
    private int disableCount = 0;
	private InputHandler input;
	public SpriteSwitcher Switcher;



    private bool outsideAnimation = false;


	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] bool leadingCharBeforeDelay = false;


	// Use this for initialization
	void Start()
	{
		input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputHandler>();
		_tmpProText = GetComponent<TMP_Text>()!;
        WriteText(lines[disableCount]);
		
	}

    void FixedUpdate(){
    }



    void WriteText(string toWrite){
        writer = toWrite;
        writing = true;
		if(disableCount < 2 || disableCount == 3 || disableCount == 5|| disableCount == 10 || disableCount == 16){
			Switcher.SetSprite(1);
		}
		else if(disableCount == 9 || disableCount == 11){
			Switcher.SetSprite(1);
		}
		else{
			Switcher.SetSprite(0);
		}
        StartCoroutine("TypeWriterTMP");
    }

    public void OnMouseClick(){
        Debug.Log("heard");
        if(!writing){
			Debug.Log("Should Write new");
            WriteText(lines[disableCount]);
        }
		else if(writing){
			timeBtwChars = 0f;
			Debug.Log("Skipping Text");
		}
    }

	IEnumerator TypeWriterTMP()
    {
		timeBtwChars = .1f;
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

		foreach (char c in writer)
		{
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		
        disableCount++;
        writing = false;
	}

	public void EnableThisWriter(bool enable){
		if(enable){
			input.SetTypeWriter(this);
		}
	}



    private string[] lines = {
	"What... Where am I?",
	"It's so dark in here, I can't see anything...",

	"Hi Ludwig.",
	"Coots?! What's going on?",
	"I heard you were exploiting me for content again.",
	"....",

	"Oh don't worry, you don't need to apologize. I've already gotten revenge, you see...",
	"I TURNED YOUR ASS INTO THE IRS!",
	"They've finally come for you, Ludwig. And now you're trapped.",

	"You turned me into the IRS?",
	"  ",
	"And you gave me FROSTED TIPS?!",

	"You gave YOURSELF frosted tips.",
	"It's just one of the many choices that led you to where you are now: the Basement of the IRS Headquarters.",
	"Anyways, I called because I'm getting hungry and Cutie won't feed me until you get home. So hurry up and get back here.",
	"*click*",

	"Is that a chessboard with boxing rings around it?",

	"Defeat your opponents to proceed through rooms. Be quick about it--Coots is hangry!"
	};




}*/