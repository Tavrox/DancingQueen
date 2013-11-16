using UnityEngine;
using System.Collections;

public class CharSim : MonoBehaviour {
	
	private Cursor cursor;
	private OTSprite full_body;
	private OTSprite dialog_pic;
	private DialogDisplayer dial;
	public enum charList
	{
		Vanessa, 	// ID 07
		Bastien, 	// ID 10
		Boris, 		// ID 05
		Bob,		// ID 18
		Alex,		// ID 11
		Chloe,		// ID 12
		Charlie,	// ID 19
		Didier,		// ID 18
		Paul,		// ID 09
		Thomas,		// ID 08
		Yannick,	// ID 06
		Stephane,	// ID 15
		Anais,		// OBSOLETE
		Claire,		// ID 14
		Christine,	// ID 17
		Manon,		// ID 02
		Raphael		// ID 13
	};
	public string characterID;
	public charList charac;
	public int sympathy_score = 0;
	public bool triggeredUltimate = false;
	public bool voteForPlayer = false;
	
	public string whisperSound;
	public int minRandomVarWhispers, maxRandomVarWhispers;
	public int minRandomDelay, maxRandomDelay;
	[Range (0,5)] public float frequencyWhispers = 0.5f;

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
	
	
	public void lowerSympathy(int _val)
	{
		sympathy_score -= _val;
	}
	public void raiseSympathy(int _val)
	{
		sympathy_score += _val;
	}
	public void raiseGlobalSympathy(int _val)
	{
			// Machin.raiseSympa
	}
	public void lowerGlobalSympathy(int _val)
	{
	 	// Machin.lowerSympa
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
		collider.enabled = true;
	}
	
	private void GameDialog()
	{
		collider.enabled = false;
	}
	
	private void OnMouseDown()
	{
		if (DialogUI.exists != true)
		{
			DialogUI.createDialog(this);
//			MasterAudio.PlaySound(pathSoundGroup, pathSoundVariation);
		}
	}
	
	public charList getEnumChar()
	{
		return charList.Vanessa;
	}
		
	public void playWhispers()
	{
		int rand = Random.Range(minRandomVarWhispers,maxRandomVarWhispers);
		int randDelay = Random.Range(minRandomDelay,maxRandomDelay);
		string transfRand;
		if (rand < 10)
		{
			transfRand = "0" + rand.ToString();
		}
		else
		{
			transfRand = rand.ToString();
		}
		PlaySoundResult psr = MasterAudio.PlaySound("010_Bastien_00","0" + characterID + "_" + charac.ToString() + "_" + transfRand, randDelay);


//		Debug.Log ("0" + characterID + charac.ToString() + transfRand);
		Debug.Log(psr.ActingVariation);
		Debug.Log("Delay" + randDelay);
	}

	private void OnMouseOver()
	{
		OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
		spr.frameName = "cursor_talk";
		
	}
	
	private void OnMouseExit()
	{
		OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
		spr.frameName = "cursor_default";
	}

}
