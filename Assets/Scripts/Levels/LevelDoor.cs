using UnityEngine;
using System.Collections;

public class LevelDoor : MonoBehaviour {
	
	public enum doorType { BeginLevel, EndLevel }
	public doorType myDoorType;
	public enum levelList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public levelList goToLevel;
	
	// Use this for initialization
	void Start () {
		GameEventManager.NextLevel += NextLevel;
		GameEventManager.PreviousLevel += PreviousLevel;
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
						Application.LoadLevel("Bar");
						break;
					}
				case (levelList.Dancefloor) :
					{
						Application.LoadLevel("Dancefloor");
						break;
					}
				case (levelList.Toilets) :
					{
						Application.LoadLevel("Toilets");
						break;
					}
				case (levelList.VIP) :
					{
						Application.LoadLevel("VIP");
						break;
					}
			}
	}
	
	private void PreviousLevel () 
	{
		switch (goToLevel)
			{
				case (levelList.Bar) :
					{
						Application.LoadLevel("Bar");
						break;
					}
				case (levelList.Dancefloor) :
					{
						Application.LoadLevel("Dancefloor");
						break;
					}
				case (levelList.Toilets) :
					{
						Application.LoadLevel("Toilets");
						break;
					}
				case (levelList.VIP) :
					{
						Application.LoadLevel("VIP");
						break;
					}
			}
	}
	private void OnMouseDown()
	{
		if (DialogUI.exists != true)
		{
			if(myDoorType.ToString()=="BeginLevel") GameEventManager.TriggerPreviousLevel();
			else GameEventManager.TriggerNextLevel();
			
		}
		
	}
	
}
