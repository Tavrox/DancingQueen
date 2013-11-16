using UnityEngine;
using System.Collections;

public class TestMode : MonoBehaviour {
	
	public int[] wantedTweaks;

	
	// Use this for initialization
	void Start () 
	{
	}
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			GameObject.Find("Level Manager").GetComponent<LevelManager>();
		}
	
	}
	
	void OnGUI()
	{
//		moveVel = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), moveVel, 0f, 10f);
		
	}
}
