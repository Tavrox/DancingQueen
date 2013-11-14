using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogAsset : MonoBehaviour {
	
	public static DialogScript dial = new DialogScript();

	[MenuItem("Assets/Create/Dialog")]
    public static void CreateAsset ()
    {
			float rand = Random.Range(0,1000);
			string randStr = rand.ToString();
			AssetDatabase.CreateAsset(dial, "Assets/Resources/07Configs/Test/test" + randStr + ".asset");
    }
}