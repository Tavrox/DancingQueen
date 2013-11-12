using UnityEngine;
using System.Collections;

public class CharacterPositions : MonoBehaviour 
{
	public int posX;
	public int posY;
	public int depth;
	public enum placeList
	{
		Bar,
		Dancefloor,
		Toilets,
		VIP
	}
	public placeList place;
}
