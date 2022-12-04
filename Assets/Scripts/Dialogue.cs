﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum DialogueType
{
Text,
Continue,
Done
}


//All of this code needs to be overhauled
public class Dialogue : Singleton<Dialogue> {
	private string str;
	public float scrollequation;
	public static string Name, opponentName;
	public GameObject DialogueBox;
	public string stringToReveal;
	public bool fastText;
	public CustomText dialoguetext;
	public GameObject indicator;
	public bool deactivated;
	public GameObject subdialogue;
	public int taskType;
	public bool finishedText;
	public Image box;
	public bool finishedThePrompt;
    public bool playSoundAfterText = false;
    public AudioClip clipToPlay;
	public GameCursor cursor;
	public int selectedOption;
	public GameObject yesnomenu, slotsmenu, buycoinsmenu;
    public CustomText[] buycoinstext;
    string laststring;
    public bool keepTextOnScreen;
    public UnityEvent onFinishText;
    public DialogueType currentDialogueType;
    public bool waitForButtonPress; //wait for button press before closing the textbox
    public bool needButtonPress = true;
    public FontAtlas fontAtlas;

    
    
	void Start(){
        subdialogue.SetActive(true);
		finishedThePrompt = true;
		finishedText = true;
		box.enabled = false;
        indicator.SetActive(false);
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);

	}


	IEnumerator AnimateText(string strComplete){
        InputManager.dialogueCheck = true;
		box.enabled = true;
		dialoguetext.enabled = true;
        dialoguetext.text = "";
        dialoguetext.gameObject.SetActive(true);
		indicator.SetActive (false);

	    strComplete = strComplete.Replace("<player>",GameData.instance.playerName).Replace("<rival>",GameData.instance.rivalName).Replace("#MON","POKéMON").Replace("&l","\n\n").Replace("&c","").Replace("&p","");
		int i = 0;
		if(currentDialogueType != DialogueType.Done) str = "";
        else str = stringToReveal;

        if(currentDialogueType == DialogueType.Continue){
            str = laststring + "\n\n";
        }
		
		dialoguetext.text = str;

		if(currentDialogueType != DialogueType.Done){
            if(fastText){
                str += strComplete;
                dialoguetext.text = str;
                i = strComplete.Length;
            }
		    
            while( i < strComplete.Length ){
				yield return new WaitForEndOfFrame();
			    if(i < strComplete.Length - 1 && strComplete.Substring(i,2) == "\n\n"){ //are we at a double line break?
                    str += strComplete.Substring(i,2);
                    i += 2;
                }
                else if(i < strComplete.Length - 1 && strComplete[i] == '<'){ //is the current character a left bracket?
                    foreach(BracketChar bracketChar in fontAtlas.bracketChars){
                        string currentBracketChar = "<" + bracketChar.name + ">";
                        if(strComplete.Substring(i).IndexOf(currentBracketChar) == 0){ //is the current bracket expression detected the current entry?
                            str += strComplete.Substring(i,currentBracketChar.Length);
                            i += currentBracketChar.Length;
                            break;
                        }
                    }
                }	
			    else str += strComplete[i++]; //if not just add the current character
			    dialoguetext.text = str;

			    if (!fastText) {
				    yield return new WaitForSeconds (scrollequation);
			    }
		    }
	    }

		stringToReveal = str;
	}

	
	// Update is called once per frame
	void Update(){  
		if (deactivated && !Player.instance.isDisabled) {
			Player.instance.isDisabled = true;
		}

		buycoinstext [0].text = GameData.instance.money.ToString ();
		buycoinstext [1].text = GameData.instance.coins.ToString ();
		if (!finishedThePrompt) {

                if (InputManager.Pressed(Button.A)) {
					finishedThePrompt = true;
					StopAllCoroutines ();
                    InputManager.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
				}
                if (InputManager.Pressed(Button.B)) {
                switch (taskType)
                {
                    case 0:
                        selectedOption = 1;
                        break;
                    case 1:
                        selectedOption = 3;
                        break;
                }
					StopAllCoroutines ();
                    InputManager.dialogueCheck = false;
					dialoguetext.text = "";
                    cursor.SetActive(false);
					finishedThePrompt = true;
				}

				


                if (InputManager.Pressed(Button.Down)) {
					selectedOption++;
                switch (taskType)
                {
                    case 0:
                        MathE.Clamp(ref selectedOption, 0, 1);
                        cursor.SetPosition(120, 72 - 16 * selectedOption);
                        break;
                    case 1:
                        MathE.Clamp(ref selectedOption, 0, 2);
                        cursor.SetPosition(120, 40 - 16 * selectedOption);
                        break;
                }
                    
                }
                if (InputManager.Pressed(Button.Up)) {
					selectedOption--;
                switch (taskType)
                {
                    case 0:
                        MathE.Clamp(ref selectedOption, 0, 1);
                        cursor.SetPosition(120, 72 - 16 * selectedOption);
                        break;
                    case 1:
                        MathE.Clamp(ref selectedOption, 0, 2);
                        cursor.SetPosition(120, 40 - 16 * selectedOption);
                        break;
                }
               
				}
			} 
		 
		if (GameData.instance.textChoice == 2) {
            scrollequation = 3 * 0.016f;
		}
		if (GameData.instance.textChoice == 1) {
            scrollequation = 2f * 0.016f;
		}
		if (GameData.instance.textChoice == 0) {
            scrollequation = 1f * 0.016f;
		}
	}


