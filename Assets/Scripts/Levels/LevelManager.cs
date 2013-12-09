using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public OTSprite background;
	[SerializeField] private Player player;
	public static CharSim currentCharacterSpeaking;
	public bool gameEnded = false;
	public bool canGoToVIP = false;
	public bool triggerDialogVanessaGroup;
	public bool triggerDialogGuys;
	public bool triggerDialogVanessaMusic;
	public bool triggerDialogClaireSlow;
	public bool triggerDialogChloe;
	
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
	public bool stopTimer = true;
	private bool tutoActivated = false;

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
	public bool eventHappening = false;
	public int numberVotes = 0;
	private string musicStyle;

	private string backBarFrame;
	private string backDancefloorFrame;
	private string backToiletsFrame;
	private string backVIPFrame;


	void Awake()
	{
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
		checkDialogs();
		if (eventHappening != true)
		{
			checkEvents();
		}
		checkTuto();
	}

	void updateTimer()
	{
		if (stopTimer != true)
		{
			Minutes +=1;
			if (Minutes == 60)
			{
				Hours+=1;
				Minutes = 0;
			}
			if (Hours == 24)
			{
				Timer.text = "";
			}
		}
	}

	private void checkTuto()
	{
		if (currentLvl == levelList.Bar)
		{
			if (_Yannick.GetComponent<Yannick>().hasSpokenOnceToPlayer == false)
			{
				// Door bar Locked
				if (GameObject.Find("Introduction") == null && tutoActivated == false)
				{
					GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
					dialEvent.GetComponent<DialogEvent>().setupEvent(_Yannick.GetComponent<Yannick>(),"6001");
					tutoActivated = true;
				}

			}
			else
			{ 
				if (GameObject.Find("DoorBarToDance(Clone)") != null)
				{
					GameObject.Find("DoorBarToDance(Clone)").GetComponent<LevelDoor>().locked = false;
				}
				_Chloe.GetComponent<Chloe>().dialDisabled = false;
				_Vanessa.GetComponent<Vanessa>().dialDisabled = false;
			}
		}
	}

	public int getVotes()
	{
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
		return numberVotes;
	}
	private void checkEvents()
	{
		Alex compAlex 			= _Alex.GetComponent<Alex>();
		Bastien compBastien 	= _Bastien.GetComponent<Bastien>();
		Bob compBob 			= _Bob.GetComponent<Bob>();
		Boris compBoris 		= _Boris.GetComponent<Boris>();
		Charlie compCharlie		= _Charlie.GetComponent<Charlie>();
		Chloe compChloe 		= _Chloe.GetComponent<Chloe>();
		Christine compChristine	= _Christine.GetComponent<Christine>();
		Claire compClaire 		= _Claire.GetComponent<Claire>();
		Manon compManon 		= _Manon.GetComponent<Manon>();
		Paul compPaul 			= _Paul.GetComponent<Paul>();
		Raphael compRaphael 	= _Raphael.GetComponent<Raphael>();
		Stephane compStephane 	= _Stephane.GetComponent<Stephane>();
		Thomas compThomas 		= _Thomas.GetComponent<Thomas>();
		Vanessa compVanessa 	= _Vanessa.GetComponent<Vanessa>();
		Yannick compYannick 	= _Yannick.GetComponent<Yannick>();
		PlayerSim compPlayer 	= _Player.GetComponent<PlayerSim>();

		// PAUL EVENTS
		// TRIGGER MISSION 
		if (compPaul.sympathy_score >= compPaul.amountMissionRaph && compPaul.missionEncours != true)
		{
			compPaul.dialToTrigger = "9021";
		}
		// TRIGGER MISSION 
		if (compPaul.sympathy_score >= compPaul.amountMissionRaph && compPaul.missionEncours == true)
		{
			compPaul.dialToTrigger = "9025";
		}
		if (compRaphael.coupleClaire == false )
		{
			compPaul.dialToTrigger = "9024";
		}
		// RESET AFTER MISSION
		if (compPaul.resetAfterMission == true)
		{
			compPaul.dialToTrigger = "9001";
		}

		// CHLOE
		if (compChloe.knowsHomo == true && compChloe.sympathy_score > compChloe.neededToKiss)
		{
			compChloe.dialToTrigger = "12016";
		}

		//THOMAS
		if (compThomas.isBattleDance == true && compThomas.hasHeardAboutClaire == true)
		{
			compThomas.dialToTrigger = "8010";
		}
		if (compThomas.isBattleDance == true && compThomas.hasHeardAboutClaire == false 
		    && compRaphael.coupleClaire == false && compPlayer.numberDrugs > 0)
		{
			compThomas.dialToTrigger = "8029";
		}

		Claire Claire = GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
		Raphael Raph = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		Didier Didi = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();

		if (Raph.hasTalkedRaphael == true && Claire.talkedAboutFlirting == false)
		{
			if (DialogUI.exists != true)
			{
				GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
				dialEvent.GetComponent<DialogEvent>().setupEvent(Claire,"14010");
				Claire.talkedAboutFlirting = true;
			}
		}
		if (Raph.kissedPlayer == true && Claire.talkedAboutKissing == false)
		{
			if (DialogUI.exists != true)
			{
//				IngameUI.destroyIngameUI();
//				DialogUI.createDialog(Claire, "14005");
				
				GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
				dialEvent.GetComponent<DialogEvent>().setupEvent(Claire,"14005");
				Claire.talkedAboutKissing = true;
			}
		}
		if (Didi.hasPutSlow == true && Claire.talkedAboutSlow == false)
		{
			if (DialogUI.exists != true)
			{
//				IngameUI.destroyIngameUI();
//				DialogUI.createDialog(Claire, "14007");
				
				GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
				dialEvent.GetComponent<DialogEvent>().setupEvent(Claire,"14007");
				Claire.talkedAboutSlow = true;
			}
		}

		Alex GO = GameObject.FindGameObjectWithTag("Alex").GetComponent<Alex>();
		if (Hours == 21)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS1 != true)
				{
					//					IngameUI.destroyIngameUI();
					//					DialogUI.createDialog(GO, "11001" );
					GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
					dialEvent.GetComponent<DialogEvent>().setupEvent(GO,"11001");
					GO.casseCouilleS1 = true;
				}
				
			}
		}
		
		if (Hours == 22)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS2 == false && GO.gotPlayerInVIP != true && GO.casseCouilleS1 == true)
				{
					//					IngameUI.destroyIngameUI();
					//					DialogUI.createDialog(GO, "11006");
					GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
					dialEvent.GetComponent<DialogEvent>().setupEvent(GO,"11006");
					GO.casseCouilleS2 = true;
				}
				
			}
		}
		
		if (Hours == 23)
		{
			if (DialogUI.exists != true)
			{
				if (GO.casseCouilleS3 == false && GO.gotPlayerInVIP == false  && GO.casseCouilleS2 == true)
				{
					//					IngameUI.destroyIngameUI();
					//					DialogUI.createDialog(GO, "11011");
					GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
					dialEvent.GetComponent<DialogEvent>().setupEvent(GO,"11011");
					GO.casseCouilleS3 = true;
				}
				
			}
		}
		
		if (Hours == 24 && gameEnded != true)
		{
			gameEnded = true;
			GameObject EndingEvent = Instantiate(Resources.Load("04Misc/Ending")) as GameObject;
			Ending _end = EndingEvent.GetComponent<Ending>();
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

	private void createEventTransition(CharSim charToTrigg, string dialToTrigg)
	{
		if (eventHappening == true)
		{
//			eventHappening = false;
//			DialogEvent dialEve = new DialogEvent();
//			dialEve.triggerEvent();
//			StartCoroutine(WaitEvent(3f, dialEve));
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
	private void checkDialogs()
	{
		if (triggerDialogClaireSlow == true)
		{
			Claire _claire = GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
			_claire.TriggerDialogClaireMusic();
			triggerDialogClaireSlow = false;
			Debug.Log("triggerDialogClaireSlow");
		}
		if (triggerDialogChloe == true)
		{
			Girls _girls = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			_girls.TriggerDialogChloe();
			triggerDialogChloe = false;
			Debug.Log("triggerDialogChloe");
		}
		if (triggerDialogGuys == true)
		{
			Boys _boys = GameObject.FindGameObjectWithTag("Boys").GetComponent<Boys>();
			_boys.TriggerDialog();
			triggerDialogGuys = false;
			Debug.Log("triggerDialogGuys");
		}
		if (triggerDialogVanessaMusic == true)
		{
			Vanessa _vanessa = GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
			_vanessa.TriggerDialogVanessa();
			triggerDialogVanessaMusic = false;
			Debug.Log("triggerDialogVanessaMusic");
		}
		if (triggerDialogVanessaGroup == true)
		{
			Girls _girls = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			_girls.TriggerDialogVanessa();
			triggerDialogVanessaGroup = false;
			Debug.Log("triggerDialogVanessaGroup");
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
				hideChar("Bob");
			
				unhideChar("Raphael");
				unhideChar("Bastien");
				unhideChar("Thomas");
				unhideChar("Didier");
				unhideChar("Manon");

				if (_Vanessa.GetComponent<Vanessa>().isSad == false)
				{
					unhideChar("Vanessa");
				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{
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
					hideChar("Vanessa");
				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{
					_Thomas.GetComponentInChildren<OTSprite>().frameName = "thomas_fullbar";
					_Thomas.transform.position = new Vector3(-2.060373f, 0.8314278f, 3f);
					_Thomas.transform.localScale = new Vector3(0.4477435f, 0.4477435f, 0.4477435f);
					_Thomas.GetComponent<BoxCollider>().center = new Vector3 (0f, 1.31f, 0f);
					_Thomas.GetComponent<BoxCollider>().size = new Vector3(6.2f, 13.82f, 20f);
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
				if (_Vanessa.GetComponent<Vanessa>().isSad == false)
				{
					hideChar("Vanessa");
				}
			hideChar("Vanessa");

				unhideChar("Boris");
				unhideChar("Paul");
				unhideChar("Charlie");
				break;
			}
			case (levelList.VIP) :
			{
				setCharacGO("");
			
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
				musicLvl = MusicList.Country;
				print ("Music set : "+music);
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
				musicToPlay = "CountryVIP";
				
			}
			if (music ==  MusicList.Slow)
			{
				musicToPlay = "SlowVIP";
			}
		}
		musicLvl = music;
		MasterAudio.ChangePlaylistByName(music.ToString());
		MasterAudio.TriggerPlaylistClip(musicToPlay);
		Debug.Log("Playlist settled " + music.ToString());
		Debug.Log("Clip played " + musicToPlay);
		
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
	IEnumerator WaitEvent(float WaitTime, DialogEvent eve)
	{
		yield return new WaitForSeconds(WaitTime);
		Destroy(GameObject.Find("Event(Clone)"));
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
		return res;
	}
}
