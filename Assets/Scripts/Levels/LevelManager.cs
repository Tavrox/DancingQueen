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
	private OTSprite black;

	public enum MusicList
	{
		Electro,
		Groovy,
		Country,
		Slow
	}
	public MusicList musicLvl = MusicList.Groovy;
	public enum levelList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public levelList currentLvl;

	public int stepVotesForWin = 8;
	public float updateTimerEvery = 1.35f;
	private string musicToPlay;
	private GUIText Timer;
	public int Hours = 20;
	public int Minutes = 0;
	private string musicStyle;

	private string backBarFrame;
	private string backDancefloorFrame;
	private string backToiletsFrame;
	private string backVIPFrame;


	void Awake()
	{
		black = GameObject.Find("Overlay").GetComponent<OTSprite>();
		InvokeRepeating("updateTimer",0, updateTimerEvery);
		setCharPrefab();
		setBackgrounds();
		setWaypoints();
		instantiateDoors();
		Screen.showCursor = false;
		setMusic(musicLvl, levelList.Bar);
		_Player = GameObject.Find("PlayerData");
	}

	// Use this for initialization
	void Start () 
	{
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
		setCharacGO("");
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
		if (Minutes < 10)
		{
			MinutesTransfo = "0" + Minutes.ToString();
		}
		else
		{
			MinutesTransfo = Minutes.ToString();
		}
		if ( _Thomas.GetComponent<Thomas>().isBattleDance == true && currentLvl == levelList.Dancefloor)
		{
			hideChar("Thomas");
		}
		if ( _Vanessa.GetComponent<Vanessa>().isSad == false && currentLvl == levelList.Dancefloor)
		{
			unhideChar("Vanessa");
		}
		Timer.text = Hours.ToString() + ":" + MinutesTransfo;
		checkTimer();
		checkClaireState();
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

	private void checkClaireState()
	{
		Claire Claire = GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
		Raphael Raph = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		// Music
		// Player kissed Raphael
		// Player spoken to Raphael
		
		if (Raph.hasTalkedRaphael == true && Claire.talkedAboutFlirting == false)
		{
			if (DialogUI.exists != true)
			{
				Claire.dialToTrigger = "11006";
				IngameUI.destroyIngameUI();
				DialogUI.createDialog(Claire);
				Claire.talkedAboutFlirting = true;
			}
		}
		if (Raph.kissedPlayer == true && Claire.talkedAboutKissing == false)
		{
			if (DialogUI.exists != true)
			{
				Claire.dialToTrigger = "11006";
				IngameUI.destroyIngameUI();
				DialogUI.createDialog(Claire);
				Claire.talkedAboutKissing = true;
			}
		}


	}

	void checkTimer()
	{
		Alex GO = GameObject.FindGameObjectWithTag("Alex").GetComponent<Alex>();
		print ("CassCo1" + GO.casseCouilleS1);
		print ("CassCo2" + GO.casseCouilleS2);
		print ("CassCo3" + GO.casseCouilleS3);
		print ("Dialog State" + DialogUI.exists);
		if (Hours >= 21)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS1 != true)
				{
					IngameUI.destroyIngameUI();
					DialogUI.createDialog(GO);
					GO.casseCouilleS1 = true;
				}

			}
		}

		if (Hours > 22)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS2 == false && GO.gotPlayerInVIP != true && GO.casseCouilleS1 == true)
				{
					IngameUI.destroyIngameUI();
					DialogUI.createDialog(GO, "11006");
					GO.casseCouilleS2 = true;
				}
				
			}
		}

		if (Hours > 23)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS3 == false && GO.gotPlayerInVIP == false  && GO.casseCouilleS3 == true)
				{
					IngameUI.destroyIngameUI();
					DialogUI.createDialog(GO);
					GO.casseCouilleS3 = true;
				}
				
			}
		}

		if (Hours == 24)
		{
			fadeToBlack();
			setCharacGO("killAll");
			OTSprite gameo = GameObject.Find("Overlay").GetComponent<OTSprite>();
			OTSprite gameoLoose = GameObject.Find("Win").GetComponent<OTSprite>();
			OTSprite gameoWin = GameObject.Find("Loose").GetComponent<OTSprite>();
			gameo.alpha = 1f;

			if (calculateWin() == true)
			{
				gameo.frameName = "win";

			}
			else
			{
				gameo.frameName = "loose";
			}

		}

	}

	public void changeRoom ( levelList lv)
	{
		Debug.Log ("ChangeRoom");
		currentLvl = lv;
		setWaypoints();
		_Player = GameObject.Find("PlayerData");
		_Player.GetComponent<PlayerSim>().reloadCharacs();
		setCharacGO("");
		setCharPrefab();
		setBackgrounds();
		changeBackground();
		cleanDoors();
		instantiateDoors();
		setCharPos();
		setMusic(musicLvl, lv);
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
		print ("Change pos char");
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

				if (_Vanessa.GetComponent<Vanessa>().isSad == false)
				{
					print ("instantiate thomas bar");
					_Vanessa.transform.position = new Vector3(-2.350567f, 0.5271564f,0);
					unhideChar("Vanessa");
				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{
					print ("hide thomas dance");
					hideChar("Thomas");
				}
				if (_Raphael.GetComponent<Raphael>().coupleClaire == false)
				{
					
				}
				break;
			}
			case (levelList.Bar) :
			{
				
				setCharacGO("Thomas");

				hideChar("Raphael");
				hideChar("Bastien");
				hideChar("Didier");
				hideChar("Manon");
				hideChar("Thomas");

				unhideItem("Comptoir");
				unhideChar("Yannick");
				unhideChar("Vanessa");
				unhideChar("Chloe");
				hideChar("Thomas");
			       
				if (_Vanessa.GetComponent<Vanessa>().isSad == false)
				{
					print ("Hide vanessa bar");
					hideChar("Vanessa");
				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{
					_Thomas.GetComponentInChildren<OTSprite>().frameName = "thomas_fullbar";
					_Thomas.transform.position = new Vector3(-2.060373f, 0.8314278f, 3f);
					_Thomas.transform.localScale = new Vector3(0.4477435f, 0.4477435f, 0.4477435f);
					print(_Thomas.GetComponentInChildren<OTSprite>().frameName);
					print ("Unhide thomas bar");
					unhideChar("Thomas");
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
			
				hideChar("Bob");
			
				if (_Bob.GetComponent<Bob>().unlocked == true)
				{
					unhideChar("Bob");
				}
				
				if ( _Alex.GetComponent<Alex>().gotPlayerInVIP == true)
			    {
				Debug.Log("Isolate player");
					IngameUI.destroyIngameUI();
					DialogUI.createDialog(_Alex.GetComponent<Alex>());
				}
				break;
			}
		}
	}

	public void setMusic(MusicList music, levelList level)
	{

		if (level == levelList.Bar)
		{
			if (music ==  MusicList.Groovy)
			{
				musicToPlay = "GroovyBar";

			}
			if (music ==  MusicList.Electro)
			{
				musicToPlay = "ElectroBar";
				
			}
			if (music ==  MusicList.Country)
			{
				musicToPlay = "CountryBar";
				Vanessa go = GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
				go.dialToTrigger = "7009";
				go.isSad = false;
			}
			if (music ==  MusicList.Slow)
			{
				musicToPlay = "SlowBar";
				
			}
		}
		if (level == levelList.Dancefloor)
		{
			if (music ==  MusicList.Groovy)
			{
				musicToPlay = "GroovyDancefloor";
				
			}
			if (music ==  MusicList.Electro)
			{
				musicToPlay = "ElectroDancefloor";
				
			}
			if (music ==  MusicList.Country)
			{
				musicToPlay = "CountryDancefloor";
				Vanessa go = GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
				go.dialToTrigger = "7009";
				go.isSad = false;
			}
			if (music ==  MusicList.Slow)
			{
				musicToPlay = "SlowDancefloor";
				
			}
		}
		if (level == levelList.Toilets)
		{
			if (music ==  MusicList.Groovy)
			{
				musicToPlay = "GroovyToilets";
				
			}
			if (music ==  MusicList.Electro)
			{
				musicToPlay = "ElectroToilets";
				
			}
			if (music ==  MusicList.Country)
			{
				musicToPlay = "CountryToilets";
				Vanessa go = GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
				go.dialToTrigger = "7009";
				go.isSad = false;
			}
			if (music ==  MusicList.Slow)
			{
				musicToPlay = "SlowToilets";
				
			}
		}
		if (level == levelList.VIP)
		{
			if (music ==  MusicList.Groovy)
			{
				musicToPlay = "GroovyVIP";
				
			}
			if (music ==  MusicList.Electro)
			{
				musicToPlay = "ElectroVIP";
				
			}
			if (music ==  MusicList.Country)
			{
				Vanessa go = GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
				go.dialToTrigger = "7009";
				go.isSad = false;
				musicToPlay = "CountryVIP";
				
			}
			if (music ==  MusicList.Slow)
			{
				musicToPlay = "SlowVIP";
			}
		}
		MasterAudio.TriggerPlaylistClip(musicToPlay);
		
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
	private void raiseAlpha()
	{
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
 		if (black.alpha > 0)
		{
			black.alpha -= 0.2f;
		}
		else
		{
			CancelInvoke();
		}
	}
	IEnumerator Wait(float WaitTime)
	{
		yield return new WaitForSeconds(WaitTime);
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
	public bool calculateWin()
	{
		setCharacGO("");
		int numberVotes = 0;
		bool res = false;
		if (_Paul.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Alex.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Vanessa.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Boris.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Bob.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Bastien.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Thomas.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Yannick.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Stephane.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
		}
		if (_Chloe.GetComponent<CharSim>().voteForPlayer == true)
		{
			numberVotes += 1;
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
		Debug.Log("PPL voted you : " + numberVotes);
		Debug.Log("PPL need : " + stepVotesForWin);
		return res;
	}
}
