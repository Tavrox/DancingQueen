using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	
	public OTSprite background;
	[SerializeField] private Player player;
	public Waypoints waypoint1;
	public Waypoints waypoint2;
	
	public int ID;
	public int nextLvlID;
	public int previousLvlID;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
