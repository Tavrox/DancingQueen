using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public OTSprite background;
	[SerializeField] private Player player;
	public Waypoints waypoint1;
	public Waypoints waypoint2;
	
	public bool thomasBattle = false;
	public bool chloeToilets = false;
	public bool vanessaSad = false;
	public bool raphaelSingle = false;
	public bool gameWon = false;
	
	public Thomas _Thomas;
	public Chloe _Chloe;
	public Raphael _Raphael;
	public Vanessa _Vanessa;
	
	public enum levelList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public levelList currentLvl;

	// Use this for initialization
	void Start () 
	{
		switch (currentLvl)
			{
				case (levelList.Dancefloor) :
					{
						if (_Vanessa.triggeredUltimate == true)
						{
							// Enlever Vanessa a la scène
							// Positionner Vanessa
						}
						if (_Thomas.isBattleDance == true)
						{
							// Enlever Thomas à la scène
							// Positionner Thomas
						}
						break;
					}
				case (levelList.Bar) :
					{
						if (_Vanessa.triggeredUltimate == true)
						{
							// Enlever Vanessa a la scène
							// Positionner Vanessa
						}
						if (_Thomas.isBattleDance == true)
						{
							// Ajouter Thomas de la scène
							// Positionner Thomas à la scène
						}
						break;
					}
				case (levelList.Toilets) :
					{
						if (_Chloe.triggeredUltimate == true)
						{
							// Ajouter Chloe a la scène
							// Positionner Chloe
						}
						if (_Raphael.coupleClaire == true)
						{
							// Ajouter Chloe a la scène
							// Positionner Chloe
						}
						break;
					}
				case (levelList.VIP) :
					{
						break;
					}
			}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
