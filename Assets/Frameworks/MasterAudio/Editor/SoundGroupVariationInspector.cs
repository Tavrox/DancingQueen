using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SoundGroupVariation))]
public class SoundGroupVariationInspector : Editor {
	public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
		
		EditorGUI.indentLevel = 0;
		var isDirty = false;
		
		SoundGroupVariation _variation = (SoundGroupVariation)target;
		
		if (_variation.logoTexture != null) {
			GUIHelper.DrawTexture(_variation.logoTexture);
		}
		
		var oldLocation = _variation.audLocation;
		var newLocation = (SoundGroupVariation.AudioLocation) EditorGUILayout.EnumPopup("Audio Origin", _variation.audLocation);

		if (newLocation != oldLocation) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Audio Origin");
			_variation.audLocation = newLocation;
		}
		
		switch (_variation.audLocation) {
			case SoundGroupVariation.AudioLocation.Clip:
				var newClip = (AudioClip) EditorGUILayout.ObjectField("Audio Clip", _variation.audio.clip, typeof(AudioClip), false);
				
				if (newClip != _variation.audio.clip) {
					UndoHelper.RecordObjectPropertyForUndo(_variation.audio, "assign Audio Clip");
					_variation.audio.clip = newClip; 
				}
				break;
			case SoundGroupVariation.AudioLocation.ResourceFile:
				if (oldLocation != _variation.audLocation) {
					if (_variation.audio.clip != null) {
						Debug.Log("Audio clip removed to prevent unnecessary memory usage on Resource file variation.");
					}
					_variation.audio.clip = null;
				}

				EditorGUILayout.BeginVertical();
				var anEvent = Event.current;
			
				GUI.color = Color.yellow;
				var dragArea = GUILayoutUtility.GetRect(0f, 20f,GUILayout.ExpandWidth(true));
				GUI.Box (dragArea, "Drag Resource Audio clip here to use its name!");
				GUI.color = Color.white;

				var newFilename = string.Empty;
				
				switch (anEvent.type) {
					case EventType.DragUpdated:
					case EventType.DragPerform:
						if(!dragArea.Contains(anEvent.mousePosition)) {
							break;
						}
						
						DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
						
						if(anEvent.type == EventType.DragPerform) {
							DragAndDrop.AcceptDrag();
							
							foreach (var dragged in DragAndDrop.objectReferences) {
								var aClip = dragged as AudioClip;
								if(aClip == null) {
									continue;
								}
								
								newFilename = aClip.name;
								if (newFilename != 	_variation.resourceFileName) {
									UndoHelper.RecordObjectPropertyForUndo(_variation, "change Resource filename");
								    _variation.resourceFileName = aClip.name;
								}
								break;
							}
						}
						Event.current.Use();
						break;
				}
				EditorGUILayout.EndVertical();
			
				newFilename = EditorGUILayout.TextField("Resource Filename", _variation.resourceFileName);
				if (newFilename != 	_variation.resourceFileName) {
					UndoHelper.RecordObjectPropertyForUndo(_variation, "change Resource filename");
					_variation.resourceFileName = newFilename;
				}
				break;
		}
		
		var newVolume = EditorGUILayout.Slider("Volume", _variation.audio.volume, 0f, 1f);
		if (newVolume != _variation.audio.volume) {
			UndoHelper.RecordObjectPropertyForUndo(_variation.audio, "change Volume");
			_variation.audio.volume = newVolume;
		}

		var newPitch = EditorGUILayout.Slider("Pitch", _variation.audio.pitch, -3f, 3f);
		if (newPitch!= _variation.audio.pitch) {
			UndoHelper.RecordObjectPropertyForUndo(_variation.audio, "change Pitch");
			_variation.audio.pitch = newPitch;
		}

		var newLoop = EditorGUILayout.Toggle("Loop Clip", _variation.audio.loop);
		if (newLoop != _variation.audio.loop) {
			UndoHelper.RecordObjectPropertyForUndo(_variation.audio, "toggle Loop");
			_variation.audio.loop = newLoop;
		}

		var newRandomPitch = EditorGUILayout.Slider("Random Pitch", _variation.randomPitch, 0f, 3f);
		if (newRandomPitch != _variation.randomPitch) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Random Pitch");
			_variation.randomPitch = newRandomPitch; 
		}

		var newRandomVolume = EditorGUILayout.Slider("Random Volume", _variation.randomVolume, 0f, 1f);
		if (newRandomVolume != _variation.randomVolume) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Random Volume");
			_variation.randomVolume = newRandomVolume;
		}

		var newWeight = EditorGUILayout.IntSlider("Weight (Instances)", _variation.weight, 0, 100);
		if (newWeight != _variation.weight) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Weight");
			_variation.weight = newWeight;
		}

		var newUseFades = EditorGUILayout.BeginToggleGroup("Use Custom Fading", _variation.useFades);
		if (newUseFades != _variation.useFades) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "toggle Use Custom Fading");
			_variation.useFades = newUseFades;
		}

		var newFadeIn = EditorGUILayout.Slider("Fade In Time (sec)", _variation.fadeInTime, 0f, 10f);
		if (newFadeIn != _variation.fadeInTime) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Fade In Time");
			_variation.fadeInTime = newFadeIn;
		}

		var newFadeOut = EditorGUILayout.Slider("Fade Out time (sec)", _variation.fadeOutTime, 0f, 10f);
		if (newFadeOut != _variation.fadeOutTime) {
			UndoHelper.RecordObjectPropertyForUndo(_variation, "change Fade Out Time");
			_variation.fadeOutTime = newFadeOut;
		}

		EditorGUILayout.EndToggleGroup();
		
		var ma = MasterAudio.Instance;
		if (ma != null) {
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.Space(10);
			GUILayout.Label("Actions");
			GUILayout.FlexibleSpace();
			var buttonPressed = GUIHelper.AddVariationButtons(ma, "Click to preview Variation");
			EditorGUILayout.EndHorizontal();
			
			if (!Application.isPlaying) {
				GUIHelper.ShowColorWarning("*Fading & random settings are ignored by preview in edit mode.");
			}
			
			switch (buttonPressed) {
				case GUIHelper.DTFunctionButtons.Play:
					if (Application.isPlaying) {
						MasterAudio.PlaySound(_variation.OriginalParent.name, 1f, null, 0f, _variation.name);
					} else {
						isDirty = true;
						PlaySound(_variation, _variation.audio);
					}
					break;
			}
		} 
		
		if (GUI.changed || isDirty) {
			EditorUtility.SetDirty(target);
		}	

		GUIHelper.RepaintIfUndoOrRedo(this);

		//DrawDefaultInspector();
    }
	
	private void PlaySound(SoundGroupVariation aVar, AudioSource aud) {
		aud.Stop();
		aud.Play();
	}
}
