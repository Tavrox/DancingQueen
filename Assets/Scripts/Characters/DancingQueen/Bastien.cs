using UnityEngine;
using System.Collections;

public class Bastien : CharSim 
{
	public bool refusedMission = false;
	public bool hasSpokenOncePlayer = false;
	public bool acceptedMission = false;
	public bool succeedMission = false;

	void OnMouseDown()
	{
		if (DialogUI.exists != true && GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>().hasTalkedThomas == true  && refusedMission == false )
		{
			DialogUI.createDialog(this);
			IngameUI.destroyIngameUI();
			MasterAudio.PlaySound("010_Bastien_00",1f,1f ,0f,"0" + characterID + "_" + charac.ToString() + "_click" );
		}
	}
	void OnMouseOver()
	{
		if(DialogUI.exists != true  && GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>().hasTalkedThomas == true && refusedMission == false )
		{
			OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
			spr.frameName = "cursor_talk";
		}
	}
}
