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
	
	}
	
	private void OnMouseDown()
	{
		switch (dialogItem)
		{
			case (ListDialog.CloseDialog) :
				{
					IngameUI.createIngameUI();
					destroyDialog();
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
		GameObject target = GameObject.FindGameObjectWithTag("DialogUI");
		Destroy(target);
		exists = false;
	}
	public static void createDialog()
	{
		Object prefabSprite = Resources.Load("03UI/DialogDisplay");
		Instantiate(prefabSprite);
		exists = true;
	}
}
