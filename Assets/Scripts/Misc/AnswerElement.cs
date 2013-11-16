using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnswerElement : MonoBehaviour {

	public GUIText Text;
	public int ID;
	public string Action;
	public enum CharList
	{
		Boris,
		Vanessa,
		Kara,
		Bob,
		Alex,
		Paul,
		Thomas,
		Yannick,
		Stephane
	};
	public CharList SpeakingCharacter;
	public string Condition;
}





