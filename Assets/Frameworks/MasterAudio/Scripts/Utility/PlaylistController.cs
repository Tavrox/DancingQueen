using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PlaylistController : MonoBehaviour {
	private AudioSource activeAudio;
	private AudioSource transitioningAudio;
	private float activeAudioEndVolume;
	private float transitioningAudioStartVolume;
	private bool isCrossFading = false;
	private float crossFadeStartTime;
	private bool isShuffle = false;
	private bool isAutoAdvance = false;
	private bool repeatPlaylist = false;
	private List<int> clipsRemaining = new List<int>();
	private int currentSequentialClipIndex;
	private AudioDuckingMode duckingMode;
	private float timeToStartUnducking;
	private float timeToFinishUnducking;
	private float originalMusicVolume;
	private float initialDuckVolume;
	private float duckRange;
	private MusicSetting currentSong;
	private GameObject go;
	private bool noNextSong = false;
	private FadeMode curFadeMode = FadeMode.None;
	private float slowFadeTargetVolume;
	private float slowFadeVolStep;
	
	private static PlaylistController _instance;
	
	public enum FadeMode {
		None,
		GradualFade
	}
	
	public enum AudioDuckingMode {
		NotDucking,
		SetToDuck,
		Ducked
	}

	#region Monobehavior events
	void Awake() {
		duckingMode = AudioDuckingMode.NotDucking;
		this.useGUILayout = false;
		currentSong = null;

        var audios = this.GetComponents<AudioSource>();
        if (audios.Length < 2)
        {
            Debug.LogError("This prefab should have exactly two Audio Source components. Please revert it.");
            return;
        }

        AudioSource audio1 = audios[0];
        AudioSource audio2 = audios[1];

        audio1.clip = null;
        audio2.clip = null;

        activeAudio = audio1;
        transitioningAudio = audio2;
		go = this.gameObject;
		curFadeMode = FadeMode.None;
	}
	
	// Use this for initialization 
	void Start () {
        // fill up randomizer
		InitializePlaylist();
		
		var pControl = MasterAudio.PlaylistControl;
		
		if (pControl == null) {
			return;
		}
		
		isShuffle = MasterAudio.PlaylistControl._shuffle;
		isAutoAdvance = MasterAudio.PlaylistControl._autoAdvance;
		repeatPlaylist = MasterAudio.PlaylistControl._repeatPlaylist;
		 
		if (MasterAudio.CurrentPlaylistSettings.Count > 0 && MasterAudio.PlaylistControl._startPlaylistOnAwake) {
			PlayNextOrRandom();
		}
		
		StartCoroutine(this.CoStart());
	}
	
	IEnumerator CoStart() {
    	while (true) {
        	yield return StartCoroutine(this.CoUpdate());
		}
	}
	
	IEnumerator CoUpdate() {
		yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
		
		// gradual fade code
		if (curFadeMode != FadeMode.GradualFade) {
			yield break;
		}

		if (activeAudio == null) {
			yield break; // paused or error in setup
		}

		var newVolume = MasterAudio.PlaylistMasterVolume + slowFadeVolStep;
		
		if (slowFadeVolStep > 0f) {
			newVolume = Math.Min(newVolume, slowFadeTargetVolume);	
		} else {
			newVolume = Math.Max(newVolume, slowFadeTargetVolume);	
		}
			
		MasterAudio.PlaylistMasterVolume = newVolume;
		
		if (newVolume == slowFadeTargetVolume) {
			curFadeMode = FadeMode.None;
		}
	}
	
	void FixedUpdate() {
		if (isCrossFading) {
			// cross-fade code
			if (activeAudio.volume >= activeAudioEndVolume) {
				CeaseAudioSource(transitioningAudio);
				isCrossFading = false;
				SetDuckProperties(); // they now should read from a new audio source
			}
			
			var ratioPassed = (Time.time - crossFadeStartTime) / MasterAudio.Instance.CrossFadeTime;
			
			activeAudio.volume = ratioPassed * activeAudioEndVolume;
			transitioningAudio.volume = transitioningAudioStartVolume * (1 - ratioPassed);
	
			// end cross-fading code
		} 
		
		if (isAutoAdvance && !activeAudio.loop && activeAudio.clip != null && !noNextSong) {
			var currentClipTime = activeAudio.clip.length - activeAudio.time - (MasterAudio.Instance.CrossFadeTime / activeAudio.pitch);
			var clipFadeStartTime = Time.deltaTime * EventCalcSounds.FRAMES_EARLY_TO_TRIGGER * activeAudio.pitch;
			if (currentClipTime < clipFadeStartTime) {
				PlayNextOrRandom();
			}
		}
		
		if (isCrossFading) {
			return;
		}
		
		this.AudioDucking();
	}
	#endregion
	
	#region public methods
	public static PlaylistController Instance {
		get {
			if (_instance == null) {
				_instance = (PlaylistController) GameObject.FindObjectOfType(typeof(PlaylistController));
			}
			
			return _instance;
		}
	}
	
    /// <summary>
    /// This method is used by Master Audio to update conditions based on the Ducked Volume Multiplier changing.
    /// </summary>
	public void UpdateDuckedVolumeMultiplier() {
		if (Application.isPlaying) {
			SetDuckProperties();
		}
	}
	
    /// <summary>
    /// This method will pause the Playlist.
    /// </summary>
	public void PausePlaylist() {
		if (activeAudio == null || transitioningAudio == null) {
			return;
		}
		
		activeAudio.Pause();
		transitioningAudio.Pause();
	}
	
    /// <summary>
    /// This method will unpause the Playlist.
    /// </summary>
	public void ResumePlaylist() {
		if (activeAudio == null || transitioningAudio == null) {
			return;
		}
		
		activeAudio.Play();
		transitioningAudio.Play();
	}
	
	public void InitializePlaylist() {
		FillClips();
		currentSequentialClipIndex = 0;
	}
	
    /// <summary>
    /// This method will Stop the Playlist.
    /// </summary>
	public void StopPlaylist() {
		PlayPlaylistSong(new MusicSetting());
	}
	
    /// <summary>
    /// This method allows you to fade the Playlist to a specified volume over X seconds.
    /// </summary>
    /// <param name="targetVolume">The volume to fade to.</param>
    /// <param name="fadeTime">The amount of time to fully fade to the target volume.</param>
	public void FadeToVolume(float targetVolume, float fadeTime) {
		if (fadeTime <= MasterAudio.INNER_LOOP_CHECK_INTERVAL) {
			MasterAudio.PlaylistMasterVolume = targetVolume;
			return;			
		}
		
		curFadeMode = FadeMode.GradualFade;
		slowFadeTargetVolume = targetVolume;
        slowFadeVolStep = (slowFadeTargetVolume - MasterAudio.PlaylistMasterVolume) / (fadeTime / MasterAudio.INNER_LOOP_CHECK_INTERVAL);
	}
	
    /// <summary>
    /// This method will play a random song in the current Playlist.
    /// </summary>
	public void PlayRandomSong() {
		if (clipsRemaining.Count == 0) {
			Debug.LogWarning("There are no playlist clips to play.");
			return;
		}
				
		var randIndex = UnityEngine.Random.Range(0, clipsRemaining.Count);
		var clipIndex = clipsRemaining[randIndex];
	
		PlayPlaylistSong(MasterAudio.CurrentPlaylistSettings[clipIndex]);

		clipsRemaining.RemoveAt(randIndex);
		if (clipsRemaining.Count == 0) {
			FillClips();
		}
	}

    /// <summary>
    /// This method will play the next song in the current Playlist.
    /// </summary>
	public void PlayNextSong() {
		if (currentSequentialClipIndex >= MasterAudio.CurrentPlaylistSettings.Count) {
			Debug.LogWarning("Master Audio has run out of songs to play. You can check the 'Repeat Playlist' checkbox to start over at song 1.");
			noNextSong = true;
			return;
		}
		
		PlayPlaylistSong(MasterAudio.CurrentPlaylistSettings[currentSequentialClipIndex]);
		currentSequentialClipIndex++;
		
		if (repeatPlaylist && currentSequentialClipIndex >= MasterAudio.CurrentPlaylistSettings.Count) {
			currentSequentialClipIndex = 0;
		}
	}
	
	public void TriggerPlaylistClip(MusicSetting setting) {
		PlayPlaylistSong(setting);
	}
	
	public void DuckMusicForTime(float duckLength, float pitch, float duckedTimePercentage) {
		if (isCrossFading) {
			return; // no ducking during cross-fading, it screws up calculations.
		}
		
		var rangedDuck = duckLength / pitch;
		
		duckingMode = AudioDuckingMode.SetToDuck;
		timeToStartUnducking = Time.time + (rangedDuck * duckedTimePercentage);
		timeToFinishUnducking = Math.Max(Time.time + rangedDuck, timeToStartUnducking);
	}

    /// <summary>
    /// This method is used to update state based on the Playlist Master Volume.
    /// </summary>
	public void UpdateMasterVolume() {
		if (!Application.isPlaying) {
			return;
		}

		if (activeAudio != null && currentSong != null && !IsCrossFading) {
			activeAudio.volume = currentSong.volume * MasterAudio.PlaylistMasterVolume;
		}
		
		if (currentSong != null) {
            activeAudioEndVolume = currentSong.volume * MasterAudio.PlaylistMasterVolume;
		}
		
		SetDuckProperties();
	}
	
	#endregion
	
	#region Helper methods
	private void PlayNextOrRandom() {
		if (!isShuffle) {
			PlayNextSong();
		} else {
			PlayRandomSong();
		}
	}
	
	private void FillClips() {
		clipsRemaining.Clear();
		var playlistSettings = MasterAudio.CurrentPlaylistSettings;
		
		if (playlistSettings == null) {
			return; // No MA in scene.
		}
		
		for (var i = 0; i < MasterAudio.CurrentPlaylistSettings.Count; i++) {
			var clip = MasterAudio.CurrentPlaylistSettings[i].clip;
			
			if (clip == null) {
				continue;
			}
			
			clipsRemaining.Add(i);
		}
	}
	
	private void PlayPlaylistSong(MusicSetting setting) {
		AudioSource audioClip;
		AudioSource transClip;
		
		if (activeAudio == null) {
			Debug.LogError("PlaylistController prefab is not in your scene. Cannot play a song.");
			return;
		}
		
		if (activeAudio.clip == null) {
			audioClip = activeAudio;
			transClip = transitioningAudio;
		} else if (transitioningAudio.clip == null) {
			audioClip = transitioningAudio;
			transClip = activeAudio;
		} else {
			// both are busy!
			//Debug.LogWarning("Both audio sources are busy cross-fading. You may want to shorten your cross-fade time in Playlist Inspector.");
			audioClip = transitioningAudio;
			transClip = activeAudio;
		}

		if (setting.clip != null) {
			audioClip.clip = setting.clip;
			audioClip.pitch = setting.pitch;
		}
		
		audioClip.loop = setting.isLoop;
		audioClip.clip = setting.clip;
		audioClip.pitch = setting.pitch;
		
		if (MasterAudio.Instance.CrossFadeTime == 0 || transClip.clip == null) {
			CeaseAudioSource(transClip);
			audioClip.volume = setting.volume * MasterAudio.PlaylistMasterVolume;
		} else {
			audioClip.volume = 0f;
			isCrossFading = true;
			duckingMode = AudioDuckingMode.NotDucking;
			crossFadeStartTime = Time.time;
		}
		
		SetDuckProperties();
		
		var playlist = MasterAudio.CurrentPlaylist;
		if (playlist.songTransitionType == MasterAudio.SongFadeInPosition.SynchronizeClips) {
			transitioningAudio.time = activeAudio.time;
		}
		
		audioClip.Play();
		
		noNextSong = false;
		
		activeAudio = audioClip;
		transitioningAudio = transClip;
		
		activeAudioEndVolume = setting.volume * MasterAudio.PlaylistMasterVolume;
		transitioningAudioStartVolume = transitioningAudio.volume * MasterAudio.PlaylistMasterVolume;
		currentSong = setting;
	}

	private void CeaseAudioSource(AudioSource source) {
		source.Stop();
		source.clip = null;
	}

	private void SetDuckProperties() {
		originalMusicVolume = activeAudio.volume;
		if (currentSong != null) {
			originalMusicVolume = currentSong.volume * MasterAudio.PlaylistMasterVolume;
		}

		initialDuckVolume = MasterAudio.Instance.DuckedVolumeMultiplier * originalMusicVolume;
        duckRange = originalMusicVolume - MasterAudio.Instance.DuckedVolumeMultiplier;

		duckingMode = AudioDuckingMode.NotDucking; // cancel any ducking
	}
	
	private void AudioDucking() {
		switch (duckingMode) {
			case AudioDuckingMode.NotDucking:
				break;
			case AudioDuckingMode.SetToDuck:
				activeAudio.volume = initialDuckVolume;
				duckingMode = AudioDuckingMode.Ducked;
				break;
			case AudioDuckingMode.Ducked:
				if (Time.time >= timeToFinishUnducking) {
					activeAudio.volume = originalMusicVolume;
					duckingMode = AudioDuckingMode.NotDucking;
				} else if (Time.time >= timeToStartUnducking) { 
					activeAudio.volume = initialDuckVolume + (Time.time - timeToStartUnducking) / (timeToFinishUnducking - timeToStartUnducking) * duckRange;
				}
				break;
		} 
	}
	#endregion

	#region Properties
    /// <summary>
    /// This property returns the GameObject for the PlaylistController's GameObject.
    /// </summary>
	public GameObject PlaylistControllerGameObject {
		get {
			return go;
		}
	}

    /// <summary>
    ///  This property returns the current Audio Source for the current Playlist song that is playing.
    /// </summary>
	public AudioSource CurrentPlaylistSource {
		get {
			if (activeAudio == null) {
				return null;
			}
			
			return activeAudio;
		}
	}
	
    /// <summary>
    ///  This property returns the current Audio Clip for the current Playlist song that is playing.
    /// </summary>
	public AudioClip CurrentPlaylistClip {
		get {
			if (activeAudio == null) {
				return null;
			}
			
			return activeAudio.clip;
		}
	}
	
    /// <summary>
    /// This property returns the currently fading out Audio Clip for the Playlist (null if not during cross-fading).
    /// </summary>
	public AudioClip FadingPlaylistClip {
		get {
			if (!isCrossFading) {
				return null;
			}
			
			if (transitioningAudio == null) {
				return null;
			}
			
			return transitioningAudio.clip;
		}
	}
	
    /// <summary>
    /// This property returns the currently fading out Audio Source for the Playlist (null if not during cross-fading).
    /// </summary>
	public AudioSource FadingSource {
		get {
			if (!isCrossFading) {
				return null;
			}
			
			if (transitioningAudio == null) {
				return null;
			}
			
			return transitioningAudio;
		}
	}
	
    /// <summary>
    /// This property returns whether or not the Playlist is currently cross-fading.
    /// </summary>
	public bool IsCrossFading {
		get {
			return isCrossFading;
		} 
	}

	#endregion
}
