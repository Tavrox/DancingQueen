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
					destroyDialog();
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
		print ("OM2");
		for (var i = 0; i < target.Length ; i++)
		{
			print ("OMG");
			Destroy(target[i]);
			exists = false;
		}
	}
	public static void createDialog()
	{
		Object prefabSprite = Resources.Load("03UI/DialogOverlay");
		Instantiate(prefabSprite);
		exists = true;
	}
}
