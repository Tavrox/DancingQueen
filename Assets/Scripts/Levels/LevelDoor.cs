using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {
	
	public enum doorType { EndLevel }
	public doorType myDoorType;
	public enum levelList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public levelList goToLevel;
	private LevelManager lvManager;
	
	// Use this for initialization
	void Start () 
	{
		lvManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		GameEventManager.NextLevel += NextLevel;
	}
//	void Update () {
//		if(null) FindObjectOfType(typeof(LevectorMoveDoor));	
//	}
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
	
	private void NextLevel ()
	{
		switch (goToLevel)
			{
				case (levelList.Bar) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Bar);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						lvManager.currentLvl = LevelManager.levelList.Bar;
						break;
					}
				case (levelList.Dancefloor) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Dancefloor);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
				case (levelList.Toilets) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.Toilets);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
				case (levelList.VIP) :
					{
						string targetBg = lvManager.getBackground(LevelManager.levelList.VIP);
						GameObject.Find("Background").GetComponentInChildren<OTSprite>().frameName = targetBg;
						break;
					}
			}
	}

	private void OnMouseDown()
	{
		if (DialogUI.exists != true)
		{
			if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerNextLevel();
			else GameEventManager.TriggerNextLevel();
			lvManager.changeRoom();
		}
		
	}
	
}
