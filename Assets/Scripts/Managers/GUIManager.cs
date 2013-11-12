using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public GUIText gameOverText, instructionsText;
	public GUIText HPTxt;
	
	private Player _player;
	private IngameUI drug;
	private IngameUI notebook;
	private IngameUI trombi;

	
	
	// Use this for initialization
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
	}
	
	// Update is called once per frame
	void Update () {
//		if(null) FindObjectOfType(typeof(GUIManager));
		if(Input.GetKeyDown(KeyCode.Space)){
			GameEventManager.TriggerGameStart();
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			GameEventManager.TriggerGameUnpause();
		}
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
	
	private void OnMouseOver()
	{
		print ("MouseOver" + this.name);	
	}
}
