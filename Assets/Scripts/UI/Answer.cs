using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour {
	
	public int id;
	public string choice, answerLine;
	public Dialog nextDialog;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseOver () {
		print ("answer hover");	
	}
}
