using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerSim : MonoBehaviour {

	public int numberDrugs = 0;
	public List<string> banAnswers = new List<string>();
	public int votesAdded;
	public enum langList {  fr, en };
	public langList langChosen = langList.fr;
	public OTSprite drugN1;
	public OTSprite drugN2;

	public string whisperSound;
	public int minRandomVarWhispers, maxRandomVarWhispers;
	[Range (0,5)] public float randomDelayMin = 0f;
	[Range (0,5)] public float randomDelayMax = 0.01f;
	[Range (0,5)] public float frequencyWhispers = 0.5f;
	public string characterID;
	public bool canGoVIP = false;
	public bool modeIntro = true;
	private OTSprite black;
	
	void Start()
	{

	}
	void Update()
	{
		if (DialogUI.exists == false)
		{
			switch (numberDrugs)
			{

			case 0 : 
			{
				drugN1.renderer.enabled = false;
				drugN1.renderer.enabled = false;
				Debug.Log("0Drug");
				break;
			}
				
			case 1 :
			{
				drugN1.renderer.enabled = true;
				drugN2.renderer.enabled = false;
				Debug.Log("1Drug");
				break;
			}
			case 2 :
			{	
				drugN1.renderer.enabled = true;
				drugN2.renderer.enabled = true;
				Debug.Log("2Drug");
				break;
			}
				
			default :
			{
				drugN1.renderer.enabled = false;
				drugN2.renderer.enabled = false;
				print ("Default case drugs");
				break;
			}
			}
		}

	}

	public void reloadCharacs()
	{

	}

	public void addBanAnswers(string id)
	{
		banAnswers.Add(id);
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

	public void playWhispers()
	{
		int rand = Random.Range(minRandomVarWhispers,maxRandomVarWhispers);
		string transfRand;
		if (rand < 10)
		{
			transfRand = "0" + rand.ToString();
		}
		else
		{
			transfRand = rand.ToString();
		}
		PlaySoundResult psr = MasterAudio.PlaySound("0" + characterID + "_" + "Kara");
	}
	private void fadeToBlack()
	{
		InvokeRepeating("raiseAlpha", 0f, 0.5f);
		DialogUI.exists = true;
	}
	
	private void fadeToWhite()
	{
		InvokeRepeating("lowerAlpha", 0f, 0.5f);
		DialogUI.exists = false;
	}
	private void raiseAlpha()
	{
		if (black.alpha < 1)
		{
			black.alpha += 0.1f;
		}
		else
		{
			CancelInvoke();
		}
	}
	
	private void lowerAlpha()
	{
		if (black.alpha > 0)
		{
			black.alpha -= 0.1f;
		}
		else
		{
			modeIntro = false;
			DialogUI.exists = false;
			CancelInvoke();
		}
	}
	
}
