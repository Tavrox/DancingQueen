using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {
	
	public enum ListMenu
	{
		Notebook,
		Trombinoscope,
		CloseMenu,
	}
	public ListMenu menu;
	public static bool exists;
	
	// Use this for initialization
	void Start () {
	
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
	private void OnMouseDown()
	{
		switch (menu)
		{
			case (ListMenu.CloseMenu) :
				{
					GameEventManager.TriggerGameUnpause();
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
		if (this == null)
		{
			switch (menu)
			{
				case (ListMenu.CloseMenu) :
					{
						
						break;
					}
				case (ListMenu.Trombinoscope) :
					{
						break;
					}
				case (ListMenu.Notebook) :
					{
						break;
					}
			}
		}
		
	}
	private void GameUnpause()
	{
		switch (menu)
		{
			case (ListMenu.CloseMenu) :
				{
					destroyMenu();
					break;
				}
			case (ListMenu.Trombinoscope) :
				{
//					Destroy(gameObject);
					break;
				}
			case (ListMenu.Notebook) :
				{
//					Destroy(gameObject);
					break;
				}
		}
	}
	
	private void GameDialog()
	{
		destroyMenu();
	}
	
	public static void destroyMenu()
	{
		GameObject target = GameObject.FindGameObjectWithTag("MenuUI");
		Destroy(target);
		exists = false;
	}
	
	public static void createMenu()
	{
		Object prefabSprite = Resources.Load("03UI/MenuUI");
		Instantiate(prefabSprite);
		exists = true;
	}
}
