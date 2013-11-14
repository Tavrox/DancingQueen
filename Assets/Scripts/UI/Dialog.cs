using UnityEngine;
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
		
		if (answers != null)
		{
			answers[0] = transform.parent.transform.parent.GetComponent<DialogDisplayer>().answer1;
			answers[1] = transform.parent.transform.parent.GetComponent<DialogDisplayer>().answer2;
			answers[2] = transform.parent.transform.parent.GetComponent<DialogDisplayer>().answer3;
			
		}
	
	}
	
	public string getLines () 
	{
		return(dialLines[0]);
	}
	
	public void fetchLines()
	{
		dialLines[0] = " blalblallfd/n slll";
	}
}
