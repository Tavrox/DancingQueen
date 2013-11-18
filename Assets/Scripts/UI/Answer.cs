using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour {
	
	public int id;
	public string choice_fr, choice_en, answerLine_fr, answerLine_en, ID_nextDialog, action, condition;
	public int sympathy_value;
	public Dialog nextDialog;
	private OTSprite spr;
	public bool triggered;
	public Object answer;
	
	
	// Use this for initialization
	void Start () {
		spr = GetComponentInChildren<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseOver()
	{
		if ( spr != null )
		{
			spr.frameName = "backbtn_hover";
		}
	}
	void OnMouseExit()
	{
		if ( spr != null )
		{
			spr.frameName = "backbtn";
		}
	}
	
	void OnMouseDown()
	{
		triggered = true;
	}
	void OnMouseUp()
	{
		triggered = false;
	}
	public void setNextDialog(string txt) {
		ID_nextDialog = txt;
	}
	public void setChoiceEN(string txt) {
		choice_en = txt;
	}
	public void setAnswerLineEN(string txt) {
		answerLine_en = txt;
	}
	public void setChoiceFR(string txt) {
		choice_fr = txt;
	}
	public void setAnswerLineFR(string txt) {
		answerLine_fr = txt;
	}
	public void setAction(string txt) {
		action = txt;
	}
	public void setCondition(string txt) {
		condition = txt;
	}
	public void setSympathyValue(int sco) {
		sympathy_value = sco;
	}
}
