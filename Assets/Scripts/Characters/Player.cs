using UnityEngine;
using System.Collections;

public class Player : Character {
	
	[HideInInspector] public Vector3 position;
	[HideInInspector] public Transform trans;
	[HideInInspector] public Dialog dialog;
	//public Skill skillLaunch;
	
//	public Skills skill_knife;
//	public Skills skill_axe;
//	public Skills skill_shield;
	public OTSprite menu;
	public OTAnimatingSprite currSprite;
	
	[SerializeField] private Rect hp_display;
	[SerializeField] private SoundSprite soundMan;
	[SerializeField] private ModulatedSound mdSound;
	
	public bool shootingKnife;
	[HideInInspector] public bool paused = false;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start();
		
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		
		enabled = false;
		spawnPos = thisTransform.position;
		soundMan = GetComponent<SoundSprite>();
		mdSound = GetComponent<ModulatedSound>();
		//dialog = GameObject.Find("Dialog").GetComponent<Dialog>();
	}
	
	// Update is called once per frame
	public void Update () 
	{
		
		mdSound.PercentSound(this);
		Debug.Log ("Player_Shield" + hasShield);
		UpdateMovement();
	}
	
	private void GameStart () {
		if(FindObjectOfType(typeof(Player)) && this != null) {
			transform.localPosition = spawnPos;
			enabled = true;
		}
	}
	
	private void GameOver () 
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		movingDir = moving.None;
	}
	private void GamePause()
	{
		enabled = false;
		isLeft = false;
		isRight = false;
		isJump = false;
		isPass = false;
		paused = true;
		movingDir = moving.None;
		
	}
	private void GameUnpause()
	{
		paused = false;
		enabled = true;	
	}
	
	////////// HP MANAGING ////////////////
	public void RegenHP(int _val)
	{
		HP += _val;
	}
	public void DiminishHP(int _val)
	{
		HP -= _val;
	}
	private void DisplayHP()
	{
		
	}
	////////////////////////////////////////
	
	public OTAnimatingSprite getSprite()
	{
		return currSprite;
	}
	public int getCurrentFrameIndex()
	{
		return currSprite.CurrentFrame().index;
	}
	private void manageLifeSound()
	{
		
	}
}
