using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour {
	
	public int id;
	public string choice, answerLine;
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
			spr.frameName = "02backbtn";
		}
	}
	void OnMouseExit()
	{
		if ( spr != null )
		{
			spr.frameName = "01backbtn";
		}
	}
	
	void OnMouseDown()
	{
		print (this.name + "triggered");
		triggered = true;
	}
}
