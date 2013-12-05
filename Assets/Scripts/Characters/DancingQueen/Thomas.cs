using UnityEngine;
using System.Collections;

[System.Serializable]
public class Thomas : CharSim 
{
	[SerializeField] public bool isBattleDance = false;
	[SerializeField] public bool hasTalkedThomas = false;
	[SerializeField] public bool knowThomasPreferences = true;
	public bool hasHeardAboutClaire = false;

}