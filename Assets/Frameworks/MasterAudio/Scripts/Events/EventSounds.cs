using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dark Tonic/Master Audio/EventSounds")]
public class EventSounds : MonoBehaviour {
	public bool showGizmo = true;
	public MasterAudio.SoundSpawnLocationMode soundSpawnMode = MasterAudio.SoundSpawnLocationMode.CallerLocation;
	public bool disableSounds = false;

	public enum EventType {
		OnStart,
		OnVisible,
		OnInvisible,
		OnCollision,
		OnTriggerEnter,
		OnTriggerExit,
		OnMouseEnter,
		OnMouseClick,
		OnSpawned,
		OnDespawned,
		OnEnable,
		OnDisable
	}
	
	public enum VariationType {
		PlaySpecific,
		PlayRandom
	}
	
	public static List<string> layerTagFilterEvents = new List<string>() {
		EventType.OnCollision.ToString(),
		EventType.OnTriggerEnter.ToString(),
		EventType.OnTriggerExit.ToString()		
	};
	
	public AudioEvent startSound;
	public AudioEvent visibleSound;
	public AudioEvent invisibleSound;
	public AudioEvent collisionSound;
	public AudioEvent triggerSound;
	public AudioEvent triggerExitSound;
	public AudioEvent mouseEnterSound;
	public AudioEvent mouseClickSound;
	public AudioEvent spawnedSound;
	public AudioEvent despawnedSound;
	public AudioEvent enableSound;
	public AudioEvent disableSound;
	
	public bool useStartSound = false;
	public bool useVisibleSound = false;
	public bool useInvisibleSound = false;
	public bool useCollisionSound = false;
	public bool useTriggerEnterSound = false;
	public bool useTriggerExitSound = false;
	public bool useMouseEnterSound = false;
	public bool useMouseClickSound = false;
	public bool useSpawnedSound = false;
	public bool useDespawnedSound = false;
	public bool useEnableSound = false;
	public bool useDisableSound = false;
	
	private bool isVisible;
	
	private Transform trans;
	
	void Awake() {
		this.trans = this.transform;
		this.SpawnedOrAwake();
	}
	
	protected virtual void SpawnedOrAwake() {
		this.isVisible = false;
	}
	
	void Start() {
		if (this.useStartSound) {
			PlaySound(startSound, EventType.OnStart);
		}
	}
	
	void OnBecameVisible() {
		if (this.useVisibleSound && !this.isVisible) {
			this.isVisible = true;
			PlaySound(visibleSound, EventType.OnVisible);
		}
	}
	
	void OnBecameInvisible() {
		if (this.useInvisibleSound) {
			this.isVisible = false;
			PlaySound(invisibleSound, EventType.OnInvisible);
		}
	}
	
	void OnEnable() {
		if (this.useEnableSound) {
			PlaySound(enableSound, EventType.OnEnable);
		}
	}
	
	void OnDisable() {
		if (!this.useDisableSound || MasterAudio.AppIsShuttingDown) {
			return;
		}
		
		PlaySound(this.disableSound, EventType.OnDisable);
	}
	
	void OnCollisionEnter(Collision collision) {
		if (!this.useCollisionSound) {
			return;
		}
		
		// check filters for matches if turned on
		if (collisionSound.useLayerFilter && !collisionSound.matchingLayers.Contains(collision.gameObject.layer)) {
			return;
		}
		
		if (collisionSound.useTagFilter && !collisionSound.matchingTags.Contains(collision.gameObject.tag)) {
			return;
		}
		
		PlaySound(collisionSound, EventType.OnCollision);
	}
	
	void OnTriggerEnter(Collider other) {
		if (!this.useTriggerEnterSound) {
			return;
		}
		
		// check filters for matches if turned on
		if (triggerSound.useLayerFilter && !triggerSound.matchingLayers.Contains(other.gameObject.layer)) {
			return;
		}
		
		if (triggerSound.useTagFilter && !triggerSound.matchingTags.Contains(other.gameObject.tag)) {
			return;
		}
		
		PlaySound(triggerSound, EventType.OnTriggerEnter);
	}

