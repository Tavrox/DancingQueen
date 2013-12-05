using UnityEngine;
using System.Collections;
using UnityEditor;

public class TestMode : MonoBehaviour {
	
	public int[] wantedTweaks;
	public bool activated;
//	private GUI debugBoris;
	
	private Alex _Alex;
	private Bastien _Bastien;
	private Bob _Bob;
	private Boris _Boris;
	private Charlie _Charlie;
	private Chloe _Chloe;
	private Christine _Christine;
	private Claire _Claire;
	private Didier _Didier;
	private Girls _Girls;
	private Manon _Manon;
	private Paul _Paul;
	private Raphael _Raphael;
	private Stephane _Stephane;
	private Thomas _Thomas;
	private Vanessa _Vanessa;
	private Yannick _Yannick;
	private PlayerSim _Player;
	private LevelManager _LM;
	private PlaylistController _PC;

	// Use this for initialization
	void Start () 
	{
		_Alex 		= GameObject.FindGameObjectWithTag("Alex").GetComponent<Alex>();
		_Bob 		= GameObject.FindGameObjectWithTag("Bob").GetComponent<Bob>();
		_Boris 		= GameObject.FindGameObjectWithTag("Boris").GetComponent<Boris>();
		_Charlie	= GameObject.FindGameObjectWithTag("Charlie").GetComponent<Charlie>();
		_Chloe 		= GameObject.FindGameObjectWithTag("Chloe").GetComponent<Chloe>();
		_Christine	= GameObject.FindGameObjectWithTag("Christine").GetComponent<Christine>();
		_Claire 	= GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
		_Didier 	= GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
		_Manon 		= GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
		_Paul 		= GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		_Stephane 	= GameObject.FindGameObjectWithTag("Stephane").GetComponent<Stephane>();
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
		_Vanessa 	= GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
		_Yannick 	= GameObject.FindGameObjectWithTag("Yannick").GetComponent<Yannick>();
		_Girls 		= GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();

		if (activated == true)
		{
			ResetGame();
			print ("reset");
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			Boys _boys = GameObject.FindGameObjectWithTag("Boys").GetComponent<Boys>();
			_boys.TriggerDialog();
		}
		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			_Girls.TriggerDialogChloe();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			_Girls.TriggerDialogVanessa();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
			dialEvent.GetComponent<DialogEvent>().setupEvent(_Girls,"1010");

//			DialogEvent dialEv = GameObject.FindGameObjectWithTag("Event").GetComponent<DialogEvent>();
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			DialogEvent dialEvent = GameObject.FindGameObjectWithTag("Event").GetComponent<DialogEvent>();
			dialEvent.setupEvent(_Girls,"1010");
			dialEvent.triggerDeath();
		}
	}

	[ContextMenu ("ResetGame")]
	private void ResetGame()
	{
		Debug.Log("Reset Games");

		_Alex 		= GameObject.FindGameObjectWithTag("Alex").GetComponent<Alex>();
		_Bob 		= GameObject.FindGameObjectWithTag("Bob").GetComponent<Bob>();
		_Boris 		= GameObject.FindGameObjectWithTag("Boris").GetComponent<Boris>();
		_Bastien 	= GameObject.FindGameObjectWithTag("Bastien").GetComponent<Bastien>();
		_Charlie	= GameObject.FindGameObjectWithTag("Charlie").GetComponent<Charlie>();
		_Chloe 		= GameObject.FindGameObjectWithTag("Chloe").GetComponent<Chloe>();
		_Christine	= GameObject.FindGameObjectWithTag("Christine").GetComponent<Christine>();
		_Claire 	= GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
		_Didier 	= GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
		_Manon 		= GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
		_Paul 		= GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		_Stephane 	= GameObject.FindGameObjectWithTag("Stephane").GetComponent<Stephane>();
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
		_Vanessa 	= GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
		_Yannick 	= GameObject.FindGameObjectWithTag("Yannick").GetComponent<Yannick>();
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_PC 		= GameObject.Find("PlaylistController").GetComponent<PlaylistController>();

		_LM.Hours = 20;
		_LM.Minutes = 0;
		_LM.gameEnded = false;
		_LM.updateTimerEvery = 1.8f;
		_LM.canGoToVIP = false;
		_LM.setMusic(LevelManager.MusicList.Groovy, LevelManager.levelList.Bar);
		MasterAudio.PlaylistMasterVolume = 0.4f;
		_Alex.voteForPlayer = false;
		_Alex.casseCouilleS1 = false;
		_Alex.casseCouilleS2 = false;
		_Alex.casseCouilleS3 = false;
		_Alex.gotPlayerInVIP = false;
		_Alex.sympathy_score = 0;
		_Bastien.refusedMission = false;
		_Bastien.hasSpokenOncePlayer = false;
		_Bastien.acceptedMission = false;
		_Bastien.succeedMission = false;
		_Bastien.sympathy_score = 0;
		_Bob.unlocked = false;
		_Bob.sympathy_score = 0;
		_Chloe.isInToilet = false;
		_Chloe.knowsHomo = false;
		_Chloe.sympathy_score = 0;
		_Claire.talkedAboutSlow = false;
		_Claire.talkedAboutKissing = false;
		_Claire.talkedAboutFlirting = false;
		_Claire.sympathy_score = 0;
		_Didier.canPutSlow = false;
		_Didier.canPutElectro = false;
		_Didier.canPutCountry = false;
		_Didier.hasPutCountry = false;
		_Didier.hasPutElectro = false;
		_Didier.hasPutSlow = false;
		_Didier.missionDidierDone = false;
		_Didier.missionDidierEncours = false;
		_Didier.sympathy_score = 0;
		_Manon.missionDone = false;
		_Manon.missionEncours = false;
		_Manon.sympathy_score = 0;
		_Paul.PlayerKnowsIsDealer = false;
		_Paul.sympathy_score = 0;
		_Raphael.coupleClaire = true;
		_Raphael.kissedPlayer = false;
		_Raphael.hasTalkedRaphael = false;
		_Raphael.sympathy_score = 0;
		_Thomas.knowThomasPreferences = false;
		_Thomas.hasTalkedThomas = false;
		_Thomas.isBattleDance = false;
		_Thomas.sympathy_score = 0;
		_Vanessa.isSad = true;
		_Vanessa.knowsDance = false;
		_Vanessa.sympathy_score = 0;
		_Yannick.hasSpokenOnceToPlayer = false;
		_Yannick.sympathy_score = 0;
		_Player.numberDrugs = 0;
	}

	[ContextMenu ("ThomasDialogAboutClaire")]
	private void ThomasDialogAboutClaire()
	{
		Debug.Log("ThomasDialogAboutClaire");

		_Paul 		= GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_PC 		= GameObject.Find("PlaylistController").GetComponent<PlaylistController>();

		_Paul.PlayerKnowsIsDealer = true;
		_Paul.missionDone = false;
		_Paul.sympathy_score = 100;
		_Raphael.coupleClaire = false;
		_Raphael.kissedPlayer = false;
		_Raphael.hasTalkedRaphael = false;
		_Raphael.sympathy_score = 100;
		_Thomas.knowThomasPreferences = false;
		_Thomas.hasTalkedThomas = false;
		_Thomas.isBattleDance = false;
		_Thomas.sympathy_score = 100;
		_Player.numberDrugs = 3;
	}

	[ContextMenu ("MissionPaulOver")]
	private void MissionPaulOver()
	{
		Debug.Log("MissionPullOver");
		
		_Paul 		= GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_PC 		= GameObject.Find("PlaylistController").GetComponent<PlaylistController>();
		
		_Paul.PlayerKnowsIsDealer = true;
		_Paul.missionEncours = false;
		_Paul.missionDone = true;
		_Paul.sympathy_score = 100;
	}
	
	void OnGUI()
	{
		if (GUI.changed)
			EditorUtility.SetDirty(_Thomas);
//		debugBoris = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), moveVel, 0f, 10f);
		
	}
}
