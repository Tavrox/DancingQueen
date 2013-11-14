using UnityEngine;
using System.Collections;

public class DialogDisplayer : MonoBehaviour {
	
	public GUIText talkTextGUI, answer1TextGUI, answer2TextGUI, answer3TextGUI, speaker;	//Text object
	[HideInInspector] public string[] talkLines;	//Array containing all the sentences of the dialog
	public int textScrollSpeed = 20;
	
	public Dialog firstDialog;
	public Answer answer1, answer2, answer3;
	public int delayStart = 3;
	
	private bool talking;	//The dialog is displayed
	private bool finishedTalking;
	private bool textIsScrolling;	//The text is currently scrolling
	private int currentLine;	//Current line read
	private int startLine, i;
	private string displayText; //The text displayed
	private Dialog dialToDisplay;
	
	//[HideInInspector] public Player player; //The player to lock
	
	
	void Start() {
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
		
		dialToDisplay = firstDialog;
		talkLines = dialToDisplay.dialLines;
//		answer1TextGUI.text = answer1.answerLine;
//		answer2TextGUI.text = answer2.answerLine;
//		answer3TextGUI.text = answer3.answerLine;
		answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = false;
		
			InvokeRepeating("playWhispers",0, 0.5f);
		StartCoroutine( Wait(delayStart) );
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		print ("Character talking ?" + talking);
		print ("Char Finished talking ?" + finishedTalking);
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
			InstantiateAnswers();
			print ("Answer 1 triggered :" + answer1.name + answer1.triggered);
			if (answer1.triggered == true)
			{
				print ("launch next dialog");
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
		GameObject prefabSprite = Resources.Load("03UI/Answer1") as GameObject;
		if (answer1 == null)
		{
			GameObject instance = Instantiate(prefabSprite) as GameObject;
			answer1 = instance.GetComponent<Answer>();
		}
		if (answer2 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer2") as GameObject;
			GameObject instance = Instantiate(prefabSprite) as GameObject;
			answer2 = instance.GetComponent<Answer>();
		}
		if (answer3 == null)
		{
			prefabSprite = Resources.Load("03UI/Answer3") as GameObject;
			GameObject instance = Instantiate(prefabSprite) as GameObject;
			answer3 = instance.GetComponent<Answer>();
		}
		
		
		
		 
		
	}
	
	private void DestroyAnswers()
	{
		Destroy(answer1);
		Destroy(answer2);
		Destroy(answer3);
	}
	
	public void launchNextDialog()
	{
		answer1.nextDialog = findNextDialog();
		dialToDisplay = answer1.nextDialog;
		talkLines = dialToDisplay.dialLines;
		answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = false;
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
				print ("text is scrolling");
			}
		}
		
		if (currentLine == talkLines.Length-1) 
		{
			
			print ("Reached End of Dialog");
			finishedTalking = true;
			talking = false;
		}
		textIsScrolling = false; //Text is not scrolling anymore
	}
	
	IEnumerator PlayAudioDialog()
	{
		yield return new WaitForSeconds(10f);
		print ("Sound play");
		PlaySoundResult psr = MasterAudio.PlaySound("010_Bastien_01","010_Bastien_02", 0f);
		print (psr.SoundPlayed);
	}
	
	private void playWhispers()
	{
		MasterAudio.PlaySound("010_Bastien_01", "010_Bastien_" + Random.Range(0,0) + Random.Range(1,9) , Random.Range(0,3));
	}
}