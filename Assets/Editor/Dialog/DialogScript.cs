using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogScript : ScriptableObject {
	
	public Character charac;
	public List<DialogElement> DialogItems;
}
