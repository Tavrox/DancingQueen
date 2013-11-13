using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharSimAsset : MonoBehaviour {

	[MenuItem("Assets/Create/CharSim")]
    public static void CreateAsset ()
    {
			CharSim top = new CharSim();
			AssetDatabase.CreateAsset(top, "Assets/Topics/Test/CharSim.asset");
    }
}
