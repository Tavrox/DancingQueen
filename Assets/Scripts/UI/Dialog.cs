﻿using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {
	
	public int id;
	public string[] dialLines;
	public Answer[] answers;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string getLines () {
		return(dialLines[0]);
	}
}
