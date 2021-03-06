﻿using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {

	public enum ListAction
	{
		LaunchScene,
		DisplayTrombi,
		DisplayNotebook,
		MuteSound,
		ChangeLanguage,
		LowerSound,
		RaiseSound,
		FunStuff, // To do funny miscellaneous stuff in menus :)
	}
	public ListAction action;
	private Object prefabSprite;
	private OTSprite childSpr;
	public static bool exists;
	public static bool trombiTrigg;
	private PlayerSim _Player;
	// Use this for initialization
	
	void Start () 
	{
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
		_Player = GameObject.Find("PlayerData").GetComponent<PlayerSim>();
		childSpr = GetComponentInChildren<OTSprite>();
		
		exists = true;
	}
	private void OnMouseDown()
	{
		switch (action)
		{
			case (ListAction.DisplayTrombi) :
			{
				Debug.Log ("Trigger Trombinoscope");
				MenuUI.destroyMenu();
				prefabSprite = Resources.Load("03UI/Trombi");
				Instantiate(prefabSprite);
				GameEventManager.TriggerGamePause();
				trombiTrigg = true;
				break;
			}
			case (ListAction.DisplayNotebook) :
			{
				MenuUI.destroyMenu();
				prefabSprite = Resources.Load("03UI/Trombi");
				Instantiate(prefabSprite);
				GameEventManager.TriggerGamePause();
				break;
			}
			case (ListAction.ChangeLanguage) :
			{
				if (_Player.langChosen == PlayerSim.langList.en)
				{
					_Player.langChosen = PlayerSim.langList.fr;
					
					childSpr.frameName = "fr";
				}
				else
				{
					_Player.langChosen = PlayerSim.langList.en;
					childSpr.frameName = "en";
				}
				break;
			}
		}
	}
	private void GameUnpause()
	{
		trombiTrigg = false;
		
	}
	private void GameDialog()
	{
		destroyIngameUI();
	}
	public static void destroyIngameUI()
	{
		GameObject[] target = GameObject.FindGameObjectsWithTag("IngameUI");
		for (var i = 0; i < target.Length ; i++)
		{
			Destroy(target[i]);
			exists = false;
		}
		trombiTrigg = false;
	}
	public static void createIngameUI()
	{
		Object prefabSprite = Resources.Load("03UI/IngameUI");
		Instantiate(prefabSprite);
		exists = true;
	}
}
