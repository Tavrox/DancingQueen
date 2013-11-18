using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {
	
	public enum doorType { EndLevel }
	public doorType myDoorType;

	public LevelManager.levelList goToLevel;
	private LevelManager lvManager;

	public bool locked = false;
	
	// Use this for initialization
	void Start () 
	{ 

		lvManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		GameEventManager.NextLevel += NextLevel;
	}
	void Update()
	{
		Bob bobby = GameObject.FindGameObjectWithTag("Bob").GetComponent<Bob>();
		if ( bobby.unlocked == true)
		{
			this.locked = false;
		}

	}

	void OnTriggerEnter(Collider other)
    {
		if ( GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() != null)
		{
			if(other.gameObject.CompareTag("Player")) 
			{	
				if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerPreviousLevel();
				else GameEventManager.TriggerNextLevel();
			}	
		}
    }

	private void OnMouseOver()
	{
		GameObject _Yannick = GameObject.FindGameObjectWithTag("Yannick");
		/*
		if (_Yannick.GetComponent<Yannick>().hasSpokenOnceToPlayer == true)
		{*/
			if (DialogUI.exists != true && locked != true)
			{
				OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
				spr.frameName = "cursor_use";
			}
//		}
	}

	private void OnMouseExit()
	{
		OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
		spr.frameName = "cursor_default";
	}
	
	private void NextLevel ()
	{
		switch (goToLevel)
			{
					case (LevelManager.levelList.Bar) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Bar);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						lvManager.currentLvl = LevelManager.levelList.Bar;
						break;
					}
						case (LevelManager.levelList.Dancefloor) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Dancefloor);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
						case (LevelManager.levelList.Toilets) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Toilets);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
					case (LevelManager.levelList.VIP) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.VIP);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
			}
	}

	private void OnMouseDown()
	{
		GameObject _Yannick = GameObject.FindGameObjectWithTag("Yannick");
//		if (_Yannick.GetComponent<Yannick>().hasSpokenOnceToPlayer == true)
//		{
			if (locked != true)
			{
				if (DialogUI.exists != true  && locked != true)
				{
					MasterAudio.PlaySound("Porte", "Porte",0f);
					StartCoroutine( openDoor(1.15f) );

				}
			}
//		}
	}

	IEnumerator openDoor(float wait)
	{
		yield return new WaitForSeconds(wait);
		if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerNextLevel();
		else GameEventManager.TriggerNextLevel();
		lvManager.changeRoom(this.goToLevel);
	}
}
