using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AnswerAsset : MonoBehaviour {
	
	public static AnswerScript answ = new AnswerScript();
	
	[MenuItem("Assets/Create/Answer")]
    public static void CreateAsset ()
    {
			
			float rand = Random.Range(0,1000);
			string randStr = rand.ToString();
			AssetDatabase.CreateAsset(answ, "Assets/Resources/07Configs/Test/test" + randStr + "00.asset");

    }
}




