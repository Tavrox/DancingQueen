using UnityEngine;
using System.Collections;

public class AnswerButton : MonoBehaviour {
	
	[HideInInspector] public bool clic;
	
	// Use this for initialization
	void Start () {
		clic = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{
			clic = true;
		}
	}
}