	void OnTriggerExit(Collider other) {
		if (!this.useTriggerExitSound) {
			return;
		}
		
		// check filters for matches if turned on
		if (triggerExitSound.useLayerFilter && !triggerExitSound.matchingLayers.Contains(other.gameObject.layer)) {
			return;
		}
		
		if (triggerExitSound.useTagFilter && !triggerExitSound.matchingTags.Contains(other.gameObject.tag)) {
			return;
		}
		
		PlaySound(triggerExitSound, EventType.OnTriggerExit);
	}
	
	void OnMouseEnter() {
		if (this.useMouseEnterSound) {
			PlaySound(mouseEnterSound, EventType.OnMouseEnter);
		}
	}
	
	void OnMouseDown() {
		if (this.useMouseClickSound) {
			PlaySound(mouseClickSound, EventType.OnMouseClick);
		}
	}	
	
	void OnSpawned() {
		this.SpawnedOrAwake();

		if (this.useSpawnedSound) {
			PlaySound(spawnedSound, EventType.OnSpawned);
		}
	}

	void OnDespawned() {
		if (this.useDespawnedSound) {
			PlaySound(despawnedSound, EventType.OnDespawned);
		}
	}

	void OnDrawGizmos() {
		if (showGizmo) {
			Gizmos.DrawIcon(this.transform.position, MasterAudio.GIZMO_FILE_NAME, true);
		}
	}

	private IEnumerator TryPlayStartSound(AudioEvent aEvent) {
		for (var i = 0; i < 3; i++) {
			yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
	
			var result = PlaySound(aEvent, EventType.OnStart, false);
			if (result != null && result.SoundPlayed) {
				break;
			}
		}
	}

