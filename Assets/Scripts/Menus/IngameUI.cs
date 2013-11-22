using UnityEngine;
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
	private PlayerSim _Player;
	// Use this for initialization
	
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
		_Player = GameObject.Find("PlayerData").GetComponent<PlayerSim>();
		childSpr = GetComponentInChildren<OTSprite>();
		
		exists = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (DialogUI.exists == false)
		{
			switch (_Player.numberDrugs)
			{

				case 1 :
				{
					GameObject.Find("Drugs").GetComponentInChildren<OTSprite>().renderer.enabled = true;
					break;
				}
				case 2 :
				{	
					GameObject.Find("Drugs").GetComponentInChildren<OTSprite>().renderer.enabled = true;
					GameObject.Find("Drugs2").GetComponentInChildren<OTSprite>().renderer.enabled = true;
					break;
				}

				default :
				{
					GameObject.Find("Drugs").GetComponentInChildren<OTSprite>().renderer.enabled = false;
					GameObject.Find("Drugs2").GetComponentInChildren<OTSprite>().renderer.enabled = false;
					break;
				}
			}
		}
	}
	private void OnMouseOver()
	{
	}
	
	private void OnMouseDown()
	{
		switch (action)
		{
			case (ListAction.DisplayTrombi) :
			{
				MenuUI.destroyMenu();
				prefabSprite = Resources.Load("03UI/Trombi");
				Instantiate(prefabSprite);
				GameEventManager.TriggerGamePause();
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
	private void checkExistingMenu()
	{
		
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
	}
	public static void createIngameUI()
	{
		Object prefabSprite = Resources.Load("03UI/IngameUI");
		Instantiate(prefabSprite);
		exists = true;
	}
}
