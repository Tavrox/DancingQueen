using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerSim : MonoBehaviour {

	public int numberDrugs = 0;
	public List<string> banAnswers = new List<string>();
	public int votesAdded;
	public enum langList {  fr, en };
	public langList langChosen = langList.fr;
	void Start()
	{

	}
	void Update()
	{
		if (numberDrugs > 0)
		{

		}
	}

	public void reloadCharacs()
	{

	}

	public void addBanAnswers(string id)
	{
		banAnswers.Add(id);
		Debug.Log("ID BANNEDBIS " + id);
	}

	public bool findBanAnswers(string id)
	{
		bool result = false;
		if (banAnswers.Contains(id))
		{
			result = true;
		}
		else 
		{
			result = false;
		}
		return result;
	}
	
}
