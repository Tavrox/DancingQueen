using UnityEngine;
using System.Collections;
using UnityEditor;

public class TestMode : MonoBehaviour {
	
	public int[] wantedTweaks;
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
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();
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
			Girls _girls = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			_girls.TriggerDialogChloe();
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			Girls _girls = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			_girls.TriggerDialogVanessa();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
//			eve.triggerEvent();
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
//			eve.triggerDeath();
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

		_LM.Hours = 0;
		_LM.Minutes = 0;
		_LM.gameWon = false;
		_LM.updateTimerEvery = 1.8f;
		_LM.canGoToVIP = false;
		_LM.setMusic(LevelManager.MusicList.Groovy, LevelManager.levelList.Bar);
		_PC.masterPlaylistVolume = 1f;
		_Alex.voteForPlayer = false;
		_Alex.casseCouilleS1 = false;
		_Alex.casseCouilleS2 = false;
		_Alex.casseCouilleS3 = false;
		_Alex.gotPlayerInVIP = false;
		_Bastien.refusedMission = false;
		_Bastien.hasSpokenOncePlayer = false;
		_Bastien.acceptedMission = false;
		_Bastien.succeedMission = false;
		_Bob.unlocked = false;
		_Chloe.isInToilet = false;
		_Chloe.knowsHomo = false;
		_Claire.talkedAboutSlow = false;
		_Claire.talkedAboutKissing = false;
		_Claire.talkedAboutFlirting = false;
		_Didier.canPutSlow = false;
		_Didier.canPutElectro = false;
		_Didier.canPutCountry = false;
		_Didier.hasPutCountry = false;
		_Didier.hasPutElectro = false;
		_Didier.hasPutSlow = false;
		_Didier.missionDidierDone = false;
		_Didier.missionDidierEncours = false;
		_Manon.missionDone = false;
		_Manon.missionEncours = false;
		_Paul.PlayerKnowsIsDealer = false;
		_Raphael.coupleClaire = true;
		_Raphael.kissedPlayer = false;
		_Raphael.hasTalkedRaphael = false;
		_Thomas.knowThomasPreferences = false;
		_Thomas.hasTalkedThomas = false;
		_Thomas.isBattleDance = false;
		_Vanessa.isSad = true;
		_Vanessa.knowsDance = false;
		_Yannick.hasSpokenOnceToPlayer = false;


	}
	
	void OnGUI()
	{

//		debugBoris = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), moveVel, 0f, 10f);
		
	}
}
