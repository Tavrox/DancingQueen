using UnityEngine;
using System.Collections;

public class DialogUI : MonoBehaviour {
	
	public enum ListDialog
	{
		Overlay,
		CloseDialog,
		Answers,
		Dialogs
	}
	public ListDialog dialogItem;
	public static bool exists;
	private Object prefabSprite;
	
	
	// Use this for initialization
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
		
		exists = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		print ("Dialog exists ?" + exists);
	}
	
	private void OnMouseDown()
	{
		switch (dialogItem)
		{
			case (ListDialog.CloseDialog) :
			{
				destroyDialog();
				IngameUI.destroyIngameUI();
				MenuUI.destroyMenu();
				IngameUI.createIngameUI();
				break;
			}
		}
	}
	private void GameStart () 
	{
		
	}
	private void GameOver () 
	{
		
	}
	private void GamePause()
	{
		
	}
	private void GameUnpause()
	{
		
	}
	private void GameDialog()
	{
		
	}
	public static void destroyDialog()
	{
		GameObject[] target = GameObject.FindGameObjectsWithTag("DialogUI");

		MasterAudio.FadeOutAllOfSound("002_Manon", 1f);
		MasterAudio.FadeOutAllOfSound("004_Kara", 1f);
		MasterAudio.FadeOutAllOfSound("005_Bob", 1f);
		MasterAudio.FadeOutAllOfSound("006_Yannick", 1f);
		MasterAudio.FadeOutAllOfSound("007_Vanessa", 1f);
		MasterAudio.FadeOutAllOfSound("008_Thomas", 1f);
		MasterAudio.FadeOutAllOfSound("009_Paul", 1f);
		MasterAudio.FadeOutAllOfSound("010_Bastien", 1f);
		MasterAudio.FadeOutAllOfSound("011_Alex", 1f);
		MasterAudio.FadeOutAllOfSound("012_Chloe", 1f);
		MasterAudio.FadeOutAllOfSound("013_Raphael", 1f);
		MasterAudio.FadeOutAllOfSound("014_Claire", 1f);
		MasterAudio.FadeOutAllOfSound("015_Stephane", 1f);
		MasterAudio.FadeOutAllOfSound("016_Alice", 1f);
		MasterAudio.FadeOutAllOfSound("017_Christine", 1f);
		MasterAudio.FadeOutAllOfSound("018_Boris", 1f);
		MasterAudio.FadeOutAllOfSound("019_Charlie", 1f);
		MasterAudio.FadeOutAllOfSound("020_Dominique", 1f);
		MasterAudio.FadeOutAllOfSound("023_Jeremie", 1f);
		MasterAudio.FadeOutAllOfSound("025_Didier", 1f);


		for (var i = 0; i < target.Length ; i++)
		{
			Destroy(target[i]);
			exists = false;
		}
		IngameUI.createIngameUI();
	}
	public static void createDialog(CharSim _chosenChar, string _DialToTrigger = null)
	{
		DialogUI.exists = true;
		exists = true;
		GameEventManager.TriggerGameDialog();
		GameObject prefabSprite = Resources.Load("03UI/Dialog") as GameObject;
		if (_DialToTrigger != null)
		{
			_chosenChar.dialToTrigger = _DialToTrigger;
		}
		Instantiate(prefabSprite);
		LevelManager.currentCharacterSpeaking = _chosenChar;
	}
}
