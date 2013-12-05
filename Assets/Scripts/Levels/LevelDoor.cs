using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {
	
	public enum doorType { EndLevel }
	public doorType myDoorType;

	public LevelManager.levelList goToLevel;
	private LevelManager lvManager;

	public bool locked = false;
	public bool delayTransitionOver = false;
	
	// Use this for initialization
	void Start () 
	{ 
		lvManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		GameEventManager.NextLevel += NextLevel;
		StartCoroutine(WaitTransition(4f));
	}
	void Update()
	{
		Bob _bob = GameObject.FindGameObjectWithTag("Bob").GetComponent<Bob>();
		if ( _bob.unlocked == true)
		{
			this.locked = false;
		}
		PlayerSim _play = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		if (_play.canGoVIP == true)
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
		if (DialogUI.exists != true && locked != true  && IngameUI.trombiTrigg == false && delayTransitionOver == true)
		{
			OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
			spr.frameName = "cursor_use";
		}
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

	IEnumerator WaitTransition(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		delayTransitionOver = true;
	}
	private void OnMouseDown()
	{
		if (locked != true  && delayTransitionOver == true)
		{
			if (DialogUI.exists != true  && locked != true)
			{
				MasterAudio.PlaySound("Porte",1f,1f ,0f, "Porte");
				StartCoroutine( openDoor(1.15f) );

			}
		}
	}

	IEnumerator openDoor(float wait)
	{
		yield return new WaitForSeconds(wait);
		if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerNextLevel();
		else GameEventManager.TriggerNextLevel();
		lvManager.changeRoom(this.goToLevel);
	}
}
