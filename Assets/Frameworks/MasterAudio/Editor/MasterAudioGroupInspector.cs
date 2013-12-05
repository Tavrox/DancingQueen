using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(MasterAudioGroup))]
public class MasterAudioGroupInspector : Editor {
	private bool isValid = true;
	
	public override void OnInspectorGUI() {
        EditorGUIUtility.LookLikeControls();
		
		EditorGUI.indentLevel = 0;
		var isDirty = false;
		
		MasterAudio ma = MasterAudio.Instance;
		if (ma == null) {
			isValid = false;
		} 
		
		MasterAudioGroup _group = (MasterAudioGroup)target;
		_group = RescanChildren(_group);
		
		if (!isValid) {
			return;
		}

		if (_group.logoTexture != null) {
			GUIHelper.DrawTexture(_group.logoTexture);
		}

		var newVol = EditorGUILayout.Slider("Group Master Volume", _group.groupMasterVolume, 0f, 1f);
		if (newVol != _group.groupMasterVolume) {
			UndoHelper.RecordObjectPropertyForUndo(_group, "change Group Master Volume");
			_group.groupMasterVolume = newVol;
		}

		var newRetrigger = EditorGUILayout.IntSlider("Retrigger Percentage", _group.retriggerPercentage, 0, 100);
		if (newRetrigger != _group.retriggerPercentage) {
			UndoHelper.RecordObjectPropertyForUndo(_group, "change Retrigger Percentage");
			_group.retriggerPercentage = newRetrigger;
		}

		var newVarMode = (MasterAudioGroup.VariationMode) EditorGUILayout.EnumPopup("Variation Mode", _group.curVariationMode);
		if (newVarMode != _group.curVariationMode) {
			UndoHelper.RecordObjectPropertyForUndo(_group, "change Variation Mode");
			_group.curVariationMode = newVarMode;
		}

		if (_group.curVariationMode == MasterAudioGroup.VariationMode.Normal) {
			var newLimitPoly = EditorGUILayout.Toggle("Limit Polyphony", _group.limitPolyphony);
			if (newLimitPoly != _group.limitPolyphony) {
				UndoHelper.RecordObjectPropertyForUndo(_group, "toggle Limit Polyphony");
				_group.limitPolyphony = newLimitPoly;
			}
			if (_group.limitPolyphony) {
				int maxVoices = 0;
				for (var i = 0; i < _group.groupVariations.Count; i++) {
					var variation = _group.groupVariations[i];
					maxVoices += variation.weight;
				}

				var newVoiceLimit = EditorGUILayout.IntSlider("Polyphony Voice Limit", _group.voiceLimitCount, 1, maxVoices);
				if (newVoiceLimit != _group.voiceLimitCount) {
					UndoHelper.RecordObjectPropertyForUndo(_group, "change Polyphony Voice Limit");
					_group.voiceLimitCount = newVoiceLimit;
				}
			}

			var newLimitMode = (MasterAudioGroup.LimitMode) EditorGUILayout.EnumPopup("Replay Limit Mode", _group.limitMode);
			if (newLimitMode != _group.limitMode) {
				UndoHelper.RecordObjectPropertyForUndo(_group, "change Replay Limit Mode");
				_group.limitMode = newLimitMode;
			}

			switch (_group.limitMode) {
				case MasterAudioGroup.LimitMode.FrameBased:
					var newFrameLimit = EditorGUILayout.IntSlider("Min Frames Between", _group.limitPerXFrames, 1, 120);
					if (newFrameLimit != _group.limitPerXFrames) {
						UndoHelper.RecordObjectPropertyForUndo(_group, "change Min Frames Between");
						_group.limitPerXFrames = newFrameLimit;
					}
					break;
				case MasterAudioGroup.LimitMode.TimeBased:
					var newMinTime = EditorGUILayout.Slider("Min Seconds Between", _group.minimumTimeBetween, 0.1f, 10f);
					if (newMinTime != _group.minimumTimeBetween) {
						UndoHelper.RecordObjectPropertyForUndo(_group, "change Min Seconds Between");
						_group.minimumTimeBetween = newMinTime;
					}
					break;
			}
		} else {
			GUIHelper.ShowColorWarning("*In this mode, only one variation can be played at a time.");
			GUIHelper.ShowColorWarning(" They will chain together randomly until you stop the Group.");
		}

		var newBulkMode = (SoundGroupVariation.AudioLocation) EditorGUILayout.EnumPopup("Bulk Variation Mode", _group.bulkVariationMode);
		if (newBulkMode != _group.bulkVariationMode) {
			UndoHelper.RecordObjectPropertyForUndo(_group, "change Bulk Variation Mode");
			_group.bulkVariationMode = newBulkMode;
		}
		if (_group.bulkVariationMode == SoundGroupVariation.AudioLocation.ResourceFile) {
			GUIHelper.ShowColorWarning("*Resource mode: make sure to drag from Resource folders only.");
		}

		int? deadChildIndex = null;
		
		if (!Application.isPlaying) {
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(4);
			GUILayout.Label("Actions", EditorStyles.wordWrappedLabel, GUILayout.Width(50f));
			GUILayout.Space(96);
			GUI.contentColor = Color.green;
			if (GUILayout.Button(new GUIContent("Equalize Weights", "Reset Weights to zero"), EditorStyles.toolbarButton, GUILayout.Width(120))) {
				isDirty = true;
				EqualizeWeights(_group);
			}	
			GUI.contentColor = Color.white;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
		
		if (!Application.isPlaying) {
			// new variation settings
			EditorGUILayout.BeginVertical();
			var anEvent = Event.current;
	
			GUI.color = Color.yellow;
			
			var dragArea = GUILayoutUtility.GetRect(0f,35f,GUILayout.ExpandWidth(true));
			GUI.Box (dragArea, "Drag Audio clips here to create variations!");
	
			GUI.color = Color.white;
			
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
							
							CreateVariation(_group, ma, aClip);
						}
					}
					Event.current.Use();
					break;
			}
			EditorGUILayout.EndVertical();
			// end new variation settings
		}

		if (_group.groupVariations.Count == 0) {
			GUIHelper.ShowColorWarning("You currently have no variations.");
		} else {
			for (var i = 0; i < _group.groupVariations.Count; i++) {
				var variation = _group.groupVariations[i];
	        	EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
				EditorGUILayout.LabelField(variation.name, EditorStyles.boldLabel);
	
				EditorGUILayout.EndHorizontal();
				
				if (variation.audio == null) {
					GUIHelper.ShowColorWarning(string.Format("The variation: '{0}' has no Audio Source.", variation.name));
					break;
				}
				
				var oldLocation = variation.audLocation;
				var newLocation = (SoundGroupVariation.AudioLocation) EditorGUILayout.EnumPopup("Audio Origin", variation.audLocation);
				if (newLocation != variation.audLocation) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Audio Origin");
					variation.audLocation = newLocation;
				}
				
				switch (variation.audLocation) {
					case SoundGroupVariation.AudioLocation.Clip:
						var newClip = (AudioClip) EditorGUILayout.ObjectField("Audio Clip", variation.audio.clip, typeof(AudioClip), false);
						if (newClip != variation.audio.clip) {
							UndoHelper.RecordObjectPropertyForUndo(variation.audio, "change Audio Clip");
							variation.audio.clip = newClip;
						}
						break;
					case SoundGroupVariation.AudioLocation.ResourceFile:
						if (oldLocation != variation.audLocation) {
							if (variation.audio.clip != null) {
								Debug.Log("Audio clip removed to prevent unnecessary memory usage on Resource file variation.");
							}
							variation.audio.clip = null;
						}
					
						EditorGUILayout.BeginVertical();
						var anEvent = Event.current;
					
						GUI.color = Color.yellow;
						var dragArea = GUILayoutUtility.GetRect(0f, 20f,GUILayout.ExpandWidth(true));
						GUI.Box (dragArea, "Drag Resource Audio clip here to use its name!");
						GUI.color = Color.white;
						
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
										
										UndoHelper.RecordObjectPropertyForUndo(variation, "change Resource Filename");
										variation.resourceFileName = aClip.name;
									}
								}
								Event.current.Use();
								break;
						}
						EditorGUILayout.EndVertical();

						var newFilename = EditorGUILayout.TextField("Resource Filename", variation.resourceFileName);		
						if (newFilename != variation.resourceFileName) {
							UndoHelper.RecordObjectPropertyForUndo(variation, "change Resource Filename");
							variation.resourceFileName = newFilename;
						}
						break;
				}

				var newVolume = EditorGUILayout.Slider("Volume", variation.audio.volume, 0f, 1f);
				if (newVolume != variation.audio.volume) {
					UndoHelper.RecordObjectPropertyForUndo(variation.audio, "change Volume");
					variation.audio.volume = newVolume;
				}

				var newPitch = EditorGUILayout.Slider("Pitch", variation.audio.pitch, -3f, 3f);
				if (newPitch != variation.audio.pitch) {
					UndoHelper.RecordObjectPropertyForUndo(variation.audio, "change Pitch");
					variation.audio.pitch = newPitch;
				}

				var newLoop = EditorGUILayout.Toggle("Loop Clip", variation.audio.loop);
				if (newLoop != variation.audio.loop) {
					UndoHelper.RecordObjectPropertyForUndo(variation.audio, "toggle Loop Clip");
					variation.audio.loop = newLoop;
				}
				
				EditorUtility.SetDirty(variation.audio);

				var newRandomPitch = EditorGUILayout.Slider("Random Pitch", variation.randomPitch, 0f, 3f);
				if (newRandomPitch != variation.randomPitch) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Random Pitch");
					variation.randomPitch = newRandomPitch;
				}

				var newRandomVol = EditorGUILayout.Slider("Random Volume", variation.randomVolume, 0f, 1f);
				if (newRandomVol != variation.randomVolume) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Random Volume");
					variation.randomVolume = newRandomVol;
				}

				var newWeight = EditorGUILayout.IntSlider("Weight (Instances)", variation.weight, 0, 100);
				if (newWeight != variation.weight) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Weight");
					variation.weight = newWeight;
				}

				var newFades = EditorGUILayout.BeginToggleGroup("Use Custom Fading", variation.useFades);
				if (newFades != variation.useFades) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "toggle Use Custom Fading");
					variation.useFades = newFades;
				}

				var newFadeIn = EditorGUILayout.Slider("Fade In Time (sec)", variation.fadeInTime, 0f, 10f);
				if (newFadeIn != variation.fadeInTime) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Fade In Time");
					variation.fadeInTime = newFadeIn;
				}

				var newFadeOut = EditorGUILayout.Slider("Fade Out time (sec)", variation.fadeOutTime, 0f, 10f);
				if (newFadeOut != variation.fadeOutTime) {
					UndoHelper.RecordObjectPropertyForUndo(variation, "change Fade Out Time");
					variation.fadeOutTime = newFadeOut;
				}
				EditorGUILayout.EndToggleGroup();
				
		        EditorGUILayout.BeginHorizontal();
		
		        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(50));
		        // A little space between button groups
		        GUILayout.Space(6);
				
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(6);

				if (!Application.isPlaying) {
					variation.possibleName = EditorGUILayout.TextField(variation.possibleName, GUILayout.MinWidth(80));
					GUI.contentColor = Color.green;
					if (GUILayout.Button(new GUIContent("Rename", "Click to rename this variation"), EditorStyles.toolbarButton, GUILayout.Width(60))) {
						UndoHelper.RecordObjectPropertyForUndo(variation, "rename Variation");
						variation.name = variation.possibleName;
					}
					GUI.contentColor = Color.white;
					
					GUILayout.Space(6);
				}
				
				if (GUILayout.Button(new GUIContent(_group.settingsTexture, "Click to goto variation"), EditorStyles.toolbarButton, GUILayout.Width(40))) {
					Selection.activeObject = variation; 
				}

				GUILayout.Space(6);
				
				if (!Application.isPlaying) {
					if (GUILayout.Button(new GUIContent(_group.deleteTexture, "Click to delete this variation"), EditorStyles.toolbarButton, GUILayout.Width(40))) {
						deadChildIndex = i;
						isDirty = true;
					}
				}

				GUILayout.Space(6);
				
				var buttonPressed = GUIHelper.AddVariationButtons(ma, "Click to preview Variation");
				switch (buttonPressed) {
					case GUIHelper.DTFunctionButtons.Play:
						if (Application.isPlaying) {
						 	MasterAudio.PlaySound(_group.name, 1f, null, 0f, variation.name);
						} else {
							variation.audio.Stop();
							variation.audio.Play();
						}
						break;
				}
				
				EditorGUILayout.EndHorizontal();
				
				GUILayout.FlexibleSpace();
		
		        EditorGUILayout.EndHorizontal();
		        EditorGUILayout.EndHorizontal();
				if (!Application.isPlaying) {
					GUIHelper.ShowColorWarning("*Fading & random settings are ignored by preview in edit mode.");
				}
				
				EditorGUILayout.Separator();
			}
		}
		
		if (deadChildIndex.HasValue) {
			var deadVar = _group.groupVariations[deadChildIndex.Value];
			
			if (deadVar != null) {
				// delete variation from Hierarchy
				GameObject.DestroyImmediate(deadVar.gameObject);
			}
			
			// delete group.
			_group.groupVariations.RemoveAt(deadChildIndex.Value);
		}
		
		if (GUI.changed || isDirty) {
			EditorUtility.SetDirty(target);
		}

		GUIHelper.RepaintIfUndoOrRedo(this);

		//DrawDefaultInspector();
    }
	
	private MasterAudioGroup RescanChildren(MasterAudioGroup group) {
		var newChildren = new List<SoundGroupVariation>();
		
		var childNames = new List<string>();
		
		SoundGroupVariation variation = null;
		
		for (var i = 0; i < group.transform.childCount; i++) {
			var child = group.transform.GetChild(i);
			
			if (!Application.isPlaying) {
				if (childNames.Contains(child.name)) {
					GUIHelper.ShowColorWarning("You have more than one variation named: " + child.name + ".");
					GUIHelper.ShowColorWarning("Please ensure each variation of this Group has a unique name.");
					isValid = false;
					return null;
				}
			}
			
			childNames.Add(child.name);
			
			variation = child.GetComponent<SoundGroupVariation>();
			
			newChildren.Add(variation);
		}
		
		group.groupVariations = newChildren;
		return group;
	}
	
	public void EqualizeWeights(MasterAudioGroup _group) {
		var variations = new SoundGroupVariation[_group.groupVariations.Count];

		SoundGroupVariation variation = null;
		for (var i = 0; i < _group.groupVariations.Count; i++) {
			variation = _group.groupVariations[i];
			variations[i] = variation;
		}

		UndoHelper.RecordObjectsForUndo(variations, "Equalize Weights");

		foreach (var vari in variations) {
			vari.weight = 1;
		}
	}
	
	public void CreateVariation(MasterAudioGroup group, MasterAudio ma, AudioClip clip) {
		var clipName = clip.name;
		
		if (group.transform.FindChild(clipName) != null) {
			GUIHelper.ShowAlert("You already have a variation for this Group named '" + clipName + "'. \n\nPlease rename these variations when finished to be unique, or you may not be able to play them by name if you have a need to.");
		}
		
		var newVar = (GameObject) GameObject.Instantiate(ma.soundGroupVariationTemplate.gameObject, group.transform.position, Quaternion.identity);
		UndoHelper.CreateObjectForUndo(newVar, "create Variation");

		newVar.transform.name = clipName;
		newVar.transform.parent = group.transform;
		var variation = newVar.GetComponent<SoundGroupVariation>();
		variation.possibleName = clipName;
		
		if (group.bulkVariationMode == SoundGroupVariation.AudioLocation.ResourceFile) {
			variation.audLocation = SoundGroupVariation.AudioLocation.ResourceFile;
			variation.resourceFileName = clipName;
		} else {
			newVar.audio.clip = clip;
		}
	}
}
