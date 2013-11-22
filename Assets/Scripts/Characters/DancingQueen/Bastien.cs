﻿using UnityEngine;
using System.Collections;

public class Bastien : CharSim 
{
	public bool refusedMission = false;
	public bool hasSpokenOncePlayer = false;

	void OnMouseDown()
	{
		if (DialogUI.exists != true && GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>().hasTalkedThomas == true  && refusedMission == false )
		{
			DialogUI.createDialog(this);
			IngameUI.destroyIngameUI();
			MasterAudio.PlaySound("010_Bastien_00","0" + characterID + "_" + charac.ToString() + "_click" );
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
