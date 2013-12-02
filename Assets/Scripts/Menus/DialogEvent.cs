using UnityEngine;
using System.Collections;

public class DialogEvent : MonoBehaviour {

	private GUIText notifier;
	private LevelManager _LevMan;
	private PlayerSim _Player;
	private float lifeTime = 3f;

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void triggerEvent()
	{
		GameObject prefabSprite = Resources.Load("03UI/Event") as GameObject;
		Instantiate(prefabSprite);
		notifier = GameObject.Find("EventNotifier").GetComponent<GUIText>();
		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		if (_Player.langChosen == PlayerSim.langList.en)
		{
			notifier.text = "Wait... Someone wants \n to tell you something !";
		}
		else
		{
			notifier.text = "Attends... quelqu'un \n vient te parler !";
		}

	}
	public void triggerDeath()
	{
		StartCoroutine("Wait", 3f);
	}

	public void destroyEvent()
	{
		Destroy (this);
		print("Event destroyed");
	}

	IEnumerator Wait(float life)
	{
		yield return new WaitForSeconds(life);
//		destroyEvent();
	}
}
