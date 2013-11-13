using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
	
	public static event GameEvent GameStart, GamePause, GameUnpause, GameOver, NextLevel, PreviousLevel, GameDialog;
	public static bool gamePaused = false;
	
	public static void TriggerGameStart(){
		if(GameStart != null){		
			Debug.Log("EnterState : GameStart");			
			GameStart();
		}
	}

	public static void TriggerGameOver(){
		if(GameOver != null){
			Debug.Log("EnterState : GameOver");
			GameOver();
		}
	}
	
	public static void TriggerNextLevel(){
		if(NextLevel != null){
			Debug.Log("EnterState : NextLevel");
			NextLevel();
		}
	}
	public static void TriggerPreviousLevel(){
		Debug.Log("EnterState : PreviousLevel");
		if(PreviousLevel != null){
			PreviousLevel();
		}
	}
	public static void TriggerGameDialog(){
		if(GameDialog != null){
			Debug.Log("EnterState : GameDialog");
			GameDialog();
		}
	}
	public static void TriggerGamePause()
	{
		if(GamePause != null)
		{
			Debug.Log("EnterState : GamePause");
			gamePaused = true;
			GamePause();
		}
	}
	public static void TriggerGameUnpause()
	{
		if(GameUnpause != null)
		{
			Debug.Log("EnterState : GameUnpause");
			gamePaused = false;
			GameUnpause();
		}
	}
}
