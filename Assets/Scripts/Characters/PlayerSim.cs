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
				break;
			}
				
			case 1 :
			{
				drugN1.renderer.enabled = true;
				drugN2.renderer.enabled = false;
				break;
			}
			case 2 :
			{	
				drugN1.renderer.enabled = true;
				drugN2.renderer.enabled = true;
				break;
			}
				
			default :
			{
				drugN1.renderer.enabled = false;
				drugN2.renderer.enabled = false;
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
		float randDelay = Random.Range(randomDelayMin,randomDelayMax);
		string transfRand;
		string charStr = "0" + characterID + "_" + "Kara";
		if (rand < 10)
		{
			transfRand = "0" + rand.ToString();
		}
		else
		{
			transfRand = rand.ToString();
		}
		PlaySoundResult psr = MasterAudio.PlaySound(charStr, 1f, 1f, 0, charStr + "_" + transfRand );
//		print ("Playing group : "  + charStr);
//		print ("Playing delay : "  + randDelay);
//		print ("Playing sound variation : "  + charStr + "_" + transfRand);
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
