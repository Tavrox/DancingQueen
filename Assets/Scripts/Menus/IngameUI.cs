using UnityEngine;
using System.Collections;

public class IngameUI : MonoBehaviour {
	
	public OTTextSprite label;
	public OTAnimatingSprite sprite;
	public enum ListAction
	{
		LaunchScene,
		DisplayTrombi,
		DisplayNotebook,
		MuteSound,
		LowerSound,
		RaiseSound,
		FunStuff, // To do funny miscellaneous stuff in menus :)
	}
	public ListAction action;
	private Object prefabSprite;
	public static bool exists;
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
				prefabSprite = Resources.Load("03UI/Notebook");
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
		GameObject target = GameObject.FindGameObjectWithTag("IngameUI");
		Destroy(target);
		exists = false;
	}
	public static void createIngameUI()
	{
		Object prefabSprite = Resources.Load("03UI/IngameUI");
		Instantiate(prefabSprite);
		exists = true;
	}
}