public IEnumerator text(string text){
		finishedText = false;
        currentDialogueType = DialogueType.Text;
		stringToReveal = "";
        string[] lines = text.Split('\n');

        for(int i = 0; i < lines.Length; i++){
            if(i == 0) currentDialogueType = DialogueType.Text;

			//If the last line was a continue line, keep it in the text
            else if(lines[i-1].Contains("&c")){
                stringToReveal = lines[i-1];
                currentDialogueType = DialogueType.Continue;
            }
            else if(lines[i-1].Contains("&p")) currentDialogueType = DialogueType.Text; //eventually change to para?
			
			yield return StartCoroutine(AnimateText(lines[i]));
        	indicator.SetActive(true);

			laststring = lines[i].Replace("&c","").Replace("&p","");
			if(laststring.Contains("&l")){
				laststring = laststring.Substring(laststring.IndexOf("&l") + 2);
			}

        	if(needButtonPress){
        		while(!InputManager.Pressed(Button.A)){
					yield return new WaitForSeconds(0.001f);
        		    if(InputManager.Pressed(Button.A)){
        		        SoundManager.instance.PlayABSound();
						break;
					}
				}
        	}
        }

        yield return StartCoroutine(done());
	}

    public IEnumerator done(){
            indicator.SetActive(true);

            if(waitForButtonPress){
            	while (!InputManager.Pressed(Button.A)) {
					yield return new WaitForSeconds (0.001f);
            	    if (InputManager.Pressed(Button.A)) {
            	        SoundManager.instance.PlayABSound();
						break;
					}
				}
            }

            InputManager.dialogueCheck = false;
			
            if (!keepTextOnScreen){
                box.enabled = false;
                dialoguetext.text = "";
                dialoguetext.enabled = false;
                dialoguetext.gameObject.SetActive(false);
            }

            keepTextOnScreen = false;
            waitForButtonPress = false;
            needButtonPress = true;
            indicator.SetActive(false);
			finishedText = true;
			stringToReveal = "";
	}

	public IEnumerator prompt(){
		selectedOption = 0;
		taskType = 0;
        cursor.SetActive(true);
        cursor.SetPosition(120, 72 - 16 * selectedOption);
		finishedText = false;
		finishedThePrompt = false;
        yesnomenu.SetActive(true);
        while (!finishedThePrompt)
        {
            yield return new WaitForSeconds(0.01f);
            if (finishedThePrompt)
            {
                break;
            }
        }
        yesnomenu.SetActive(false);
    }

	public IEnumerator slots(){
		selectedOption = 0;
		taskType = 1;
        cursor.SetActive(true);
        cursor.SetPosition(120, 40 - 16 * selectedOption);
		finishedText = false;
		finishedThePrompt = false;
        slotsmenu.SetActive(true);
        while (!finishedThePrompt)
        {
            yield return new WaitForSeconds(0.01f);
            if (finishedThePrompt)
            {
                break;
            }
        }
        slotsmenu.SetActive(false);
        cursor.SetActive(false);

	}
    public void PlaySoundAfterText(AudioClip audioClip){ //function that sets a sound to be played after the next text command.
        playSoundAfterText = true;
        clipToPlay = audioClip;
    }

	public void Deactivate(){
		StopAllCoroutines();
		finishedText = true;
        InputManager.dialogueCheck = false;
		stringToReveal = "";
		box.enabled = false;
		dialoguetext.enabled = false;
        dialoguetext.gameObject.SetActive(false);
        indicator.SetActive(false);


	}

}
