using UnityEngine;
using System.Collections;

public class CharSim : MonoBehaviour {
	
	private Cursor cursor;

	// Use this for initialization
	void Start () {
	
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
	}
	
	// Update is called once per frame
	void Update () {
	
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
	
	private void OnMouseDown()
	{
		Object prefabSprite = Resources.Load("03UI/DialogDisplay");
		Instantiate(prefabSprite);
		GameEventManager.TriggerGameDialog();
	}
	
}
