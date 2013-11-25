using UnityEngine;
using System.Collections;

public class TestMode : MonoBehaviour {
	
	public int[] wantedTweaks;
//	private GUI debugBoris;

	
	// Use this for initialization
	void Start () 
	{
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
	}
	
	void OnGUI()
	{

//		debugBoris = GUI.HorizontalSlider (new Rect (25, 25, 100, 30), moveVel, 0f, 10f);
		
	}
}
