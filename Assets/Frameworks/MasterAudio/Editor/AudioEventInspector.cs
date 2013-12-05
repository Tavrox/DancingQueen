using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EventSounds))]
[CanEditMultipleObjects]
public class AudioEventInspector : Editor
{
	private List<string> groupNames = null;
	private List<string> busNames = null;
	private List<string> playlistNames = null;
	private bool maInScene;
	private MasterAudio ma;
	private EventSounds sounds;

	public override void OnInspectorGUI()
	{
		EditorGUIUtility.LookLikeControls(); 
		
		ma = MasterAudio.Instance;
		if (ma != null) {
			GUIHelper.DrawTexture(ma.logoTexture);
		}
		
		sounds = (EventSounds)target;
		
		maInScene = ma != null;		
		if (maInScene) {
			groupNames = ma.GroupNames;
			busNames = ma.BusNames;
			playlistNames = ma.PlaylistNames;
		}
		
		EditorGUI.indentLevel = 1;
		GUILayout.Label("Group Controls", EditorStyles.boldLabel);

		var newSpawnMode = (MasterAudio.SoundSpawnLocationMode) EditorGUILayout.EnumPopup("Sound Spawn Mode", sounds.soundSpawnMode);
		if (newSpawnMode != sounds.soundSpawnMode) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Spawn Mode");
			sounds.soundSpawnMode = newSpawnMode;
		}

		sounds.showGizmo = EditorGUILayout.Toggle("Show 3D Gizmo", sounds.showGizmo);
		
