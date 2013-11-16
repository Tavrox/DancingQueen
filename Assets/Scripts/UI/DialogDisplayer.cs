using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class DialogDisplayer : MonoBehaviour {
	
	public GUIText talkTextGUI, answer1TextGUI, answer2TextGUI, answer3TextGUI,answer4TextGUI,answer5TextGUI,answer6TextGUI, speaker;	//Text object
	[HideInInspector] public string[] talkLines;	//Array containing all the sentences of the dialog
	public int textScrollSpeed = 20;
	
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
	private XmlDocument xmlDoc;
	private XmlNodeList dialogNodes;
	private string CharDialID;
	private string[] fullDialog = new string[8];
	private string[,] arrayAnswers = new string[8,10];
	private bool answersInitiated;
	private GameObject instance1,instance2,instance3,instance4,instance5,instance6, prefabSprite; 

	//[HideInInspector] public Player player; //The player to lock
	
	IEnumerator Start() {
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
		/**ADD**/
		//string url = "http://paultondeur.com/files/2010/UnityExternalJSONXML/books.xml";
		string url = "file:///"+Application.dataPath+"/"+"Dialogs/dialogs.xml";
		WWW www = new WWW(url);
		
		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		yield return www;
		
		if (www.error == null)
		{
			//Sucessfully loaded the XML
			
			//Create a new XML document out of the loaded data
			xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(www.text);
			
			//Point to the book nodes and process them
			dialogNodes = xmlDoc.SelectNodes("dialogs/dialog");
		}
		CharDialID = "18002";
		getDialogContent();

		for(int j = 0; j < arrayAnswers.Length - 1; j++) {
			int k;
			for(k = 0; k < 7; k++) 
			{
				if(arrayAnswers[j,k]==null) break;
			}
			if(arrayAnswers[j,k]==null) break;
		}
		
		setCharacter(LevelManager.currentCharacterSpeaking);
		InvokeRepeating("playWhispers", 0 , currentChar.frequencyWhispers);
		StartCoroutine( Wait(delayStart) );
	}

	public void getDialogContent() {
		foreach (XmlNode node in dialogNodes)
		{
			if(node.Attributes.GetNamedItem("fullID").Value == CharDialID)
			{
				fullDialog = new string[8];
             	arrayAnswers = new string[8,10];

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
					arrayAnswers[i,3] = answer.Attributes.GetNamedItem("Sympathy_value").Value;
					arrayAnswers[i,4] = answer.Attributes.GetNamedItem("Condition").Value;
					arrayAnswers[i,5] = answer.Attributes.GetNamedItem("Action").Value;
					arrayAnswers[i,6] = answer.SelectSingleNode("fr").Attributes.GetNamedItem("Choice_fr").Value;
					arrayAnswers[i,7] = answer.SelectSingleNode("en").Attributes.GetNamedItem("Choice_en").Value;
					arrayAnswers[i,8] = answer.SelectSingleNode("fr").InnerText;
					arrayAnswers[i,9] = answer.SelectSingleNode("en").InnerText;
					i++;
				}
				dialToDisplay = firstDialog;
				talkLines = dialToDisplay.dialLines;
				talkLines[0]=fullDialog[5];
				break;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//print (currentChar);
		/*
				if(null) FindObjectOfType(typeof(Dialog)); 
				//player = GameObject.FindWithTag("Player").GetComponent<Player>();
				//player = GameObject.Find("Pop1").GetComponent<Player>();	//Get the player
				
				//If the text is scrolling
				StopCoroutine("startScrolling"); //Stop the scroll effect					
					//talkTextGUI.text = talkLines[currentLine]; //STATIC
	          	talkTextGUI.text = talkLines[currentLine];	//Display the whole line
	          	textIsScrolling = false;	//The text is not scrolling anymore
	          	if(buttonRep1.clic) 
				{
					print(buttonRep1.clic);
					buttonRep1.clic = false;
					
				}
				if(lockPlayer) player.enabled = false;	//Lock the player if lockPlayer is true
		*/
		//print ("Character talking ?" + talking);
		//print ("Char Finished talking ?" + finishedTalking);
		if(talking == true)
		{
//			 StartCoroutine("PlayAudioDialog");
			if(Input.GetMouseButtonDown(0)) 
			{ 
				
				//Dialog interaction button detection
				if(textIsScrolling)
				{
	            }
				else {
					if(currentLine < talkLines.Length - 1)
					{ 
						//If the text is not scrolling and still lines to read
						currentLine++;	//Go to next line
						StartCoroutine("StartScrolling");	//Start scroll effect
					}
					else
					{	//If there is no more lines to read
//						currentLine = 0;	//reset the current line number (for next reading time)
						//talkTextGUI.text = "";	//Empty the text object
//						print ("Reached End");
//						talking = false; 	//Not talking anymore
						
						//if(lockPlayer) player.enabled = true;	//Unlock the playe if needed
					}
	          	}
			}
			
		}
		if (finishedTalking == true)
		{
			if(fullDialog[4]!="closeDialog") 
			{
				InstantiateAnswers();
				finishedTalking =false;
				answersInitiated =true;
			}
			else {
				if(Input.GetMouseButtonDown(0)) {
					finishedTalking =false;
					DialogUI.destroyDialog();
				}
			}
		}
		if(answersInitiated) {
			answer1TextGUI.text = answer1.choice;
			answer2TextGUI.text = answer2.choice;
			answer3TextGUI.text = answer3.choice;
			answer4TextGUI.text = answer4.choice;
			answer5TextGUI.text = answer5.choice;
			answer6TextGUI.text = answer6.choice;
			if (answer1.triggered == true)
			{
				CharDialID = answer1.ID_nextDialog;
				launchNextDialog();
			}
			if (answer2.triggered == true)
			{
				CharDialID = answer2.ID_nextDialog;
				launchNextDialog();
			}
			if (answer3.triggered == true)
			{
				CharDialID = answer3.ID_nextDialog;
				launchNextDialog();
			}
			if (answer4.triggered == true)
			{
				CharDialID = answer4.ID_nextDialog;
				launchNextDialog();
			}
			if (answer5.triggered == true)
			{
				CharDialID = answer5.ID_nextDialog;
				launchNextDialog();
			}
			if (answer6.triggered == true)
			{
				CharDialID = answer6.ID_nextDialog;
				launchNextDialog();
			}
		}
	}
	
	//Call to the dialog from another class
	public void startDialog() 
	{
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
		prefabSprite = Resources.Load("03UI/Answer1") as GameObject;
		if (answer1 == null)
		{
			instance1 = Instantiate(prefabSprite) as GameObject;
			answer1 = instance1.GetComponent<Answer>();
			answer1TextGUI = instance1.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer1.setAnswerLine(arrayAnswers[0,8]);
			answer1.setChoice(arrayAnswers[0,6]);
			answer1.setNextDialog(arrayAnswers[0,2]);
			answer1.setAction(arrayAnswers[0,4]);
			
//			print("HAHAHAHAHAH"+answer1.choice);
		}
		if (answer2 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer2") as GameObject;
			instance2 = Instantiate(prefabSprite) as GameObject;
			answer2TextGUI = instance2.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer2 = instance2.GetComponent<Answer>();
			answer2.setAnswerLine(arrayAnswers[1,8]);
			answer2.setChoice(arrayAnswers[1,6]);
			answer2.setNextDialog(arrayAnswers[1,2]);
			answer2.setAction(arrayAnswers[2,4]);
		}
		if (answer3 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer3") as GameObject;
			instance3 = Instantiate(prefabSprite) as GameObject;
			answer3TextGUI = instance3.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer3 = instance3.GetComponent<Answer>();
			answer3.setAnswerLine(arrayAnswers[2,8]);
			answer3.setChoice(arrayAnswers[2,6]);
			answer3.setNextDialog(arrayAnswers[2,2]);
			answer3.setAction(arrayAnswers[2,4]);
		}
		if (answer4 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer4") as GameObject;
			instance4 = Instantiate(prefabSprite) as GameObject;
			answer4TextGUI = instance4.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer4 = instance4.GetComponent<Answer>();
			answer4.setAnswerLine(arrayAnswers[3,8]);
			answer4.setChoice(arrayAnswers[3,6]);
			answer4.setNextDialog(arrayAnswers[3,2]);
			answer4.setAction(arrayAnswers[3,4]);
			print ("Content Answer 4 : " + answer4.choice);
			if (answer4.choice == "" || answer4.choice == null )
			{
				print ("Destroy Answer4");
				answer4.GetComponentInChildren<OTSprite>().renderer.enabled = false;
				answer4.collider.enabled = false;
				answer4.enabled = false;
			}
		}
		if (answer5 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer5") as GameObject;
			instance5 = Instantiate(prefabSprite) as GameObject;
			answer5TextGUI = instance5.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer5 = instance5.GetComponent<Answer>();
			answer5.setAnswerLine(arrayAnswers[4,8]);
			answer5.setChoice(arrayAnswers[4,6]);
			answer5.setNextDialog(arrayAnswers[4,2]);
			answer5.setAction(arrayAnswers[4,4]);
		}
		if (answer6 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer6") as GameObject;
			instance6 = Instantiate(prefabSprite) as GameObject;
			answer6TextGUI = instance6.GetComponent<Answer>().GetComponentInChildren<GUIText>();
			answer6 = instance6.GetComponent<Answer>();
			answer6.setAnswerLine(arrayAnswers[5,8]);
			answer6.setChoice(arrayAnswers[5,6]);
			answer6.setNextDialog(arrayAnswers[5,2]);
			answer6.setAction(arrayAnswers[5,4]);
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
	}
	
	public void launchNextDialog()
	{
		getDialogContent();
		DestroyAnswers();
		answersInitiated = false;
		answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = answer4TextGUI.enabled = answer5TextGUI.enabled = answer6TextGUI.enabled = true;
		talking = true;
		currentLine = 0;
		StartCoroutine("StartScrolling");
	}
	
	private Dialog findNextDialog()
	{
		Dialog dial = new Dialog();
		dial.fetchLines();
		// Seek ID of current answer.
		// Seek ID of Next dialog according to current answer
		
		
		//FORNOW
		return dial;
		
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
	
	IEnumerator Wait(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	    startDialog();
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
			if(textIsScrolling && currentLine == startLine){
				
				yield return new WaitForSeconds((float) (1f/ textScrollSpeed)); //Waiting textScrollSpeed between each update
				displayText += talkLines[currentLine][i];
				talkTextGUI.text = displayText;
				//print ("text is scrolling");
			}
		}
		
		if (currentLine == talkLines.Length-1) 
		{
			//print ("Reached End of Dialog");
			finishedTalking = true;
			talking = false;
		}
		textIsScrolling = false; //Text is not scrolling anymore
	}
	
	private void playWhispers()
	{
		currentChar.playWhispers();
	}
	public void setCharacter(CharSim _chosenChar)
	{
		currentChar = _chosenChar;
	}
}