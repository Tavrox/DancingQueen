using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class DialogDisplayer : MonoBehaviour {
	
	public GUIText talkTextGUI, answer1TextGUI, answer2TextGUI, answer3TextGUI,answer4TextGUI,answer5TextGUI,answer6TextGUI, speaker;	//Text object
	[HideInInspector] public string[] talkLines;	//Array containing all the sentences of the dialog
	public int textScrollSpeed = 60;
	
	public Dialog firstDialog;
	public Answer answer1, answer2, answer3, answer4, answer5, answer6;
	public int delayStart = 3;

	private CharSim currentChar;
	private bool isPlayerSpeaking;
	private bool talking;	//The dialog is displayed
	private bool finishedTalking;
	private bool textIsScrolling;	//The text is currently scrolling
	private int currentLine;	//Current line read
	private int startLine, i;
	private string displayText; //The text displayed
	private Dialog dialToDisplay;

	/**ADD**/
//	private XmlDocument xmlDoc;
	private XmlNodeList dialogNodes;
	private string CharDialID;
	private string[] fullDialog = new string[8];
	private string[,] arrayAnswers = new string[8,10];
	private bool answersInitiated;
	private GameObject instance1,instance2,instance3,instance4,instance5,instance6, prefabSprite; 
	private bool playerSpoken,playerSpeaking, killAtferDisplay, textInserted, coroutineRunning;
	private LevelManager  lvManager;
	private GameObject _Player, _BgDialogPlayer, _BgDialogPNJ;
	private GUIText _NextDialog;

	//[HideInInspector] public Player player; //The player to lock
	
	void Start() {
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;

		
		setCharacter(LevelManager.currentCharacterSpeaking);
		CharDialID = currentChar.dialToTrigger;
		lvManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_Player = GameObject.Find("PlayerData");
		_BgDialogPNJ = GameObject.Find("overlaySprite-2");
		_BgDialogPlayer = GameObject.Find("overlaySprite");
		_BgDialogPNJ.renderer.enabled = false;
		_BgDialogPlayer.renderer.enabled = false;
		_NextDialog = GameObject.Find("DialogNext>").GetComponent<GUIText>();

		/**ADD**/
		// string url = "http://paultondeur.com/files/2010/UnityExternalJSONXML/books.xml";
		// string url = "file:///"+Application.dataPath+"/"+"Dialogs/dialogs.xml";
		//string url = getCharacXML(currentChar.charac);
		//WWW www = new WWW(url);
		TextAsset textAsset = getCharacXML(currentChar.charac);
		//Resources.Load("dialogs_Yannick") as TextAsset;
		//print (textAsset);
		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		//yield return www;
		
		//if (www.error == null)
		//{
			//Sucessfully loaded the XML
			
		//Create a new XML document out of the loaded data
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset.text);
			
			//Point to the book nodes and process them
			dialogNodes = xmlDoc.SelectNodes("dialogs/dialog");
		//}
		getDialogContent();

		InvokeRepeating("playWhispers", UnityEngine.Random.Range(currentChar.randomDelayMin,currentChar.randomDelayMax) , currentChar.frequencyWhispers);
		startDialog("");//StartCoroutine( Wait(delayStart) );

		
		OTSprite otherFace = GameObject.Find("otherFaceSpriteDialog").GetComponent<OTSprite>();
		otherFace.frameName = currentChar.getCharFrame(currentChar.charac);
	}

	
	// Update is called once per frame
	void Update ()
	{

		if(talking == true)
		{
			if(playerSpeaking == true) {
				
				_Player.GetComponent<PlayerSim>().playWhispers();
				_BgDialogPNJ.renderer.enabled = false;
				_BgDialogPlayer.renderer.enabled = true;
			}
			else 
			{
				_BgDialogPNJ.renderer.enabled = true;
				_BgDialogPlayer.renderer.enabled = false;
			}

			/*
			if(Input.GetMouseButtonDown(0)) 
			{ 
				//Dialog interaction button detection
				if(textIsScrolling)
				{
	            }
				else {
					if(currentLine < talkLines.Length - 1)
					{ print("CLICLIC");
						//If the text is not scrolling and still lines to read
						currentLine++;	//Go to next line
						StartCoroutine("StartScrolling");	//Start scroll effect
					}
					else
					{	

					}
	          	}
			}
			*/
		}
		if (finishedTalking == true)
		{
			if(fullDialog[4]!="closeDialog" || arrayAnswers[0,8] == "" || arrayAnswers[0,8] == "0" ) 
			{

				if(!killAtferDisplay) 
				{

					if(!playerSpeaking) {
						if(!talking) {
							InstantiateAnswers();
							finishedTalking = false;
							answersInitiated = true;
						}
					}
					else {
						//_NextDialog.enabled = true;
						if(coroutineRunning == false) StartCoroutine("twinkleText");
						if(Input.GetMouseButtonDown(0)) {
							playerSpeaking = false;
							_NextDialog.enabled = false;
							launchNextDialog();
						}
					}
				}
				else {
					if(coroutineRunning == false) StartCoroutine("twinkleText");
					if(Input.GetMouseButtonDown(0)) {
						playerSpeaking = false;
						finishedTalking =false;
						killAtferDisplay =false;
						_NextDialog.enabled = false;
						DialogUI.destroyDialog();
					}
				}
			}
			else {
				if(coroutineRunning == false) StartCoroutine("twinkleText");
				if(Input.GetMouseButtonDown(0)) {
					finishedTalking =false;
					_NextDialog.enabled = false;
					DialogUI.destroyDialog();
				}
			}
		}
		if(answersInitiated) 
		{
			if(!textInserted) 
			{
				if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.en)
				{
					answer1TextGUI.text = answer1.choice_en;
					answer2TextGUI.text = answer2.choice_en;
					answer3TextGUI.text = answer3.choice_en;
					answer4TextGUI.text = answer4.choice_en;
					answer5TextGUI.text = answer5.choice_en;
					answer6TextGUI.text = answer6.choice_en;
				}
				else
				{
					answer1TextGUI.text = answer1.choice_fr;
					answer2TextGUI.text = answer2.choice_fr;
					answer3TextGUI.text = answer3.choice_fr;
					answer4TextGUI.text = answer4.choice_fr;
					answer5TextGUI.text = answer5.choice_fr;
					answer6TextGUI.text = answer6.choice_fr;
				}
				textInserted = true;
			}
			if (answer1 != null && answer1.triggered == true) 
			{
				displayPlayerAnswer(answer1,0);  
				checkAction(answer1.action);
				DestroyAnswers();
			}
			if (answer2 != null && answer2.triggered == true) 
			{
				displayPlayerAnswer(answer2,1);
				checkAction(answer2.action);
				DestroyAnswers();
			}
			if (answer3 != null && answer3.triggered == true)
			{
				displayPlayerAnswer(answer3,2);  
				checkAction(answer3.action);
				DestroyAnswers();
			}
			if (answer4 != null && answer4.triggered == true)
			{
				displayPlayerAnswer(answer4,3);  
				checkAction(answer4.action);
				DestroyAnswers();
			}
			if (answer5 != null && answer5.triggered == true)
			{
				displayPlayerAnswer(answer5,4);  
				checkAction(answer5.action); 
				DestroyAnswers();
			}
			if (answer6 != null && answer6.triggered == true)
			{
				displayPlayerAnswer(answer6,5);  
				checkAction(answer6.action);
				DestroyAnswers();
			}
		}
	}

	
	public void getDialogContent() {
		foreach (XmlNode node in dialogNodes)
		{
			if(node.Attributes.GetNamedItem("fullID").Value == CharDialID)
			{
				fullDialog = new string[8];
				arrayAnswers = new string[8,11];
				
				fullDialog[0] = node.Attributes.GetNamedItem("fullID").Value;
				fullDialog[1] = node.Attributes.GetNamedItem("ID_Character").Value;
				fullDialog[2] = node.Attributes.GetNamedItem("Sympathy_value").Value;
				fullDialog[3] = node.Attributes.GetNamedItem("Condition").Value;
				fullDialog[4] = node.Attributes.GetNamedItem("Action").Value;
				fullDialog[5] = node.SelectSingleNode("fr").InnerText;
				fullDialog[6] = node.SelectSingleNode("en").InnerText;
				int i=0;
				foreach (XmlNode answer in node.SelectNodes("answer"))
				{
					if(answer.Attributes.GetNamedItem("fullID").Value == null) break;
					arrayAnswers[i,0] = answer.Attributes.GetNamedItem("fullID").Value;
					arrayAnswers[i,1] = answer.Attributes.GetNamedItem("ID_Dialog").Value;
					arrayAnswers[i,2] = answer.Attributes.GetNamedItem("ID_NextDialog").Value;
					arrayAnswers[i,3] = (answer.Attributes.GetNamedItem("Sympathy_value").Value == "")? "0" : answer.Attributes.GetNamedItem("Sympathy_value").Value;
					arrayAnswers[i,4] = answer.Attributes.GetNamedItem("Condition").Value;
					arrayAnswers[i,5] = answer.Attributes.GetNamedItem("Action").Value;
					arrayAnswers[i,6] = answer.SelectSingleNode("fr").Attributes.GetNamedItem("Choice_fr").Value;
					arrayAnswers[i,7] = answer.SelectSingleNode("en").Attributes.GetNamedItem("Choice_en").Value;
					arrayAnswers[i,8] = answer.SelectSingleNode("fr").InnerText;
					arrayAnswers[i,9] = answer.SelectSingleNode("en").InnerText;
					arrayAnswers[i,10] = answer.Attributes.GetNamedItem("BanAnswer").Value;
					i++;
				}
				dialToDisplay = firstDialog;
				talkLines = dialToDisplay.dialLines;
				checkAction(fullDialog[4]);

				GUIText speaking = GameObject.Find("00_OtherSpeaker").GetComponent<GUIText>();
				string pickCharacter = returnCharacName(fullDialog[1]).ToString();
				speaking.text = pickCharacter;
				speaking.color = getCharColor(returnCharacName(fullDialog[1]).ToString());

				if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.fr)
				{
					talkLines[0]=fullDialog[5];
				}
				else
				{
					talkLines[0]=fullDialog[6];
				}
				break;
			}
			else
			{
//				Debug.Log("No dialog has been found");
			}
		}
	}
	
	private void displayPlayerAnswer(Answer displayedAnswer, int indexAnswer) 
	{
		if(!playerSpeaking) 
		{
			answer1.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer1.collider.enabled = false;answer1.enabled = false;
			answer2.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer2.collider.enabled = false;answer2.enabled = false;
			answer3.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer3.collider.enabled = false;answer3.enabled = false;
			answer4.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer4.collider.enabled = false;answer4.enabled = false;
			answer5.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer5.collider.enabled = false;answer5.enabled = false;
			answer6.GetComponentInChildren<OTSprite>().renderer.enabled = false;answer6.collider.enabled = false;answer6.enabled = false;
			answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = answer4TextGUI.enabled = answer5TextGUI.enabled = answer6TextGUI.enabled = false;

			if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.fr)
			{
				talkLines[0]= arrayAnswers[indexAnswer,8];
			}
			else
			{
				talkLines[0]= arrayAnswers[indexAnswer,9];
			}
			CharDialID = displayedAnswer.ID_nextDialog;
			modifySympathy(fullDialog[1], displayedAnswer.sympathy_value);
			banDialogID(arrayAnswers[indexAnswer,10]);
			finishedTalking = false;
			startDialog(fullDialog[4]);
			playerSpeaking = true;
		}
	}

	//Call to the dialog from another class
	public void startDialog(string action) 
	{
		checkAction(action);
		talking = true;	//Activtate talking state
		currentLine = 0;
		StartCoroutine("StartScrolling");	//Start displaying text
	}
	
	//Activate the dialog when the player is in the collision box
	void OnTriggerEnter(Collider col) 
	{
		
	}
	void OnMouseOver () 
	{
		
		
	}
	private void InstantiateAnswers()
	{
		if (answer1 == null) setAnswer(ref answer1, "Answer1",ref instance1,ref answer1TextGUI,0);
		if (answer2 == null) setAnswer(ref answer2, "Answer2",ref instance2,ref answer2TextGUI,1);
		if (answer3 == null) setAnswer(ref answer3, "Answer3",ref instance3,ref answer3TextGUI,2);
		if (answer4 == null) setAnswer(ref answer4, "Answer4",ref instance4,ref answer4TextGUI,3);
		if (answer5 == null) setAnswer(ref answer5, "Answer5",ref instance5,ref answer5TextGUI,4);
		if (answer6 == null) setAnswer(ref answer6, "Answer6",ref instance6,ref answer6TextGUI,5);
		textInserted = false;
	}
	
	private void setAnswer(ref Answer answerToSet, string path,ref GameObject instance,ref GUIText answerTextGUI, int indexAnswer) {
		prefabSprite = Resources.Load("03UI/"+path) as GameObject;
		instance = Instantiate(prefabSprite) as GameObject;
		answerToSet = instance.GetComponent<Answer>();
		answerTextGUI = instance.GetComponent<Answer>().GetComponentInChildren<GUIText>();
		answerToSet.setNextDialog(arrayAnswers[indexAnswer,2]);
		answerToSet.setSympathyValue(Convert.ToInt32(arrayAnswers[indexAnswer,3]));
		answerToSet.setAction(arrayAnswers[indexAnswer,5]);
		answerToSet.setCondition(arrayAnswers[indexAnswer,4]);

		
		if ( checkCondition(answerToSet.condition) != true && answerToSet.condition != "basic" )
		{
			answerToSet.GetComponentInChildren<OTSprite>().renderer.enabled = false;
			answerToSet.collider.enabled = false;
			answerToSet.enabled = false;
			answerTextGUI.enabled = false;
		}

		// SET CHOICES ACCORDING TO LANG
		if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.fr)
		{
			if (_Player.GetComponent<PlayerSim>().findBanAnswers((arrayAnswers[indexAnswer,0])) == false)
			{
				answerToSet.setChoiceFR(arrayAnswers[indexAnswer,6]);
				answerToSet.setAnswerLineFR(arrayAnswers[indexAnswer,8]);
			}
		} 
		// DEBUG
		else if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.en)
		{
			if (_Player.GetComponent<PlayerSim>().findBanAnswers((arrayAnswers[indexAnswer,0])) == false)
			{
				answerToSet.setChoiceEN(arrayAnswers[indexAnswer,7]);
				answerToSet.setAnswerLineEN(arrayAnswers[indexAnswer,9]);
			}
		}

		else 
		{
			Debug.Log("CANT FIND ANSWER MASSIVE ERROR");
		}

		// UNINSTANSTIATE ANSWERS IF NOT SET
		if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.fr)
		{
			if (answerToSet.choice_fr == "" || answerToSet.choice_fr == null || _Player.GetComponent<PlayerSim>().findBanAnswers((arrayAnswers[indexAnswer,0])) == true    )
			{
				answerToSet.GetComponentInChildren<OTSprite>().renderer.enabled = false;
				answerToSet.collider.enabled = false;
				answerToSet.enabled = false;
			}
		}
		if (_Player.GetComponent<PlayerSim>().langChosen == PlayerSim.langList.en  )
		{
			if (answerToSet.choice_en == "" || answerToSet.choice_en == null || _Player.GetComponent<PlayerSim>().findBanAnswers((arrayAnswers[indexAnswer,0])) == true  )
			{
				answerToSet.GetComponentInChildren<OTSprite>().renderer.enabled = false;
				answerToSet.collider.enabled = false;
				answerToSet.enabled = false;
			}
		} 

	}

	private void DestroyAnswers()
	{
		Destroy(instance1);
		Destroy(instance2);
		Destroy(instance3);
		Destroy(instance4);
		Destroy(instance5);
		Destroy(instance6);
		Destroy(answer1);
		Destroy(answer2);
		Destroy(answer3);
		Destroy(answer4);
		Destroy(answer5);
		Destroy(answer6);
		answer1 = null;
		answer2 = null;
		answer3 = null;
		answer4 = null;
		answer5 = null;
		answer6 = null;
	}
	
	public void launchNextDialog()
	{
		getDialogContent();
		answersInitiated = false;
		talking = true;
		currentLine = 0;
		StartCoroutine("StartScrolling");
	}
	
	private Dialog findNextDialog()
	{
		Dialog dial = new Dialog();
		dial.fetchLines();
		return dial;
		
	}

	private bool checkCondition(string Condition)
	{
		GameObject go = GameObject.FindGameObjectWithTag("Bastien");
		bool res = false;
		switch (Condition)
		{
			case ("Sympathy_value_Bastien25") :
			{
				Bastien charac = GameObject.FindGameObjectWithTag("Bastien").GetComponent<Bastien>();
				if (charac.sympathy_score > 25)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("MissionDidierFinie") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.missionDidierDone == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("missionDidierEncours") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.missionDidierEncours == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("knowsThomasPreferences") :
			{
				Thomas charac = GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
				if (charac.knowThomasPreferences == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("DidierSympathy25") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.sympathy_score > 25)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("ManonSympathy50") :
			{
				Manon charac = GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
				if (charac.sympathy_score > 50)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("ManonSympathy100") :
			{
				Manon charac = GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
				if (charac.sympathy_score > 100)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("Sympathy_value_Chloe50") :
			{
				Chloe charac = GameObject.FindGameObjectWithTag("Chloe").GetComponent<Chloe>();
				if (charac.sympathy_score > 50)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("isBattleDance") :
			{
				Thomas charac = GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
				if (charac.isBattleDance == false)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("claireMusic") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.canPutSlow == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("vanessaMusic") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.canPutCountry == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("bastienMusic") :
			{
				Didier charac = GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
				if (charac.canPutElectro == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("RaphaelSympathy50") :
			{
				Raphael charac = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac.sympathy_score > 50)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("ThomasSympathy50") :
			{
				Thomas charac = GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
				if (charac.sympathy_score > 50)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("PaulSympathy50") :
			{
				Paul charac = GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
				if (charac.sympathy_score > 50)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("BorisSympathy75") :
			{
				Boris charac = GameObject.FindGameObjectWithTag("Boris").GetComponent<Boris>();
				PlayerSim player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
				if (charac.sympathy_score > 75 && player.numberDrugs > 0 )
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("PlayerKnowsPaulDealer") :
			{
				Paul charac = GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
				if (charac.PlayerKnowsIsDealer == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("NoPlayerDoesntKnowsIsDealer") :
			{
				Paul charac = GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
				if (charac.PlayerKnowsIsDealer == false)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("hasTalkedThomas") :
			{
				Thomas charac = GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
				if (charac.hasTalkedThomas == false)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("hasTalkedRaphael") :
			{
				Raphael charac = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac.hasTalkedRaphael == false)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("HasDrug") :
			{
				PlayerSim charac = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
				if (charac.numberDrugs > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("MissionManonEnCours") :
			{
				Manon charac = GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
				if (charac.missionEncours == true && charac.missionDone == false)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			case ("RaphaelClaireCouplesympathy50") :
			{
				Paul charac = GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
				Raphael charac2 = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac.sympathy_score > 50 && charac2.coupleClaire == true)
				{
					return true;
				}
				else
				{
					return false;
				}

				break;
			}
			case ("NoRaphaelClairecoupleSympathy75") :
			{
				Paul charac = GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
				Raphael charac2 = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac.sympathy_score > 75 && charac2.coupleClaire == false)
				{
					return true;
				}
				else
				{
					return false;
				}

				break;
			}
			case ("kissRaphael") :
			{
				Raphael charac2 = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac2.kissedPlayer == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				
				break;
			}
			case ("MissionBastienDone") :
			{
				Raphael charac2 = GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
				if (charac2.kissedPlayer == true)
				{
					return true;
				}
				else
				{
					return false;
				}
				break;
			}
			default :
			{
				res = false;
				break;
			}
		}
		return res;
	}

	private void checkAction(string Action)
	{
		GameObject go;
		switch (Action)
		{
			case ("closeDialog") :
			{
				killAtferDisplay = true;
				break;
			}
			case ("hasTalkedRaphael") :
			{
				go = getCharacGO("Raphael");
				go.GetComponent<Raphael>().hasTalkedRaphael = true;
				break;
			}
			case ("closeDialogVIP") :
			{
				go = GameObject.Find("DoorDanceToVIP(Clone)");
				go.GetComponent<LevelDoor>().locked = true;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogCanPutElectro") :
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().canPutElectro = true;
				go.GetComponent<Didier>().hasPutElectro = true;
				lvManager.setMusic(LevelManager.MusicList.Electro, LevelManager.levelList.Dancefloor);
				lvManager.musicLvl = LevelManager.MusicList.Electro;
				killAtferDisplay = true;
				break;
			}
			case ("knowsThomasPreferences") :
			{
				go = getCharacGO("Thomas");
				go.GetComponent<Thomas>().knowThomasPreferences = true;
				break;
			}
			case ("missionDidierEncours") :
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().missionDidierEncours = true;
				break;
			}

			case ("closeDialogBanCharlie") :
			{
				killAtferDisplay = true;
				go = getCharacGO("Charlie");
				go.GetComponent<Charlie>().dialDisabled = true;
				break;
			}
			case ("closeDialogMissionDidier") :
			{
				killAtferDisplay = true;
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().missionDidierEncours = true;
				break;
			}
			case ("disableChloe") :
			{
				go = getCharacGO("Chloe");
				go.GetComponent<Chloe>().dialDisabled = true;
				break;
			}
			case ("missionBastienEncours") :
			{
				killAtferDisplay = true;
				go = getCharacGO("Bastien");
				go.GetComponent<Bastien>().acceptedMission = true;
				go.GetComponent<Bastien>().dialToTrigger = "10014";
				break;
			}

			case ("kissRaphael") :
			{
				go = getCharacGO("Raphael");
				go.GetComponent<Raphael>().kissedPlayer = true; 
				break;
			}
			case ("closeDialogDisableBastien") :
			{
				go = getCharacGO("Bastien");
				go.GetComponent<Bastien>().refusedMission = true; 
				go.GetComponent<Bastien>().dialDisabled = true; 
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogKnowsChloe") :
			{
				go = getCharacGO("Chloe");
				go.GetComponent<Chloe>().knowsHomo = true; 
				go = getCharacGO("Bastien");
				go.GetComponent<Bastien>().dialDisabled = true; 
				killAtferDisplay = true;
				break;
			}

			case ("closeDialogDisablePaul") :
			{
				go = getCharacGO("Paul");
				go.GetComponent<Paul>().dialDisabled = true; 
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogGetAlex") :
			{
				go = getCharacGO("Alex");
				go.GetComponent<Alex>().voteForPlayer = true; 
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogMissionManonDone") :
			{
				go = getCharacGO("Manon");
				go.GetComponent<Manon>().missionDone = true;
				_Player.GetComponent<PlayerSim>().numberDrugs +=1;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogtalkedClaire") :
			{
				go = getCharacGO("Claire");
				go.GetComponent<Claire>().dialToTrigger = "14005"; 
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogTalkedClaireFlirting") :
			{
				go = getCharacGO("Claire");
				go.GetComponent<Claire>().talkedAboutFlirting = true;
				go.GetComponent<Claire>().dialToTrigger = "14005"; 
				killAtferDisplay = true;
				break;
			}


			case ("closeDialoghasTalkedThomas") :
			{
				go = getCharacGO("Thomas");
				go.GetComponent<Thomas>().hasTalkedThomas = true;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogYannickSpoken") :
			{
				killAtferDisplay = true;
				go = getCharacGO("Yannick");
				go.GetComponent<Yannick>().hasSpokenOnceToPlayer = true;
				go.GetComponent<Yannick>().dialToTrigger = "6007";
				go = getCharacGO("Chloe");
				go.GetComponent<Chloe>().tutoMode = false;
				go = getCharacGO("Vanessa");
				go.GetComponent<Vanessa>().tutoMode = false;
				break;
			}
			case ("changeMusicElectro") :
			{
				lvManager.setMusic(LevelManager.MusicList.Electro, LevelManager.levelList.Dancefloor);
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogDisableVanessa") :
			{
				go = getCharacGO("Vanessa");
				go.GetComponent<Vanessa>().dialDisabled = true;
				killAtferDisplay = true;
				break;
			}
			case ("changeMusicCountry") :
			{
				lvManager.setMusic(LevelManager.MusicList.Country, LevelManager.levelList.Dancefloor);
				GameObject _Vanessa = getCharacGO("Vanessa");
				_Vanessa.transform.position = new Vector3(1.52f, -1.87f,0);
				_Vanessa.transform.localScale = new Vector3(0.5630698f, 0.5630698f,0.5630698f);
				_Vanessa.GetComponentInChildren<OTSprite>().frameName = "vanessa_dancing";
				print (_Vanessa.GetComponentInChildren<OTSprite>().frameName);
				killAtferDisplay = true;
				break;
			}
			case ("changeMusicSlow") :
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().hasPutSlow = true;
				lvManager.setMusic(LevelManager.MusicList.Slow, LevelManager.levelList.Dancefloor);
				killAtferDisplay = true;
				break;
			}
			case ("changeMusicBase") :
			{
				lvManager.setMusic(LevelManager.MusicList.Groovy, LevelManager.levelList.Dancefloor);
				killAtferDisplay = true;
				break;
			}
			case ("getDrug") :
			{
				_Player.GetComponent<PlayerSim>().numberDrugs +=1;
				break;
			}
			case ("looseDrugManon") :
			{
				_Player.GetComponent<PlayerSim>().numberDrugs -=1;
				go = getCharacGO("Manon");
				go.GetComponent<Manon>().dialToTrigger = "2015";
				break;
			}
			case ("looseDrug") :
			{
				_Player.GetComponent<PlayerSim>().numberDrugs -=1;
				break;
			}
			case ("closeDialogunlockThomasBar"):
			{
				go = getCharacGO("Thomas");
				go.GetComponent<Thomas>().isBattleDance = true;
				go.GetComponent<Thomas>().dialToTrigger = "8010";
				go.GetComponent<Thomas>().hasTalkedThomas = true;
				killAtferDisplay = true;
				break;
			}
			case ("unlockSlow"):
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().canPutSlow = true;
				break;
			}
			case ("closeDialogunlockVanessaDance"):
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().canPutCountry = true;
				disableChar("Charlie");
				killAtferDisplay = true;
				break;
			}
			case ("PlayerKnowsPaulDealer") :
			{
				go = getCharacGO("Paul");
				go.GetComponent<Paul>().PlayerKnowsIsDealer = true;
				break;
			}
			case ("closeDialogPlayerKnowsPaulDealer") :
			{
				go = getCharacGO("Paul");
				go.GetComponent<Paul>().PlayerKnowsIsDealer = true;
				go = getCharacGO("Manon");
				go.GetComponent<Manon>().missionEncours = true;
				killAtferDisplay = true;
				break;
			}
			case ("missionDidierDone") :
			{
				go = getCharacGO("Didier");
				go.GetComponent<Didier>().missionDidierDone = true;
				go.GetComponent<Didier>().canPutElectro = true;
				break;
			}
			case ("playerSeeBob") :
			{
				go = getCharacGO("Bob");
				go.GetComponent<Bob>().unlocked = true;
				killAtferDisplay = true;
				break;
			}
			case ("playerGoVIP") :
			{
				_Player.GetComponent<PlayerSim>().canGoVIP = true;
				go = getCharacGO("Alex");
				go.GetComponent<Alex>().dialToTrigger = "11014";
				go.GetComponent<Alex>().gotPlayerInVIP = true;
				killAtferDisplay = true;
				break;
			}
			case ("triggerGroupGirls") :
			{
				lvManager.triggerDialogChloe = true;
				DialogUI.destroyDialog();
				break;
			}
			case ("closeDialogTriggerGroupMeuf") :
			{
				lvManager.triggerDialogVanessaGroup = true;
				DialogUI.destroyDialog();
				break;
			}
			case ("dialogGroupGuys") :
			{
				PlayerSim _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
				_player.numberDrugs -= 1;
				lvManager.triggerDialogGuys = true;
				DialogUI.destroyDialog();
				break;
			}
			case ("looseDestroyBob") :
			{
				go = getCharacGO("Bob");
				go.GetComponent<Bob>().unlocked = false;
				GameObject.Find("Level Manager").GetComponent<LevelManager>().calculateWin();
				killAtferDisplay = true;
				break;
			}
			case ("winDestroyBob") :
			{
				go = getCharacGO("Bob");
				go.GetComponent<Bob>().unlocked = true;
				GameObject.Find("Level Manager").GetComponent<LevelManager>().calculateWin();
				killAtferDisplay = true;
				break;
			}
			/////////////////////////////////////////////
			case ("closeDialogAndLoose2votes") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded -= 2;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogAndWin2votes") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 2;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogAndWin3votes") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 3;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogandWinStephane") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 1;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogkeepChloe") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 1;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogAndloose3votes") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded -= 3;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogKeepVanessa") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 1;
				killAtferDisplay = true;
				break;
			}
			case ("closeDialogAndlooseAll") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded -= 4;
				killAtferDisplay = true;
				break;
			}
			case ("unlockThomasPechoClaire") :
			{
				go = getCharacGO("Player");
				go.GetComponent<PlayerSim>().votesAdded += 1;
				killAtferDisplay = true;
				break;
			}
		}
		if (Action != "")
		{
			Debug.Log("Action " + Action);
		}
	}
	
	IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	    startDialog("");
	}

	IEnumerator twinkleText()
	{
		coroutineRunning = true;
		_NextDialog.enabled = !_NextDialog.enabled;
		yield return new WaitForSeconds(0.5f);
		coroutineRunning = false;
	}

	//Scrolling Coroutine
	IEnumerator StartScrolling() 
	{
		textIsScrolling = true;

		startLine = currentLine;
		displayText = "";
		//Display each letter one by one
		for(i = 0; i < talkLines[currentLine].Length; i++)
		{
			if(textIsScrolling && currentLine == startLine)
			{
				displayText += talkLines[currentLine][i];
				talkTextGUI.text = displayText;
				yield return new WaitForSeconds((float) (1f/ textScrollSpeed)); //Waiting textScrollSpeed between each update
			}

		}
		if (currentLine == talkLines.Length-1) 
		{
			finishedTalking = true;
			talking = false;
		}
		textIsScrolling = false; //Text is not scrolling anymore

	}

	private void disableChar(string charac)
	{
		GameObject go = getCharacGO(charac);
		go.GetComponent<CharSim>().dialDisabled = true;
	}

	private Color getCharColor(string charac)
	{
		GameObject go = getCharacGO(charac);
		Color col = go.GetComponent<CharSim>().colorDialogs;
		return col;
	}
	
	private void playWhispers()
	{
		if (finishedTalking != true || answer1 != null)
		{
			if (textIsScrolling)
			{
				currentChar.playWhispers(fullDialog[1], returnCharacName(fullDialog[1]).ToString());
			}
		}
		if(playerSpeaking == true) 
		{
			_Player.GetComponent<PlayerSim>().playWhispers();
		}

	}

	public void setCharacter(CharSim _chosenChar)
	{
		currentChar = _chosenChar;
	}

	private TextAsset getCharacXML(CharSim.charList currentChar)
	{
		//string res = "";
		return Resources.Load("dialogs_"+currentChar.ToString()) as TextAsset;
		// string url = "file:///"+Application.dataPath+"/"+"Dialogs/dialogs.xml";
		//res = "file:///" + Application.dataPath+ "/Dialogs/dialogs_"+currentChar.ToString() +".xml";
//		res = "http://4edges-games.com/images/stories/webExport/DancingQueen/Dialogs/dialogs_"+currentChar.ToString() +".xml";
		//return res;
	}

	private GameObject getCharacGO(String charac)
	{
		GameObject res;	
		res = GameObject.FindGameObjectWithTag(charac);
		return res;
	}
	private void banDialogID(string id)
	{
		_Player.GetComponent<PlayerSim>().addBanAnswers(id);

	}
	private void modifySympathy(string characID, int value)
	{
		GameObject go = getCharacGO("Yannick");
		switch (characID)
		{

		case ("06"):
		{
			go = getCharacGO("Yannick");
			go.GetComponent<Yannick>().sympathy_score += value;
			break;
		}
		case ("25"):
		{
			go = getCharacGO("Didier");
			go.GetComponent<Didier>().sympathy_score += value;
			break;
		}
		case ("10"):
		{
			go = getCharacGO("Bastien");
			go.GetComponent<Bastien>().sympathy_score += value;
			break;
		}
		case ("07"):
		{
			go = getCharacGO("Vanessa");
			go.GetComponent<Vanessa>().sympathy_score += value;
			break;
		}
		case ("05"):
		{
			go = getCharacGO("Bob");
			go.GetComponent<Bob>().sympathy_score += value;
			break;
		}
		case ("18"):
		{
			go = getCharacGO("Boris");
			go.GetComponent<Boris>().sympathy_score += value;
			break;
		}
		case ("11"):
		{
			go = getCharacGO("Alex");
			go.GetComponent<Alex>().sympathy_score += value;
			break;
		}
		case ("12"):
		{
			go = getCharacGO("Chloe");
			go.GetComponent<Chloe>().sympathy_score += value;
			break;
		}
		case ("19"):
		{
			go = getCharacGO("Charlie");
			go.GetComponent<Charlie>().sympathy_score += value;
			break;
		}
		case ("09"):
		{
			go = getCharacGO("Paul");
			go.GetComponent<Paul>().sympathy_score += value;
			break;
		}
		case ("08"):
		{
			go = getCharacGO("Thomas");
			go.GetComponent<Thomas>().sympathy_score += value;
			break;
		}
		case ("15"):
		{
			go = getCharacGO("Stephane");
			go.GetComponent<Stephane>().sympathy_score += value;
			break;
		}
		case ("17"):
		{
			go = getCharacGO("Alice");
			go.GetComponent<Alice>().sympathy_score += value;
			break;
		}
		case ("14"):
		{
			go = getCharacGO("Claire");
			go.GetComponent<Claire>().sympathy_score += value;
			break;
		}
			
		case ("02"):
		{
			go = getCharacGO("Manon");
			go.GetComponent<Manon>().sympathy_score += value;
			break;
		}
			
		case ("13"):
		{
			go = getCharacGO("Raphael");
			go.GetComponent<Raphael>().sympathy_score += value;
			break;
		}
			
		case ("04"):
		{
			go = getCharacGO("Kara");
			go.GetComponent<Kara>().sympathy_score += value;
			break;
		}
		case ("16"):
		{
			go = getCharacGO("Kara");
			go.GetComponent<Kara>().sympathy_score += value;
			break;
		}
		case ("1"):
		{
			go = getCharacGO("Kara");
			go.GetComponent<Kara>().sympathy_score += value;
			break;
		}
		}
		
	}

	private CharSim.charList returnCharacName(string characID)
	{
		CharSim gameo = getCharacGO("Yannick").GetComponent<Yannick>();
		switch (characID)
		{
		case ("10"):
		{
			gameo = getCharacGO("Bastien").GetComponent<Bastien>();
			break;
		}
		case ("06"):
		{
			gameo = getCharacGO("Yannick").GetComponent<Yannick>();
			break;
		}
		case ("25"):
		{
			gameo = getCharacGO("Didier").GetComponent<Didier>();
			break;
		}
		case ("07"):
		{
			gameo = getCharacGO("Vanessa").GetComponent<Vanessa>();
			break;
		}
		case ("05"):
		{
			gameo = getCharacGO("Bob").GetComponent<Bob>();
			break;
		}
		case ("18"):
		{
			gameo = getCharacGO("Boris").GetComponent<Boris>();
			break;
		}
		case ("11"):
		{
			gameo = getCharacGO("Alex").GetComponent<Alex>();
			break;
		}
		case ("12"):
		{
			gameo = getCharacGO("Chloe").GetComponent<Chloe>();
			break;
		}
		case ("19"):
		{
			gameo = getCharacGO("Charlie").GetComponent<Charlie>();
			break;
		}
		case ("09"):
		{
			gameo = getCharacGO("Paul").GetComponent<Paul>();
			break;
		}
		case ("08"):
		{
			gameo = getCharacGO("Thomas").GetComponent<Thomas>();
			break;
		}
		case ("15"):
		{
			gameo = getCharacGO("Stephane").GetComponent<Stephane>();
			break;
		}
		case ("17"):
		{
			gameo = getCharacGO("Christine").GetComponent<Christine>();
			break;
		}
		case ("14"):
		{
			gameo = getCharacGO("Claire").GetComponent<Claire>();
			break;
		}
			
		case ("02"):
		{
			gameo = getCharacGO("Manon").GetComponent<Manon>();
			break;
		}
			
		case ("13"):
		{
			gameo = getCharacGO("Raphael").GetComponent<Raphael>();
			break;
		}
			
		case ("04"):
		{
			gameo = getCharacGO("Kara").GetComponent<Kara>();
			break;
		}
		case ("03"):
		{
			gameo = getCharacGO("Boys").GetComponent<Boys>();
			break;
		}
		case ("23"):
		{
			gameo = getCharacGO("Jeremie").GetComponent<Jeremie>();
			break;
		}
		case ("20"):
		{
			gameo = getCharacGO("Dominique").GetComponent<Dominique>();
			break;
		}
		case ("16"):
		{
			gameo = getCharacGO("Alice").GetComponent<Alice>();
			break;
		}
		default :
		{
			Debug.Log("Misses definition of char id");
			break;
		}
		}
		return gameo.charac;
	}

	private bool checkAnswersInitiated()
	{
		bool res = true;
		if (answer1 == null) res = false;
		if (answer2 == null) res = false;
		if (answer3 == null) res = false;
		if (answer4 == null) res = false;
		if (answer5 == null) res = false;
		if (answer6 == null) res = false;
		return res;

	}

	public void GameDialog()
	{	
		
	}
	
	private void GameStart()
	{
		
	}
	
	private void GamePause()
	{
		
	}
	
	private void GameUnpause()
	{
		
		
	}
	private void GameOver()
	{
		
	}

}