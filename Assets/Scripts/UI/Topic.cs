using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;


[System.Serializable]
public class Topic : ScriptableObject {
	
	public Character charac;
	public List<TopicElement> TopicItems;
}