		var newDisable = EditorGUILayout.Toggle("Disable Sounds", sounds.disableSounds);
		if (newDisable != sounds.disableSounds) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Disable Sounds");
			sounds.disableSounds = newDisable;
		}

		EditorGUILayout.Separator();
		GUILayout.Label("Sound Triggers", EditorStyles.boldLabel);

		var disabledText = "";
		if (sounds.disableSounds) {
			disabledText = " (DISABLED) ";
		}

		List<bool> changedList = new List<bool>();

		// trigger sounds

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);

		var newUseStart = EditorGUILayout.Toggle("Start" + disabledText, sounds.useStartSound);
		if (newUseStart != sounds.useStartSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Start Sound");
			sounds.useStartSound = newUseStart;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useStartSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.startSound, EventSounds.EventType.OnStart));
		}
		
		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newUseEnable = EditorGUILayout.Toggle("Enable" + disabledText, sounds.useEnableSound);
		if (newUseEnable != sounds.useEnableSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Enable Sound");
			sounds.useEnableSound = newUseEnable;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useEnableSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.enableSound, EventSounds.EventType.OnEnable));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newDisableSound = EditorGUILayout.Toggle("Disable" + disabledText, sounds.useDisableSound);
		if (newDisableSound != sounds.useDisableSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Disable Sound");
			sounds.useDisableSound = newDisableSound;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useDisableSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.disableSound, EventSounds.EventType.OnDisable));
		}


		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newVisible = EditorGUILayout.Toggle("Visible" + disabledText, sounds.useVisibleSound);
		if (newVisible != sounds.useVisibleSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Visible Sound");
			sounds.useVisibleSound = newVisible;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useVisibleSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.visibleSound, EventSounds.EventType.OnVisible));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newInvis = EditorGUILayout.Toggle("Invisible" + disabledText, sounds.useInvisibleSound);
		if (newInvis != sounds.useInvisibleSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Invisible Sound");
			sounds.useInvisibleSound = newInvis;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useInvisibleSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.invisibleSound, EventSounds.EventType.OnInvisible));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newCollision = EditorGUILayout.Toggle("Collision" + disabledText, sounds.useCollisionSound);
		if (newCollision != sounds.useCollisionSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Collision Sound");
			sounds.useCollisionSound = newCollision;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useCollisionSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.collisionSound, EventSounds.EventType.OnCollision));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newTrigger = EditorGUILayout.Toggle("Trigger Enter" + disabledText, sounds.useTriggerEnterSound);
		if (newTrigger != sounds.useTriggerEnterSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Trigger Enter Sound");
			sounds.useTriggerEnterSound = newTrigger;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useTriggerEnterSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.triggerSound, EventSounds.EventType.OnTriggerEnter));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newTriggerExit = EditorGUILayout.Toggle("Trigger Exit" + disabledText, sounds.useTriggerExitSound);
		if (newTriggerExit != sounds.useTriggerExitSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Trigger Exit Sound");
			sounds.useTriggerExitSound = newTriggerExit;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useTriggerExitSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.triggerExitSound, EventSounds.EventType.OnTriggerExit));
		}
		
		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newMouseEnter = EditorGUILayout.Toggle("Mouse Enter" + disabledText, sounds.useMouseEnterSound);
		if (newMouseEnter != sounds.useMouseEnterSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Mouse Enter Sound");
			sounds.useMouseEnterSound = newMouseEnter;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useMouseEnterSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.mouseEnterSound, EventSounds.EventType.OnMouseEnter));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newMouseClick = EditorGUILayout.Toggle("Mouse Click" + disabledText, sounds.useMouseClickSound);
		if (newMouseClick != sounds.useMouseClickSound) {
			sounds.useMouseClickSound = newMouseClick;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useMouseClickSound && !sounds.disableSounds) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Mouse Click Sound");
			changedList.Add(RenderAudioEvent(sounds.mouseClickSound, EventSounds.EventType.OnMouseClick));
		}

		EditorGUI.indentLevel = 0;

		EditorGUILayout.Separator();
		GUILayout.Label("Trigger Sounds for PoolManager Plugin", EditorStyles.boldLabel);

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newSpawned = EditorGUILayout.Toggle("Spawned" + disabledText, sounds.useSpawnedSound);
		if (newSpawned != sounds.useSpawnedSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Spawned Sound");
			sounds.useSpawnedSound = newSpawned;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useSpawnedSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.spawnedSound, EventSounds.EventType.OnSpawned));
		}

		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal(EditorStyles.objectFieldThumb);
		var newDespawned = EditorGUILayout.Toggle("Despawned" + disabledText, sounds.useDespawnedSound);
		if (newDespawned != sounds.useDespawnedSound) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Despawned Sound");
			sounds.useDespawnedSound = newDespawned;
		}
		EditorGUILayout.EndHorizontal();
		if (sounds.useDespawnedSound && !sounds.disableSounds) {
			changedList.Add(RenderAudioEvent(sounds.despawnedSound, EventSounds.EventType.OnDespawned));
		}

		if (GUI.changed || changedList.Contains(true)) {
			EditorUtility.SetDirty(target);
		}

		GUIHelper.RepaintIfUndoOrRedo(this);

		//DrawDefaultInspector();
	}

	private bool RenderAudioEvent(AudioEvent aEvent, EventSounds.EventType eType)
	{
		bool showLayerTagFilter = EventSounds.layerTagFilterEvents.Contains(eType.ToString());
		
		bool isDirty = false;

		EditorGUI.indentLevel = 0;
	
		if (eType == EventSounds.EventType.OnEnable) {
			GUIHelper.ShowColorWarning("*If this prefab is in the scene at startup, use Start event instead.");
		}

		var newSoundType = (MasterAudio.EventSoundFunctionType) EditorGUILayout.EnumPopup("Action Type", aEvent.currentSoundFunctionType);
		if (newSoundType != aEvent.currentSoundFunctionType) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "change Action Type"); 
			aEvent.currentSoundFunctionType = newSoundType;
		}
		
		switch (aEvent.currentSoundFunctionType) {
			case MasterAudio.EventSoundFunctionType.PlaySound:
				if (maInScene) {
					var existingIndex = groupNames.IndexOf(aEvent.soundType);
		
					int? groupIndex = null;

					EditorGUI.indentLevel = 1;
				
					if (existingIndex >= 1) {
						groupIndex = EditorGUILayout.Popup("Sound Group", existingIndex, groupNames.ToArray());
						if (existingIndex == 1) {
							GUIHelper.ShowColorWarning("*No Sound Group specified. Event will do nothing.");
						}
					} else if (existingIndex == -1 && aEvent.soundType == MasterAudio.NO_GROUP_NAME) {
						groupIndex = EditorGUILayout.Popup("Sound Group", existingIndex, groupNames.ToArray());
					} else { // non-match
						GUIHelper.ShowColorWarning("Sound Group found no match. Type in or choose one.");
						var newSound = EditorGUILayout.TextField("Sound Group", aEvent.soundType);
						if (newSound != aEvent.soundType) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
							aEvent.soundType = newSound;
						}

						var newIndex = EditorGUILayout.Popup("All Sound Groups", -1, groupNames.ToArray());
						if (newIndex >= 0) {
							groupIndex = newIndex;
						}
					}
					
					if (groupIndex.HasValue) {
						if (existingIndex != groupIndex.Value) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
						}
						if (groupIndex.Value == -1) {
							aEvent.soundType = MasterAudio.NO_GROUP_NAME;
						} else {
							aEvent.soundType = groupNames[groupIndex.Value];
						}
					}
				} else {
					var newSType = EditorGUILayout.TextField("Sound Group", aEvent.soundType);
					if (newSType != aEvent.soundType) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
						aEvent.soundType = newSType;
					}
				}
			
				var newVarType = (EventSounds.VariationType) EditorGUILayout.EnumPopup("Variation Mode", aEvent.variationType);
				if (newVarType != aEvent.variationType) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Variation Mode");
					aEvent.variationType = newVarType;
				}
			
				if (aEvent.variationType == EventSounds.VariationType.PlaySpecific) {
					var newVarName = EditorGUILayout.TextField("Variation Name", aEvent.variationName);
					if (newVarName != aEvent.variationName) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "change Variation Name");
						aEvent.variationName = newVarName;
					}
				
					if (string.IsNullOrEmpty(aEvent.variationName)) {
						GUIHelper.ShowColorWarning("*Variation Name is empty. No sound will play.");
					}
				}
			
				var newVol = EditorGUILayout.Slider("Volume", aEvent.volume, 0f, 1f);
				if (newVol != aEvent.volume) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Volume");
					aEvent.volume = newVol;
				}

				var newFixedPitch = EditorGUILayout.Toggle("Override pitch?", aEvent.useFixedPitch);
				if (newFixedPitch != aEvent.useFixedPitch) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Override pitch");
					aEvent.useFixedPitch = newFixedPitch;
				}
				if (aEvent.useFixedPitch) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Pitch");
					aEvent.pitch = EditorGUILayout.Slider("Pitch", aEvent.pitch, -3f, 3f);
				}
		
				var newDelay = EditorGUILayout.Slider("Delay Sound (sec)", aEvent.delaySound, 0f, 10f);
				if (newDelay != aEvent.delaySound) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Delay Sound");
					aEvent.delaySound = newDelay;
				}
				break;
			case MasterAudio.EventSoundFunctionType.PlaylistControl:
				var newPlaylistCmd = (MasterAudio.PlaylistCommand) EditorGUILayout.EnumPopup("Playlist Command", aEvent.currentPlaylistCommand);
				if (newPlaylistCmd != aEvent.currentPlaylistCommand) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Playlist Command");
					aEvent.currentPlaylistCommand = newPlaylistCmd;
				}
			
				EditorGUI.indentLevel = 1;
			
				switch (aEvent.currentPlaylistCommand) {
					case MasterAudio.PlaylistCommand.ChangePlaylist:
				
						if (maInScene) {
							var existingIndex = playlistNames.IndexOf(aEvent.playlistName);
				
							int? playlistIndex = null;
							
							if (existingIndex >= 1) {
								playlistIndex = EditorGUILayout.Popup("Playlist Name", existingIndex, playlistNames.ToArray());
								if (existingIndex == 1) {
									GUIHelper.ShowColorWarning("*No Playlist Name specified. Event will do nothing.");
								}
							} else if (existingIndex == -1 && aEvent.playlistName == MasterAudio.NO_GROUP_NAME) {
								playlistIndex = EditorGUILayout.Popup("Playlist Name", existingIndex, playlistNames.ToArray());
							} else { // non-match
								GUIHelper.ShowColorWarning("Playlist Name found no match. Type in or choose one.");

								var newPlaylist = EditorGUILayout.TextField("Playlist Name", aEvent.playlistName);
								if (newPlaylist != aEvent.playlistName) {
									UndoHelper.RecordObjectPropertyForUndo(sounds, "change Playlist Name");
									aEvent.playlistName = newPlaylist;
								}
								var newIndex = EditorGUILayout.Popup("All Playlists", -1, playlistNames.ToArray());
								if (newIndex >= 0) {
									playlistIndex = newIndex;
								}
							}
							
							if (playlistIndex.HasValue) {
								if (existingIndex != playlistIndex.Value) {
									UndoHelper.RecordObjectPropertyForUndo(sounds, "change Playlist Name");
								}
								if (playlistIndex.Value == -1) {
									aEvent.playlistName = MasterAudio.NO_GROUP_NAME;
								} else {
									aEvent.playlistName = playlistNames[playlistIndex.Value];
								}
							}
						} else {
							var newPlaylistName = EditorGUILayout.TextField("Playlist Name", aEvent.playlistName);
							if (newPlaylistName != aEvent.playlistName) {
								UndoHelper.RecordObjectPropertyForUndo(sounds, "change Playlist Name");
								aEvent.playlistName = newPlaylistName;
							}
						}
						
						var newStartPlaylist = EditorGUILayout.Toggle("Start Playlist?", aEvent.startPlaylist);
						if (newStartPlaylist != aEvent.startPlaylist) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Start Playlist");
							aEvent.startPlaylist = newStartPlaylist;
						}
						break;
					case MasterAudio.PlaylistCommand.FadeToVolume:
						var newFadeVol = EditorGUILayout.Slider("Target Volume", aEvent.fadeVolume, 0f, 1f);
						if (newFadeVol != aEvent.fadeVolume) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Target Volume");
							aEvent.fadeVolume = newFadeVol;
						}

						var newFadeTime = EditorGUILayout.Slider("Fade Time", aEvent.fadeTime, 0f, 10f);
						if (newFadeTime != aEvent.fadeTime) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Fade Time");
							aEvent.fadeTime = newFadeTime;
						}
						break;
					case MasterAudio.PlaylistCommand.PlayClip:
						var newClip = EditorGUILayout.TextField("Clip Name", aEvent.clipName);
						if (newClip != aEvent.clipName) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Clip Name");
							aEvent.clipName = newClip;
						}
						if (string.IsNullOrEmpty(aEvent.clipName)) {
							GUIHelper.ShowColorWarning("*Clip name is empty. Event will do nothing.");
						}
						break;
					case MasterAudio.PlaylistCommand.NextPlaylist:
						var newStartPl = EditorGUILayout.Toggle("Start Playlist?", aEvent.startPlaylist);
						if (newStartPl != aEvent.startPlaylist) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Start Playlist");
							aEvent.startPlaylist = newStartPl;
						}
						break;
					case MasterAudio.PlaylistCommand.PreviousPlaylist:
						var newStartPlayl = EditorGUILayout.Toggle("Start Playlist?", aEvent.startPlaylist);
						if (newStartPlayl != aEvent.startPlaylist) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Start Playlist");
							aEvent.startPlaylist = newStartPlayl;
						}
						break;
				}
				break;
			case MasterAudio.EventSoundFunctionType.GroupControl:
				EditorGUI.indentLevel = 1;

				var newGroupCmd = (MasterAudio.SoundGroupCommand) EditorGUILayout.EnumPopup("Group Command", aEvent.currentSoundGroupCommand);
				if (newGroupCmd != aEvent.currentSoundGroupCommand) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Group Command");
					aEvent.currentSoundGroupCommand = newGroupCmd;
				}
			
				if (aEvent.currentSoundGroupCommand != MasterAudio.SoundGroupCommand.None) {
					if (maInScene) {
						var existingIndex = groupNames.IndexOf(aEvent.soundType);
			
						int? groupIndex = null;
						
						if (existingIndex >= 1) {
							groupIndex = EditorGUILayout.Popup("Sound Group", existingIndex, groupNames.ToArray());
						} else if (existingIndex == -1 && aEvent.soundType == MasterAudio.NO_GROUP_NAME) {
							groupIndex = EditorGUILayout.Popup("Sound Group", existingIndex, groupNames.ToArray());
						} else { // non-match
							GUIHelper.ShowColorWarning("Sound Group found no match. Type in or choose one.");
							
							var newSType = EditorGUILayout.TextField("Sound Group", aEvent.soundType);
							if (newSType != aEvent.soundType) {
								UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
								aEvent.soundType = newSType;
							}
							var newIndex = EditorGUILayout.Popup("All Sound Groups", -1, groupNames.ToArray());
							if (newIndex >= 0) {
								groupIndex = newIndex;
							}
						}
						
						if (groupIndex.HasValue) {
							if (existingIndex != groupIndex.Value) {
								UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
							}
							if (groupIndex.Value == -1) {
								aEvent.soundType = MasterAudio.NO_GROUP_NAME;
							} else {
								aEvent.soundType = groupNames[groupIndex.Value];
							}
						}
					} else {
						var newSoundT = EditorGUILayout.TextField("Sound Group", aEvent.soundType);
						if (newSoundT != aEvent.soundType) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Sound Group");
							aEvent.soundType = newSoundT;
						}
					}
				}

				switch (aEvent.currentSoundGroupCommand) {
					case MasterAudio.SoundGroupCommand.None:	
						break;
					case MasterAudio.SoundGroupCommand.FadeToVolume:
						var newFadeVol = EditorGUILayout.Slider("Target Volume", aEvent.fadeVolume, 0f, 1f);
						if (newFadeVol != aEvent.fadeVolume) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Target Volume");
							aEvent.fadeVolume = newFadeVol;
						}

						var newFadeTime = EditorGUILayout.Slider("Fade Time", aEvent.fadeTime, 0f, 10f);
						if (newFadeTime != aEvent.fadeTime) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Fade Time");
							aEvent.fadeTime = newFadeTime;
						}
						break;
					case MasterAudio.SoundGroupCommand.FadeOutAllOfSound:
						var newFadeT = EditorGUILayout.Slider("Fade Time", aEvent.fadeTime, 0f, 10f);
						if (newFadeT != aEvent.fadeTime) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Fade Time");
							aEvent.fadeTime = newFadeT;
						}
						break;
					case MasterAudio.SoundGroupCommand.Mute:	
						break;
					case MasterAudio.SoundGroupCommand.Pause:	
						break;
					case MasterAudio.SoundGroupCommand.Solo:	
						break;
					case MasterAudio.SoundGroupCommand.Unmute:	
						break;
					case MasterAudio.SoundGroupCommand.Unpause:	
						break;
					case MasterAudio.SoundGroupCommand.Unsolo:	
						break;
				}
				
				break;	
			case MasterAudio.EventSoundFunctionType.BusControl:
				var newBusCmd = (MasterAudio.BusCommand) EditorGUILayout.EnumPopup("Bus Command", aEvent.currentBusCommand);
				if (newBusCmd != aEvent.currentBusCommand) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "change Bus Command");
					aEvent.currentBusCommand = newBusCmd;
				}

				EditorGUI.indentLevel = 1;
			
				if (aEvent.currentBusCommand != MasterAudio.BusCommand.None) {
					if (maInScene) {
						var existingIndex = busNames.IndexOf(aEvent.busName);
			
						int? busIndex = null;
						
						if (existingIndex >= 1) {
							busIndex = EditorGUILayout.Popup("Bus Name", existingIndex, busNames.ToArray());
							if (existingIndex == 1) {
								GUIHelper.ShowColorWarning("*No Bus Name specified. Event will do nothing.");
							}
						} else if (existingIndex == -1 && aEvent.busName == MasterAudio.NO_GROUP_NAME) {
							busIndex = EditorGUILayout.Popup("Bus Name", existingIndex, busNames.ToArray());
						} else { // non-match
							var newBusName = EditorGUILayout.TextField("Bus Name", aEvent.busName);
							if (newBusName != aEvent.busName) {
								UndoHelper.RecordObjectPropertyForUndo(sounds, "change Bus Name");
								aEvent.busName = newBusName;
							}

							var newIndex = EditorGUILayout.Popup("All Buses", -1, busNames.ToArray());
							if (newIndex >= 0) {
								busIndex = newIndex;
							}
							GUIHelper.ShowColorWarning("*Bus Name found no match. Type in or choose one.");
						}
						
						if (busIndex.HasValue) {
							if (existingIndex != busIndex.Value) {
								UndoHelper.RecordObjectPropertyForUndo(sounds, "change Bus");
							}
							if (busIndex.Value == -1) {
								aEvent.busName = MasterAudio.NO_GROUP_NAME;
							} else {
								aEvent.busName = busNames[busIndex.Value];
							}
						}
					} else {
						var newBusName = EditorGUILayout.TextField("Bus Name", aEvent.busName);
						if (newBusName != aEvent.busName) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Bus Name");
							aEvent.busName = newBusName;
						}
					}
			
					var newVolume = EditorGUILayout.Slider("Volume", aEvent.volume, 0f, 1f);
					if (newVolume != aEvent.volume) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "change Volume");
						aEvent.volume = newVolume;
					}

					var newFixPitch = EditorGUILayout.Toggle("Override pitch?", aEvent.useFixedPitch);
					if (newFixPitch != aEvent.useFixedPitch) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Override pitch");
						aEvent.useFixedPitch = newFixPitch;
					}
					if (aEvent.useFixedPitch) {
						GUIHelper.ShowColorWarning("*Random pitches for the variation will not be used.");
						var newPitch = EditorGUILayout.Slider("Pitch", aEvent.pitch, -3f, 3f);
						if (newPitch != aEvent.pitch) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Pitch");
							aEvent.pitch = newPitch;
						}
					}
				}	
			
				switch (aEvent.currentBusCommand) {
					case MasterAudio.BusCommand.FadeToVolume:
						var newFadeVol = EditorGUILayout.Slider("Target Volume", aEvent.fadeVolume, 0f, 1f);
						if (newFadeVol != aEvent.fadeVolume) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Target Volume");
							aEvent.fadeVolume = newFadeVol;
						}

						var newFadeTime = EditorGUILayout.Slider("Fade Time", aEvent.fadeTime, 0f, 10f);
						if (newFadeTime != aEvent.fadeTime) {
							UndoHelper.RecordObjectPropertyForUndo(sounds, "change Fade Time");
							aEvent.fadeTime = newFadeTime;
						}
						break;
					case MasterAudio.BusCommand.Pause:
						break;
					case MasterAudio.BusCommand.Unpause:
						break;
				}
				
				break;
		}
		
		EditorGUI.indentLevel = 0;

		var newEmit = EditorGUILayout.Toggle("Emit Particle", aEvent.emitParticles);
		if (newEmit != aEvent.emitParticles) {
			UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Emit Particle");
			aEvent.emitParticles = newEmit;
		}
		if (aEvent.emitParticles) {
			var newParticleCount = EditorGUILayout.IntSlider("Particle Count", aEvent.particleCountToEmit, 1, 100);
			if (newParticleCount != aEvent.particleCountToEmit) {
				UndoHelper.RecordObjectPropertyForUndo(sounds, "change Particle Count");
				aEvent.particleCountToEmit = newParticleCount;
			}
		}

		if (showLayerTagFilter) {
			var newUseLayers = EditorGUILayout.BeginToggleGroup("Layer filters", aEvent.useLayerFilter);
			if (newUseLayers != aEvent.useLayerFilter) {
				UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Layer filters");
				aEvent.useLayerFilter = newUseLayers;
			}
			if (aEvent.useLayerFilter) {
				for (var i = 0; i < aEvent.matchingLayers.Count; i++) {
					var newLayer = EditorGUILayout.LayerField("Layer Match " + (i + 1), aEvent.matchingLayers[i]);
					if (newLayer != aEvent.matchingLayers[i]) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "change Layer filter");
						aEvent.matchingLayers[i] = newLayer;
					}
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(24);

				if (GUILayout.Button(new GUIContent("Add", "Click to add a layer match at the end"), GUILayout.Width(60))) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "add Layer filter");
					aEvent.matchingLayers.Add(0);
					isDirty = true;
				}
				if (aEvent.matchingLayers.Count > 1) {
					if (GUILayout.Button(new GUIContent("Remove", "Click to remove the last layer match"), GUILayout.Width(60))) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "remove Layer filter");
						aEvent.matchingLayers.RemoveAt(aEvent.matchingLayers.Count - 1);
						isDirty = true;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndToggleGroup();

			var newTagFilter = EditorGUILayout.BeginToggleGroup("Tag filter", aEvent.useTagFilter);
			if (newTagFilter != aEvent.useTagFilter) {
				UndoHelper.RecordObjectPropertyForUndo(sounds, "toggle Tag filter");
				aEvent.useTagFilter = newTagFilter;
			}

			if (aEvent.useTagFilter) {
				for (var i = 0; i < aEvent.matchingTags.Count; i++) {
					var newTag = EditorGUILayout.TagField("Tag Match " + (i + 1), aEvent.matchingTags[i]);
					if (newTag != aEvent.matchingTags[i]) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "change Tag filter");
						aEvent.matchingTags[i] = newTag;
					}
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Space(24);
				if (GUILayout.Button(new GUIContent("Add", "Click to add a tag match at the end"), GUILayout.Width(60))) {
					UndoHelper.RecordObjectPropertyForUndo(sounds, "Add Tag filter");
					aEvent.matchingTags.Add("Untagged");
					isDirty = true;
				}
				if (aEvent.matchingTags.Count > 1) {
					if (GUILayout.Button(new GUIContent("Remove", "Click to remove the last tag match"), GUILayout.Width(60))) {
						UndoHelper.RecordObjectPropertyForUndo(sounds, "remove Tag filter");
						aEvent.matchingTags.RemoveAt(aEvent.matchingLayers.Count - 1);
						isDirty = true;
					}
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndToggleGroup();
		}

		return isDirty;
	}
}
