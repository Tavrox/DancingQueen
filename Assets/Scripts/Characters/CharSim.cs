﻿using UnityEngine;
using System.Collections;

public class CharSim : MonoBehaviour {
	
	private Cursor cursor;
	private OTSprite full_body;
	private OTSprite dialog_pic;
	private DialogDisplayer dial;
	
	
	public enum charList
	{
		Vanessa,
		Bastien,
		Boris,
		Bob,
		Alex,
		Chloe,
		Charlie,
		Didier,
		Paul,
		Thomas,
		Yannick,
		Stephane,
		Anais,
		Claire,
		Christine,
		Manon,
		Raphael
	}
	public charList charac;
	public int sympathy_score = 0;
	public bool triggeredUltimate = false;
	public bool voteForPlayer = false;
	
	public string whisperSound;
	public string pathSoundGroup;
	public string pathSoundVariation;
	[Range (0,5)] public float frequencyWhispers = 0.5f;

	// Use this for initialization
	void Start () {
	
	
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		GameEventManager.GamePause += GamePause;
		GameEventManager.GameUnpause += GameUnpause;
		GameEventManager.GameDialog += GameDialog;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void lowerSympathy(int _val)
	{
		sympathy_score -= _val;
	}
	public void raiseSympathy(int _val)
	{
		sympathy_score += _val;
	}
	public void raiseGlobalSympathy(int _val)
	{
			// Machin.raiseSympa
	}
	public void lowerGlobalSympathy(int _val)
	{
	 	// Machin.lowerSympa
	}
	
	private void GameStart () 
	{
		
	}
	private void GameOver () 
	{
		
	}
	private void GamePause()
	{
	
		
	}
	private void GameUnpause()
	{
		collider.enabled = true;
	}
	
	private void GameDialog()
	{
		collider.enabled = false;
	}
	
	private void OnMouseDown()
	{
		if (DialogUI.exists != true)
		{
			DialogUI.createDialog(this);
			MasterAudio.PlaySound(pathSoundGroup, pathSoundVariation);
		}
	}
	
	public charList getEnumChar()
	{
		return charList.Vanessa;
	}
		
	public void playWhispers()
	{
		MasterAudio.PlaySound(whisperSound);
	}

}
