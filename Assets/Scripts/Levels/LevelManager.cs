using UnityEngine;
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

	private GameObject[] wpChars;
	private GameObject[] wpDoors;

	public string[] musicNames;
	private string musicToPlay;
	private GUIText Timer;
	private int Hours = 20;
	private int Minutes = 0;

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
		InvokeRepeating("updateTimer",0, 1f);
		setBackgrounds();
		instantiateDoors();
		setCharacGO();
		setCharPos();
		setWaypoints();
		setMusic();
		MasterAudio.TriggerPlaylistClip(musicNames[0]);
	}

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this);
		Timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<GUIText>();
//		print (MasterAudio.TriggerPlaylistClip(musicName));
//		PlaylistController.PlayNextSong();
		//		PlaylistController.PlayNextSong();

	}
	
	// Update is called once per frame
	void Update () 
	{
		print (wpDoors[0]);
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

	}

	public void changeRoom ()
	{
		print ("ChangeRoom");
		cleanDoors();
		instantiateDoors();
	}

	private void setCharacGO()
	{
		_Alex 		= GameObject.Find("Alex");
		_Anais 		= GameObject.Find("Anais");
		_Bastien 	= GameObject.Find("Bastien");
		_Bob 		= GameObject.Find("Bob");
		_Boris 		= GameObject.Find("Boris");
		_Charlie	= GameObject.Find("Charlie");
		_Chloe 		= GameObject.Find("Chloe");
		_Christine	= GameObject.Find("Christine");
		_Claire 	= GameObject.Find("Claire");
		_Manon 		= GameObject.Find("Manon");
		_Paul 		= GameObject.Find("Paul");
		_Raphael 	= GameObject.Find("Raphael");
		_Stephane 	= GameObject.Find("Stephane");
		_Thomas 	= GameObject.Find("Thomas");
		_Vanessa 	= GameObject.Find("Vanessa");
		_Yannick 	= GameObject.Find("Yannick");
	}

	private bool setBackgrounds()
	{
		backBarFrame 	= "back_bar";
		backDancefloorFrame = "back_dancefloor";
		backToiletsFrame = "back_toilets";
		backVIPFrame = "back_vip";
		return true;
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
			// Instantiate Each doors
			GameObject doorPrefab = Resources.Load("06Levels/Level_Next") as GameObject;
			print (wpDoors[0]);
			doorPrefab.transform.position = wpDoors[0].transform.position;
			doorPrefab.GetComponentInChildren<OTSprite>().frameName = "door_bar";
			Instantiate(doorPrefab);
		}
		if (currentLvl == LevelManager.levelList.Dancefloor)
		{
			// Instantiate Each doors

		}
		if (currentLvl == LevelManager.levelList.Toilets)
		{
			// Instantiate Each doors
		}
		if (currentLvl == LevelManager.levelList.VIP)
		{
			// Instantiate Each doors
		}
	}

	private void setWaypoints()
	{
		// CHARS
		wpChars[1] = GameObject.Find("WP_Char_Bastien");
		wpChars[2] = GameObject.Find("WP_Char_Bob");
		wpChars[3] = GameObject.Find("WP_Char_Boris");
		wpChars[4] = GameObject.Find("WP_Char_Charlie");
		wpChars[5] = GameObject.Find("WP_Char_Didier");
		wpChars[6] = GameObject.Find("WP_Char_Manon");
		wpChars[7] = GameObject.Find("WP_Char_Paul");
		wpChars[8] = GameObject.Find("WP_Char_Vanessa");
		wpChars[9] = GameObject.Find("WP_Char_Yannick");

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
				// FIXED CHARACTER
				// Bastien
				// Manon
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
				if (_Vanessa.GetComponent<Vanessa>().triggeredUltimate == true)
				{

				}
				if (_Thomas.GetComponent<Thomas>().isBattleDance == true)
				{

				}
				if (_Chloe.GetComponent<Chloe>().isInToilet == false)
				{

				}
				break;
			}
			case (levelList.Toilets) :
			{
				if (_Chloe.GetComponent<Chloe>().isInToilet == true)
				{

				}
				if (_Raphael.GetComponent<Raphael>().coupleClaire == true)
				{

				}
				break;
			}
			case (levelList.VIP) :
			{
				break;
			}
		}
	}

	private void setMusic()
	{
		switch (currentLvl)
		{
			case (levelList.Dancefloor) :
			{
				musicToPlay = musicNames[0];
				break;
			}
			case (levelList.Bar) :
			{
				musicToPlay = musicNames[1];
				break;
			}
			case (levelList.Toilets) :
			{
				musicToPlay = musicNames[2];
				break;
			}
			case (levelList.VIP) :
			{
				musicToPlay = musicNames[3];
				break;
			}
		}
	}

	IEnumerator Wait(float WaitTime)
	{
		yield return new WaitForSeconds(WaitTime);
	}
}
