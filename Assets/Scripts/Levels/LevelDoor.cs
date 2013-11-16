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
		OTSprite spr = GameObject.Find("cursorSprite").GetComponent<OTSprite>();
		spr.frameName = "cursor_use";

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
		if (locked != true)
		{
			if (DialogUI.exists != true)
			{
				if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerNextLevel();
				else GameEventManager.TriggerNextLevel();
				MasterAudio.PlaySound("Porte_full", "porte_full",0f);
				lvManager.changeRoom(this.goToLevel);
			}
		}
	}
	
}
