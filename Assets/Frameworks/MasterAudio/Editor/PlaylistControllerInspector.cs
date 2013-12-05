using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlaylistController))]
public class PlaylistControllerInspector : Editor {
	public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
		EditorGUI.indentLevel = 0;
		
		//PlaylistController controller = (PlaylistController)target;
		
		var ma = MasterAudio.Instance;
		if (ma != null) {
			GUIHelper.DrawTexture(ma.logoTexture);
		}
		
		if (GUI.changed) {
			EditorUtility.SetDirty(target);
		}

		GUIHelper.RepaintIfUndoOrRedo(this);

		//DrawDefaultInspector();
    }
}
