using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharSim : MonoBehaviour {
	
	private Cursor cursor;
	private OTSprite full_body;
	private OTSprite dialog_pic;
	private DialogDisplayer dial;
	private LevelManager _LevMan;

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
		Raphael,	// ID 13
		Kara,		// ID 04
		Girls,	// ID 01
		Boys,		// ID 03
		Dominique,		// ID 20
		Jeremie,		// ID 23
		Alice		// ID XX
	};
	public string characterID;
	public charList charac;
	public string dialToTrigger = "";
	public int sympathy_score = 0;
	public bool triggeredUltimate = false;
	public bool voteForPlayer = false;
	public int[] banAnswers = new int[20];
	public Color colorDialogs = new Color(1f,1,1f,1f);
	public bool tutoMode = false;
	public bool dialDisabled = false;
	private List<string> banList;
	private bool isSpeaking;
	
	public string whisperSound;
	public int minRandomVarWhispers, maxRandomVarWhispers;
	[Range (0,5)] public float randomDelayMin = 0f;
	[Range (0,5)] public float randomDelayMax = 0.01f;
	[Range (0,5)] public float frequencyWhispers = 0.5f;

	// Use this for initialization
	void Start () 
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;

		banList = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>().banAnswers;
		_LevMan = GameObject.Find("Level Manager").GetComponent<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (sympathy_score >= 100)
		{
			voteForPlayer = true;
		}
		checkBanAnswers(banList);
	}

	private void checkBanAnswers(List<string> listBanAnswer)
	{


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

	}

	private void GameDialog()
	{

	}

	private void OnMouseOver()
	{
		_LevMan = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		if(DialogUI.exists != true && dialDisabled != true && _LevMan.eventHappening != true && IngameUI.trombiTrigg == false)
		{
			OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
			spr.frameName = "cursor_talk";
		}
	}
	
	private void OnMouseExit()
	{
		OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
		spr.frameName = "cursor_default";
	}
	private void OnMouseDown()
	{
		_LevMan = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		if (DialogUI.exists != true && dialDisabled != true && _LevMan.eventHappening != true && IngameUI.trombiTrigg == false)
		{
			DialogUI.createDialog(this);
			IngameUI.destroyIngameUI();
//			MasterAudio.PlaySound("010_Bastien_00","0" + characterID + "_" + charac.ToString() + "_click" );
		}
	}
		
	public void playWhispers(string characID, string charName)
	{
		int rand = Random.Range(minRandomVarWhispers,maxRandomVarWhispers);
		float randDelay = Random.Range(randomDelayMin,randomDelayMax);
		string charStr = "0" + characID + "_" + charName;
		string transfRand;
		if (rand < 10)
		{
			transfRand = "0" + rand.ToString();
		}
		else
		{
			transfRand = rand.ToString();
		}
		PlaySoundResult psr = null;
		if (isSpeaking == false)
		{
			psr = MasterAudio.PlaySound(charStr, 1f, 1f, randDelay, charStr + "_" + transfRand );
		}
		if (psr != null)
		{
			if (psr.SoundPlayed == true)
			{
				isSpeaking = true;
			}
		}
//		print ("Playing group : "  + charStr);
//		print ("Playing delay : "  + randDelay);
//		print ("Playing sound variation : "  + charStr + "_" + transfRand);
	}

	public string getCharFrame(CharSim.charList charac)
	{
		string res ="";
		switch (charac)
		{
			case (charList.Vanessa) :
			{
				res = "vanessa_body";
				break;
			}
			case (charList.Bastien) :
			{
				res = "bastien_body";
				break;
			}
			case (charList.Boris) :
			{
				res = "boris_body";
				break;
			}
			case (charList.Bob) :
			{
				res = "bob_body";
				break;
			}
			//
			case (charList.Alex) :
			{
				res = "alex_body";
				break;
			}
			case (charList.Chloe) :
			{
				res = "chloe_body";
				break;
			}
			case (charList.Charlie) :
			{
				res = "charlie_body";
				break;
			}
			case (charList.Didier) :
			{
				res = "didier_body";
				break;
			}
			case (charList.Paul) :
			{
				res = "paul_body";
				break;
			}
			case (charList.Thomas) :
			{
				res = "thomas_body";
				break;
			}
			case (charList.Yannick) :
			{
				res = "yannick_body";
				break;
			}
			case (charList.Stephane) :
			{
				res = "stephane_body";
				break;
			}
			case (charList.Claire) :
			{
				res = "claire_body";
				break;
			}
				
			case (charList.Christine) :
			{
				res = "christine_body";
				break;
			}
				
			case (charList.Manon) :
			{
				res = "manon_body";
				break;
			}
				
			case (charList.Raphael) :
			{
				res = "raphael_body";
				break;
			}
			case (charList.Girls) :
			{
				res = "GroupeVanessaChloé";
				break;
			}
			case (charList.Boys) :
			{
			res = "GroupeBoris";
				break;
			}
			
		}
		return res;
	}



}
