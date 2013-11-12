using UnityEngine;
using UnityEditor;
using System.Collections;

public class TopicAsset : MonoBehaviour {

	[MenuItem("Assets/Create/Topic")]
    public static void CreateAsset ()
    {
			Topic top = new Topic();
			AssetDatabase.CreateAsset(top, "Assets/Topics/Test/test.asset");
    }
}