	private PlaySoundResult PlaySound(AudioEvent aEvent, EventType eType, bool isFirstTry = true) {
		if (disableSounds || MasterAudio.AppIsShuttingDown) {
			return null;
		}
		
		#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
		aEvent.delaySound = 0f;
		#endif
		
		float volume = aEvent.volume;
		string sType = aEvent.soundType;
		float? pitch = aEvent.pitch;
		if (!aEvent.useFixedPitch) {
			pitch = null;
		}
		
		PlaySoundResult soundPlayed = null;
		
		var soundSpawnModeToUse = soundSpawnMode;
		
	 	if (aEvent == disableSound || aEvent == despawnedSound) {
			soundSpawnModeToUse = MasterAudio.SoundSpawnLocationMode.CallerLocation;
		}
		
		switch (aEvent.currentSoundFunctionType) {
			case MasterAudio.EventSoundFunctionType.PlaySound:
		    	string variationName = null;
				if (aEvent.variationType == VariationType.PlaySpecific) {
					variationName = aEvent.variationName;
				}
			
				if (eType == EventType.OnStart && isFirstTry && !MasterAudio.SoundGroupExists(sType)) {
		           // don't try to play sound yet.
		        } else {
		            switch (soundSpawnModeToUse)
		            {
		                case MasterAudio.SoundSpawnLocationMode.CallerLocation:
		                    soundPlayed = MasterAudio.PlaySound3D(sType, this.trans, false, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		                case MasterAudio.SoundSpawnLocationMode.AttachToCaller:
		                    soundPlayed = MasterAudio.PlaySound3D(sType, this.trans, true, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		                case MasterAudio.SoundSpawnLocationMode.MasterAudioLocation:
		                    soundPlayed = MasterAudio.PlaySound(sType, volume, pitch, aEvent.delaySound, variationName);
		                    break;
		            }
		        }
				
				if (soundPlayed == null || !soundPlayed.SoundPlayed) {
		            if (eType == EventType.OnStart && isFirstTry) {
						// race condition met. So try to play it a few more times.
		                StartCoroutine(TryPlayStartSound(aEvent));
					}
					return soundPlayed;
				}
				break;
			case MasterAudio.EventSoundFunctionType.PlaylistControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
			
				switch (aEvent.currentPlaylistCommand) {
					case MasterAudio.PlaylistCommand.None:
						soundPlayed.SoundPlayed = false;
						break;
					case MasterAudio.PlaylistCommand.ChangePlaylist:
						if (string.IsNullOrEmpty(aEvent.playlistName)) {
							Debug.Log("You have not specified a Playlist name for Event Sounds on '" + this.trans.name + "'.");
							soundPlayed.SoundPlayed = false;
						} else {				
							MasterAudio.ChangePlaylistByName(aEvent.playlistName, aEvent.startPlaylist);
						}
				
						break;				
					case MasterAudio.PlaylistCommand.FadeToVolume:
						MasterAudio.FadePlaylistToVolume(aEvent.fadeVolume, aEvent.fadeTime);
						break;				
					case MasterAudio.PlaylistCommand.NextPlaylist:
						MasterAudio.NextPlaylist(aEvent.startPlaylist);
						break;				
					case MasterAudio.PlaylistCommand.PreviousPlaylist:
						MasterAudio.PreviousPlaylist(aEvent.startPlaylist);
						break;				
					case MasterAudio.PlaylistCommand.PlayClip:
						if (string.IsNullOrEmpty(aEvent.clipName)) {
							Debug.Log("You have not specified a clip name for Event Sounds on '" + this.trans.name + "'.");
							soundPlayed.SoundPlayed = false;
						} else {
							MasterAudio.TriggerPlaylistClip(aEvent.clipName);
						}
				
						break;		
					case MasterAudio.PlaylistCommand.PlayRandomSong:
						MasterAudio.TriggerRandomPlaylistClip();
						break;				
					case MasterAudio.PlaylistCommand.PlayNextSong:
						MasterAudio.TriggerNextPlaylistClip();
						break;				
					case MasterAudio.PlaylistCommand.Pause:
						MasterAudio.PausePlaylist();
						break;				
					case MasterAudio.PlaylistCommand.Stop:
						MasterAudio.StopPlaylist();
						break;				
					case MasterAudio.PlaylistCommand.Resume:
						MasterAudio.ResumePlaylist();
						break;				
				}
				break;
			case MasterAudio.EventSoundFunctionType.GroupControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
			
				switch (aEvent.currentSoundGroupCommand) {
					case MasterAudio.SoundGroupCommand.None:
						soundPlayed.SoundPlayed = false;
						break;
					case MasterAudio.SoundGroupCommand.FadeToVolume:
						MasterAudio.FadeSoundGroupToVolume(aEvent.soundType, aEvent.fadeVolume, aEvent.fadeTime);
						break;
					case MasterAudio.SoundGroupCommand.FadeOutAllOfSound:
						MasterAudio.FadeOutAllOfSound(aEvent.soundType, aEvent.fadeTime);
						break;
					case MasterAudio.SoundGroupCommand.Mute:
						MasterAudio.MuteGroup(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.Pause:
						MasterAudio.PauseSoundGroup(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.Solo:
						MasterAudio.SoloGroup(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.StopAllOfSound:
						MasterAudio.StopAllOfSound(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.Unmute:
						MasterAudio.UnmuteGroup(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.Unpause:
						MasterAudio.UnpauseSoundGroup(aEvent.soundType);
						break;
					case MasterAudio.SoundGroupCommand.Unsolo:
						MasterAudio.UnsoloGroup(aEvent.soundType);
						break;
				}	

				break;
			case MasterAudio.EventSoundFunctionType.BusControl:
				soundPlayed = new PlaySoundResult() {	
					ActingVariation = null,
					SoundPlayed = true,
					SoundScheduled = false
				};
			
				switch (aEvent.currentBusCommand) {
					case MasterAudio.BusCommand.None:
						soundPlayed.SoundPlayed = false;
						break;
					case MasterAudio.BusCommand.FadeToVolume:
						MasterAudio.FadeBusToVolume(aEvent.busName, aEvent.fadeVolume, aEvent.fadeTime);
						break;
					case MasterAudio.BusCommand.Pause:
						MasterAudio.PauseBus(aEvent.busName);
						break;
					case MasterAudio.BusCommand.Unpause:
						MasterAudio.UnpauseBus(aEvent.busName);
						break;
					case MasterAudio.BusCommand.Mute:
						MasterAudio.MuteBus(aEvent.busName);
						break;
					case MasterAudio.BusCommand.Unmute:
						MasterAudio.UnmuteBus(aEvent.busName);
						break;
					case MasterAudio.BusCommand.Solo:
						MasterAudio.SoloBus(aEvent.busName);
						break;
					case MasterAudio.BusCommand.Unsolo:
						MasterAudio.UnsoloBus(aEvent.busName);
						break;
				}	

				break;
		}
		
		if (aEvent.emitParticles && soundPlayed != null && (soundPlayed.SoundPlayed || soundPlayed.SoundScheduled)) {
			MasterAudio.TriggerParticleEmission(this.trans, aEvent.particleCountToEmit);
		}
		
		return soundPlayed;
	}
}
