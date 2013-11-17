﻿using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public OTSprite background;
	[SerializeField] private Player player;
	public static CharSim currentCharacterSpeaking;
	
	public bool thomasBattle = false;
	public bool chloeToilets = false;
	public bool vanessaSad = false;
	public bool raphaelSingle = false;
	public bool gameWon = false;
	public bool canGoToVIP = false;
	public string timeInterventionAlex_1;
	public string timeInterventionAlex_2;
	public string timeInterventionAlex_3;
	
	private GameObject _Alex;
	private GameObject _Anais;
	private GameObject _Bastien;
	private GameObject _Bob;
	private GameObject _Boris;
	private GameObject _Charlie;
	private GameObject _Chloe;
	private GameObject _Christine;
	private GameObject _Claire;
	private GameObject _Manon;
	private GameObject _Paul;
	private GameObject _Raphael;
	private GameObject _Stephane;
	private GameObject _Thomas;
	private GameObject _Vanessa;
	private GameObject _Yannick;
	private GameObject _Player;

	private GameObject[] wpChars;
	private GameObject[] wpDoors;

	public enum MusicList
	{
		ElectroBar,
		ElectroToilets,
		ElectroVIP,
		ElectroDancefloor,
		GroovyBar,
		GroovyToilets,
		GroovyVIP,
		GroovyDancefloor,
		SlowBar,
		SlowToilets,
		SlowVIP,
		SlowDancefloor,
		CountryBar,
		CountryToilets,
		CountryVIP,
		CountryDancefloor
	}
	public MusicList musicLvl;
	public int stepVotesForWin = 8;
	private string musicToPlay;
	private GUIText Timer;
	private int Hours = 20;
	private int Minutes = 0;
	private OTSprite black;

	private string backBarFrame;
	private string backDancefloorFrame;
	private string backToiletsFrame;
	private string backVIPFrame;
	
	public enum levelList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public levelList currentLvl;

	void Awake()
	{
		InvokeRepeating("updateTimer",0, 0.5f);
		setCharPrefab();
		setBackgrounds();
		setWaypoints();
		instantiateDoors();
		Screen.showCursor = false;
		setMusic();
		MasterAudio.TriggerPlaylistClip(musicToPlay);
		_Player = GameObject.Find("PlayerData");
	}

	// Use this for initialization
	void Start () 
	{
		black = GameObject.Find("Overlay").GetComponent<OTSprite>();
		_Player = GameObject.Find("PlayerData");
		Timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<GUIText>();

		setCharPos();

		DontDestroyOnLoad(this);
//		print (MasterAudio.TriggerPlaylistClip(musicName));
//		PlaylistController.PlayNextSong();
		//		PlaylistController.PlayNextSong();

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			raphaelSingle = true;
			if (raphaelSingle == true)
			{
				GameObject obj = GameObject.Find("Boris");
				print (obj.GetType());
				Destroy(obj);

			}
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			fadeToWhite();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			fadeToBlack();
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			Debug.Log(_Yannick.GetComponent<Yannick>().sympathy_score);
		}
		string MinutesTransfo;
		if (Minutes <= 10)
		{
			MinutesTransfo = "0" + Minutes.ToString();
		}
		else
		{
			MinutesTransfo = Minutes.ToString();
		}
		Timer.text = Hours.ToString() + ":" + MinutesTransfo;
	}

	void updateTimer()
	{
		Minutes +=1;
		if (Minutes == 60)
		{
			Hours+=1;
			Minutes = 0;
		}
	}

	void checkTimer()
	{
		if (Hours > 21)
		{
			if (DialogUI.exists != true)
			{
				if (_Alex.GetComponent<Alex>().casseCouilleS1 != true)
				{
					// Trigger Dialog
					_Alex.GetComponent<Alex>().casseCouilleS1 = true;
				}

			}
		}

		if (Hours > 22)
		{
			if (DialogUI.exists != true)
			{
				if (_Alex.GetComponent<Alex>().casseCouilleS2 != true && _Alex.GetComponent<Alex>().gotPlayerInVIP != true)
				{
					// Trigger Dialog
					_Alex.GetComponent<Alex>().casseCouilleS2 = true;
				}
				
			}
		}

		if (Hours > 23)
		{
			if (_Alex.GetComponent<Alex>().casseCouilleS3 != true && _Alex.GetComponent<Alex>().gotPlayerInVIP != true)
			{
				// Trigger Dialog
			}
		}

		if (Hours == 24)
		{
			setCharacGO("killAll");
			OTSprite gameo = GameObject.Find("Overlay").GetComponent<OTSprite>();
			OTSprite gameoLoose = GameObject.Find("Win").GetComponent<OTSprite>();
			OTSprite gameoWin = GameObject.Find("Loose").GetComponent<OTSprite>();
			gameo.alpha = 0.5f;

			if (calculateWin() == true)
			{
				gameo.frameName = "loose";

			}
			else
			{
				gameo.frameName = "win";
			}

		}

	}

	public void changeRoom ( levelList lv)
	{
		Debug.Log ("ChangeRoom");
		setWaypoints();
		_Player = GameObject.Find("PlayerData");
		_Player.GetComponent<PlayerSim>().reloadCharacs();
		currentLvl = lv;
		setCharacGO("");
		setCharPrefab();
		setBackgrounds();
		changeBackground();
		cleanDoors();
		instantiateDoors();
		setCharPos();
	}

	private void setCharacGO(string str)
	{
		_Alex 		= GameObject.FindGameObjectWithTag("Alex");
		_Anais 		= GameObject.FindGameObjectWithTag("Anais");
		_Bastien 	= GameObject.FindGameObjectWithTag("Bastien");
		_Bob 		= GameObject.FindGameObjectWithTag("Bob");
		_Boris 		= GameObject.FindGameObjectWithTag("Boris");
		_Charlie	= GameObject.FindGameObjectWithTag("Charlie");
		_Chloe 		= GameObject.FindGameObjectWithTag("Chloe");
		_Christine	= GameObject.FindGameObjectWithTag("Christine");
		_Claire 	= GameObject.FindGameObjectWithTag("Claire");
		_Manon 		= GameObject.FindGameObjectWithTag("Manon");
		_Paul 		= GameObject.FindGameObjectWithTag("Paul");
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael");
		_Stephane 	= GameObject.FindGameObjectWithTag("Stephane");
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas");
		_Vanessa 	= GameObject.FindGameObjectWithTag("Vanessa");
		_Yannick 	= GameObject.FindGameObjectWithTag("Yannick");

		if (str == "killAll")
		{
			_Alex.collider.enabled = false;
			_Anais.collider.enabled = false; 	
			_Bastien.collider.enabled = false; 
			_Bob.collider.enabled = false; 	
			_Boris.collider.enabled = false; 	
			_Charlie.collider.enabled = false;	
			_Chloe.collider.enabled = false; 	
			_Christine.collider.enabled = false;
			_Claire.collider.enabled = false; 
			_Manon.collider.enabled = false; 	
			_Paul.collider.enabled = false; 	
			_Raphael.collider.enabled = false; 
			_Stephane.collider.enabled = false; 
			_Thomas.collider.enabled = false; 
			_Vanessa.collider.enabled = false; 
			_Yannick.collider.enabled = false; 

		}
	}
	private void setCharPrefab()
	{
		_Yannick 	= Resources.Load("02Characters/Yannick") as GameObject;
		_Bastien	= Resources.Load("02Characters/Bastien") as GameObject;
		_Bob 		= Resources.Load("02Characters/Bob") as GameObject;
		_Chloe 		= Resources.Load("02Characters/Chloe") as GameObject;
		_Charlie 	= Resources.Load("02Characters/Charlie") as GameObject;
		_Christine 	= Resources.Load("02Characters/Christine") as GameObject;
		_Claire 	= Resources.Load("02Characters/Claire") as GameObject;
		_Manon 		= Resources.Load("02Characters/Manon") as GameObject;
		_Paul 		= Resources.Load("02Characters/Paul") as GameObject;
		_Raphael 	= Resources.Load("02Characters/Raphael") as GameObject;
		_Stephane 	= Resources.Load("02Characters/Stephane") as GameObject;
		_Thomas 	= Resources.Load("02Characters/Thomas") as GameObject;
		_Vanessa 	= Resources.Load("02Characters/Vanessa") as GameObject;
	}

	private bool setBackgrounds()
	{
		backBarFrame 	= "back_bar";
		backDancefloorFrame = "back_dancefloor";
		backToiletsFrame = "back_toilets";
		backVIPFrame = "back_vip";
		return true;
	}

	private void changeBackground()
	{
		if (currentLvl == LevelManager.levelList.Bar)
		{
			background.frameName = backBarFrame;
		}
		if (currentLvl == LevelManager.levelList.Dancefloor)
		{
			background.frameName = backDancefloorFrame;
		}
		if (currentLvl == LevelManager.levelList.Toilets)
		{
			background.frameName = backToiletsFrame;
		}
		if (currentLvl == LevelManager.levelList.VIP)
		{
			background.frameName = backVIPFrame;
		}
	}
	public string getBackground(levelList lv)
	{
		if (lv == LevelManager.levelList.Bar)
		{
			return (backBarFrame);
		}
		if (lv == LevelManager.levelList.Dancefloor)
		{
			return (backDancefloorFrame);
		}
		if (lv == LevelManager.levelList.Toilets)
		{
			return (backToiletsFrame);
		}
		if (lv == LevelManager.levelList.VIP)
		{
			return (backVIPFrame);
		}
		else
		{
			Debug.LogError("Erreur dans la matrice : retour de background");
			return (backBarFrame);
		}
	}

	private void instantiateDoors()
	{
		if (currentLvl == LevelManager.levelList.Bar)
		{
			GameObject doorPrefab = Resources.Load("06Levels/DoorBarToDance") as GameObject;
			Instantiate(doorPrefab);
		}
		if (currentLvl == LevelManager.levelList.Dancefloor)
		{
			GameObject doorPrefab = Resources.Load("06Levels/DoorDanceToVIP") as GameObject;
			Instantiate(doorPrefab);
			doorPrefab = Resources.Load("06Levels/DoorDanceToWC") as GameObject;
			Instantiate(doorPrefab);
			doorPrefab = Resources.Load("06Levels/DoorDanceToBar") as GameObject;
			Instantiate(doorPrefab);
		}
		if (currentLvl == LevelManager.levelList.Toilets)
		{
			GameObject doorPrefab = Resources.Load("06Levels/DoorWCToDance") as GameObject;
			Instantiate(doorPrefab);
		}
		if (currentLvl == LevelManager.levelList.VIP)
		{
			GameObject doorPrefab = Resources.Load("06Levels/DoorVIPToDance") as GameObject;
			Instantiate(doorPrefab);
		}
	}

	private void setWaypoints()
	{
		wpChars = new GameObject[15];
		wpDoors = new GameObject[5];

		// REMEMBER TO FIX ASSIGNEMENT

		// CHARS
//		wpChars[0] = new GameObject.Find("WP_Char_Default");
		wpChars[1] = GameObject.Find("WP_Char_Bastien");
		wpChars[2] = GameObject.Find("WP_Char_Bob");
		wpChars[3] = GameObject.Find("WP_Char_Boris");
		wpChars[4] = GameObject.Find("WP_Char_Charlie");
		wpChars[5] = GameObject.Find("WP_Char_Didier");
		wpChars[6] = GameObject.Find("WP_Char_Manon");
		wpChars[7] = GameObject.Find("WP_Char_Paul");
		wpChars[8] = GameObject.Find("WP_Char_Vanessa");

		// CHARS SPECIAL CASES
		wpChars[10] = GameObject.Find("WP_Char_Raphael_Dancefloor");
		wpChars[11] = GameObject.Find("WP_Char_Raphael_Toilets");
		wpChars[12] = GameObject.Find("WP_Char_Chloe_Bar");
		wpChars[13] = GameObject.Find("WP_Char_Chloe_Toilets");

		// DOORS
		wpDoors[0] =GameObject.Find("WP_Bar");
		wpDoors[1] =GameObject.Find("WP_Dancefloor");
		wpDoors[2] =GameObject.Find("WP_Toilets");
		wpDoors[3] =GameObject.Find("WP_VIP");
	}

	public void cleanDoors()
	{
		GameObject[] doors = GameObject.FindGameObjectsWithTag("Doors");
		for (var i = 0; i < doors.Length ; i++)
		{
			Destroy(doors[i]);
			Debug.LogWarning ("Door Destroyed");
		}

	}

	private void setCharPos()
	{

		// Check Current place
		// Check events for specific char places
		// Instantiate good chars, destroy wrong chars
		// Set Pos
		switch (currentLvl)
		{
			case (levelList.Dancefloor) :
			{
			
				hideItem("Comptoir");
				hideChar("Yannick");
				hideChar("Vanessa");
				hideChar("Chloe");
			
				hideChar("Boris");
				hideChar("Paul");
				hideChar("Charlie");
			
				unhideChar("Raphael");
				unhideChar("Bastien");
				unhideChar("Thomas");
				unhideChar("Didier");
				unhideChar("Manon");

			if (_Vanessa.GetComponent<Vanessa>().isSad == true)
				{

				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{

				}
				if (_Raphael.GetComponent<Raphael>().coupleClaire == false)
				{
					
				}
				break;
			}
			case (levelList.Bar) :
			{
				
				hideChar("Raphael");
				hideChar("Bastien");
				hideChar("Thomas");
				hideChar("Didier");
				hideChar("Manon");

				unhideItem("Comptoir");
				unhideChar("Yannick");
				unhideChar("Vanessa");
				unhideChar("Chloe");
				if (_Vanessa.GetComponent<Vanessa>().triggeredUltimate == true)
				{
				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{
				}
				break;
			}
			case (levelList.Toilets) :
			{
			
				hideChar("Raphael");
				hideChar("Bastien");
				hideChar("Thomas");
				hideChar("Didier");
				hideChar("Manon");

				unhideChar("Boris");
				unhideChar("Paul");
				unhideChar("Charlie");
			
			if (_Raphael.GetComponent<Raphael>().coupleClaire == true)
				{

				}
				break;
			}
			case (levelList.VIP) :
			{
			
				hideChar("Raphael");
				hideChar("Bastien");
				hideChar("Thomas");
				hideChar("Didier");
				hideChar("Manon");
			
			if (_Bob.GetComponent<Bob>().unlocked == true)
				{
					unhideChar("Bob");
				}
				break;
			}
		}
	}

	private void setMusic()
	{
		switch (musicLvl)
		{
			case (MusicList.SlowBar) :
			{
				musicToPlay = "SlowBar";
				break;
			}
			case (MusicList.GroovyBar) :
			{
				musicToPlay = "GroovyBar";
				break;
			}
			case (MusicList.CountryBar) :
			{
				musicToPlay = "GBar";
				break;
			}
			case (MusicList.ElectroBar) :
			{
				musicToPlay = "ElectroBar";
				break;
			}
		}
	}
	private void chooseMusic()
	{

	}
	private void fadeToBlack()
	{
		InvokeRepeating("raiseAlpha", 0f, 0.5f);
		DialogUI.exists = true;
	}

	private void fadeToWhite()
	{
		InvokeRepeating("lowerAlpha", 0f, 0.5f);
		DialogUI.exists = false;
	}

	IEnumerator Wait(float WaitTime)
	{
		yield return new WaitForSeconds(WaitTime);
	}

	private void raiseAlpha()
	{
		Debug.Log("Fade" + black.alpha);
		if (black.alpha < 1)
		{
			black.alpha += 0.2f;
		}
		else
		{
			CancelInvoke();
		}
	}

	private void lowerAlpha()
	{
		Debug.Log("Fade" + black.alpha);
 		if (black.alpha > 0)
		{
			black.alpha -= 0.2f;
		}
		else
		{
			CancelInvoke();
		}
	}
	private void hideChar(string gameo)
	{
		if (gameo != null)
		{
//			GameObject.FindGameObjectWithTag(gameo).GetComponent<CharSim>().enabled = false;
			GameObject.FindGameObjectWithTag(gameo).GetComponent<CharSim>().collider.enabled = false;
			GameObject.FindGameObjectWithTag(gameo).GetComponentInChildren<OTSprite>().renderer.enabled = false;
		}
	}

	private void unhideChar (string gameo)
	{
		if (gameo != null)
		{
//			GameObject.FindGameObjectWithTag(gameo).GetComponent<CharSim>().enabled = true;
			GameObject.FindGameObjectWithTag(gameo).GetComponent<CharSim>().collider.enabled = true;
			GameObject.FindGameObjectWithTag(gameo).GetComponentInChildren<OTSprite>().renderer.enabled = true;
		}

	}
	private void hideItem(string gameo)
	{
		GameObject.Find(gameo).GetComponentInChildren<OTSprite>().renderer.enabled = false;
	}
	private void unhideItem(string gameo)
	{
		GameObject.Find(gameo).GetComponentInChildren<OTSprite>().renderer.enabled = true;
	}
	private bool calculateWin()
	{
		setCharacGO("");
		int numberVotes = 0;
		bool res = false;
		if (_Paul.GetComponent<CharSim>().voteForPlayer == true)
		{

		}
		numberVotes += _Player.GetComponent<PlayerSim>().votesAdded;
		if (numberVotes > stepVotesForWin)
		{
			Debug.Log("WIN !");

		}
		else
		{
			Debug.Log("Loose");
		}
		Debug.Log(numberVotes);
		return res;
	}
}
