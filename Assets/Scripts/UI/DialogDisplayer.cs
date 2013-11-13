using UnityEngine;
using System.Collections;

public class DialogDisplayer : MonoBehaviour {
	
	public GUIText talkTextGUI, answer1TextGUI, answer2TextGUI, answer3TextGUI, speaker;	//Text object
	[HideInInspector] public string[] talkLines;	//Array containing all the sentences of the dialog
	public int textScrollSpeed = 20;
	
	public Dialog firstDialog;
	public Answer answer1, answer2, answer3;
	public AnswerButton buttonRep1, buttonRep2, buttonRep3;
	public int delayStart = 3;
	
	private bool talking;	//The dialog is displayed
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
		answer1TextGUI.text = answer1.answerLine;
		answer2TextGUI.text = answer2.answerLine;
		answer3TextGUI.text = answer3.answerLine;
		answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = false;
		
		StartCoroutine( Wait(delayStart) );
	}
	
	// Update is called once per frame
	void Update () {
		
		print ("Bool Talking : " + talking);
		/*	if(null) FindObjectOfType(typeof(Dialog)); */
		if(talking){
			
			
			/*
			//player = GameObject.FindWithTag("Player").GetComponent<Player>();
			//player = GameObject.Find("Pop1").GetComponent<Player>();	//Get the player
			//if(lockPlayer) player.enabled = false;	//Lock the player if lockPlayer is true
			*/
			if(Input.GetMouseButtonDown(0)) { 
				
				//Dialog interaction button detection
				if(textIsScrolling)
				{
					/*
					//If the text is scrolling
					StopCoroutine("startScrolling"); //Stop the scroll effect					
	              	talkTextGUI.text = talkLines[currentLine];	//Display the whole line
	              	textIsScrolling = false;	//The text is not scrolling anymore
					*/
	            }
				else {
					if(currentLine < talkLines.Length - 1)
					{ 
						//If the text is not scrolling and still lines to read
						currentLine++;	//Go to next line
						//talkTextGUI.text = talkLines[currentLine]; //STATIC
						StartCoroutine("startScrolling");	//Start scroll effect
					}
					else{	//If there is no more lines to read
						currentLine = 0;	//reset the current line number (for next reading time)
						//talkTextGUI.text = "";	//Empty the text object
						talking = false; 	//Not talking anymore
						
						//if(lockPlayer) player.enabled = true;	//Unlock the playe if needed
					}
	          	}
			}print ("talking");
		}
		if(buttonRep1.clic) 
		{
			print(buttonRep1.clic);
			buttonRep1.clic = false;
			dialToDisplay = answer1.nextDialog;
			talkLines = dialToDisplay.dialLines;
			answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = buttonRep1.enabled = buttonRep2.enabled = buttonRep3.enabled = false;
			talking=true;
			currentLine = 0;
			StartCoroutine("startScrolling");
		}
	}
	
	//Call to the dialog from another class
	public void startDialog() 
	{
		talking = true;	//Activtate talking state
		currentLine = 0;
		StartCoroutine("startScrolling");	//Start displaying text
	}
	
	//Activate the dialog when the player is in the collision box
	void OnTriggerEnter(Collider col) 
	{
//		if(col.gameObject.CompareTag("Player")) {
//			talking=true;
//			currentLine = 0;
//			StartCoroutine("startScrolling");
//			//col.animation.CrossFade("idle");
//		}
	}
	void OnMouseOver () 
	{
		
		
	}
	
	public void GameDialog()
	{	
		print (delayStart);
		
		/*
			if(!talking) 
			{
				talking=true;
				currentLine = 0;
				StartCoroutine("startScrolling");
			}
		*/
	}

	IEnumerator Wait(float waitTime)
	{
		print (waitTime);
		yield return new WaitForSeconds(waitTime);
	    startDialog();
		print ("dialog started");
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
	//Scrolling Coroutine
	IEnumerator startScrolling() 
	{
		textIsScrolling = true;
		startLine = currentLine;
		displayText = "";
		
		//Display each letter one by one
		for(i = 0; i < talkLines[currentLine].Length; i++){
			if(textIsScrolling && currentLine == startLine){
				
				yield return new WaitForSeconds((float) (1f/ textScrollSpeed)); //Waiting textScrollSpeed between each update
				displayText += talkLines[currentLine][i];
				talkTextGUI.text = displayText;
			}
		}
		
		if (currentLine == talkLines.Length-1) {
			answer1TextGUI.enabled = answer2TextGUI.enabled = answer3TextGUI.enabled = buttonRep1.enabled = buttonRep2.enabled = buttonRep3.enabled = true;
			talking = false;
		}
		print ("Scroll Coroutine");
		textIsScrolling = false; //Text is not scrolling anymore
		
	}
}