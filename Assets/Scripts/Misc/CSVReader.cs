using UnityEngine;
using System.Collections;

public class CSVReader : MonoBehaviour {
	
	public TextAsset textFile;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setTextFile(string path)
	{
//		textFile = path;
	}
	
	public void splitFile()
	{
		textFile.text.Split(","[0]);
		
	}
}
