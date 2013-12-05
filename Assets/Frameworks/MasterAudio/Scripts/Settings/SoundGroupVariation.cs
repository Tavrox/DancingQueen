using UnityEngine;
using System;
using System.Collections;

public class SoundGroupVariation : MonoBehaviour {
	private const int FRAMES_EARLY_TO_TRIGGER = 2;
	
	public Texture logoTexture;
	public int weight = 1;
	public float randomPitch = 0f;
	public float randomVolume = 0f;
	public string possibleName = string.Empty;
	public AudioLocation audLocation = AudioLocation.Clip;
	public string resourceFileName;
	public bool useFades = false;
	public float fadeInTime = 0f;
	public float fadeOutTime = 0f;
	public float originalPitch;
	
	private AudioSource _audio;
	private Transform trans;
	private Transform originalParent;
	private float fadeMaxVolume;
	private FadeMode curFadeMode = FadeMode.None;
	private DetectEndMode curDetectEndMode = DetectEndMode.None;
	private PlaySoundParams playSndParams = null;
	
	private AudioDistortionFilter distFilter;
	private AudioEchoFilter echoFilter;
	private AudioHighPassFilter hpFilter;
	private AudioLowPassFilter lpFilter;
	private AudioReverbFilter reverbFilter;
	private AudioChorusFilter chorusFilter;
	
	public delegate void SoundFinishedEventHandler();
	public event SoundFinishedEventHandler SoundFinished;
	
	public class PlaySoundParams {
		public string soundType;
		public float volumePercentage; 
		public float? pitch;
		public Transform sourceTrans;
		public bool attachToSource;
		public float delaySoundTime;
		public bool isChainLoop;
		public float groupCalcVolume;
		
		public PlaySoundParams(string _soundType, float _volPercent, float _groupCalcVolume, float? _pitch, Transform _sourceTrans, bool _attach, float _delaySoundTime, bool _isChainLoop) {
			soundType = _soundType;
			volumePercentage = _volPercent;
			groupCalcVolume = _groupCalcVolume;
			pitch = _pitch;
			sourceTrans = _sourceTrans;
			attachToSource = _attach;
			delaySoundTime = _delaySoundTime;
			isChainLoop = _isChainLoop;
		}
	}
	
	public enum AudioLocation {
		Clip,
		ResourceFile
	}
	
	public enum FadeMode {
		None,
		FadeInOut,
		FadeOutEarly,
		GradualFade
	}
	
	public enum DetectEndMode {
		None,
		DetectEnd
	}
	
	void Awake() {
		_audio = this.audio;
		trans = this.transform;
	}
	
	void Start() {
		this.originalParent = this.trans.parent;
        originalPitch = _audio.pitch;
    }
	
	void OnDestroy() {
		StopSoundEarly();
	}
	
	void OnDisable() {
		StopSoundEarly();
	}

    private void StopSoundEarly()
    {
		if (MasterAudio.AppIsShuttingDown) {
			return;	
		}

        Stop(false, true); // maybe unload clip from Resources
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawIcon(this.transform.position, MasterAudio.GIZMO_FILE_NAME, true);
	}
	
    /// <summary>
    /// This method is called automatically from MasterAudio.PlaySound and MasterAudio.PlaySound3D. 
    /// </summary>
    /// <param name="maxVolume">If fade in time is not zero on this Variation, max volume is the fully faded in clip's target volume. Otherwise this is not used.</param>
	public void Play(float? pitch, float maxVolume, PlaySoundParams playParams = null) {
		SoundFinished = null; // clear it out so subscribers don't have to clean up
		playSndParams = playParams;
		
		// compute pitch
		if (pitch.HasValue) {
            _audio.pitch = pitch.Value;
		} else {
			var randPitch = 0f;
			
			if (randomPitch != 0f) {
				randPitch = UnityEngine.Random.Range(-randomPitch, randomPitch);
			}
            _audio.pitch = originalPitch + randPitch;
        }		
		
		// set fade mode
		this.curFadeMode = FadeMode.None;
		curDetectEndMode = DetectEndMode.DetectEnd;
		
		if (audLocation == AudioLocation.ResourceFile) {
			AudioResourceOptimizer.PopulateSourcesWithResourceClip(resourceFileName);
		}

		StopAllCoroutines();
		
		if (!_audio.isPlaying && _audio.time > 0f) {
			// paused. Do nothing except Play
		} else if (useFades && (fadeInTime > 0f || fadeOutTime > 0f)) {
			fadeMaxVolume = maxVolume;
			_audio.volume = 0f;
			StartCoroutine(FadeInOut());
		} 
		
		if (playSndParams != null && playSndParams.isChainLoop) {
			_audio.loop = false;
		}
		
		StartCoroutine(DetectSoundFinished(playParams.delaySoundTime));
	}
	
	public void AdjustVolume(float volumePercentage) {
		if (!audio.isPlaying || playSndParams == null) {
			return;
		}
		
		var newVol = playSndParams.groupCalcVolume * volumePercentage;
		_audio.volume = newVol;
		
		playSndParams.volumePercentage = volumePercentage;
	}
	
    /// <summary>
    /// This method allows you to pause the audio being played by this Variation. This is automatically called by MasterAudio.PauseSoundGroup and MasterAudio.PauseBus.
    /// </summary>
	public void Pause() {
		if (audLocation == AudioLocation.ResourceFile) {
			Stop();
			return;
		}
		
		_audio.Pause();
		this.curFadeMode = FadeMode.None;

		MaybeUnloadClip();
	}
	
	private void MaybeUnloadClip() {
		if (audLocation == AudioLocation.ResourceFile) {
			AudioResourceOptimizer.UnloadClipIfUnused(resourceFileName);
		}
	} 
	
    /// <summary>
    /// This method allows you to stop the audio being played by this Variation. 
    /// </summary>
    public void Stop(bool stopEndDetection = false, bool isDestroyedOrDisabled = false)
    {
		if (stopEndDetection) {
			curDetectEndMode = DetectEndMode.None; // turn off the chain loop endless repeat
		}
		
		_audio.Stop();

        if (isDestroyedOrDisabled && this.trans.parent != originalParent)
        {
            MasterAudio.TrackLostVariation(this);
        }

		playSndParams = null;
		
		MaybeUnloadClip();
	}
	
    /// <summary>
    /// This method allows you to fade the sound from this Variation to a specified volume over X seconds.
    /// </summary>
    /// <param name="newVolume">The target volume to fade to.</param>
    /// <param name="fadeTime">The time it will take to fully fade to the target volume.</param>
	public void FadeToVolume(float newVolume, float fadeTime) {
		this.curFadeMode = FadeMode.GradualFade;
		
		StartCoroutine(FadeOverTimeToVolume(newVolume, fadeTime));
	}
	
	private IEnumerator FadeOverTimeToVolume(float targetVolume, float fadeTime) {
		if (fadeTime <= MasterAudio.INNER_LOOP_CHECK_INTERVAL) {
			_audio.volume = targetVolume; // time really short, just do it at once.
			if (_audio.volume <= 0f) {
				Stop();
			}
			curFadeMode = FadeMode.None;
			yield break;
		}
		
		yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL); // wait for the clip to start playing :)
		
		var volStep = (targetVolume - _audio.volume) / (fadeTime / MasterAudio.INNER_LOOP_CHECK_INTERVAL);
		float newVol;
		while (_audio.volume != targetVolume && curFadeMode == FadeMode.GradualFade) {		
			newVol = _audio.volume + volStep;
			
			if (volStep > 0f) {
				newVol = Math.Min(newVol, targetVolume);	
			} else {
				newVol = Math.Max(newVol, targetVolume);	
			}
			
			_audio.volume = newVol;
			
			yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL); // wait for the clip to start playing :)
		}
		
		if (_audio.volume <= 0f) {
			Stop();
		}
		
		if (curFadeMode != FadeMode.GradualFade) {
			yield break; // in case another fade cancelled this one
		}
		
		curFadeMode = FadeMode.None;
	}
	
    /// <summary>
    /// This method will fully fade out the sound from this Variation to zero using its existing fadeOutTime.
    /// </summary>
	public void FadeOutNow() {
		if (MasterAudio.AppIsShuttingDown) {
			return;
		}
		StartCoroutine(FadeOutEarly(fadeOutTime));
	}

    /// <summary>
    /// This method will fully fade out the sound from this Variation to zero using over X seconds.
    /// </summary>
    /// <param name="fadeTime">The time it will take to fully fade to the target volume.</param>
    public void FadeOutNow(float fadeTime)
    {
		if (MasterAudio.AppIsShuttingDown) {
			return;
		}
		StartCoroutine(FadeOutEarly(fadeTime));
	}
	
	private IEnumerator FadeOutEarly(float fadeTime) {
		curFadeMode = FadeMode.FadeOutEarly; // cancel the FadeInOut loop, if it's going.
		
		var stepVolumeDown = _audio.volume / fadeTime * MasterAudio.INNER_LOOP_CHECK_INTERVAL;
		
		while (_audio.isPlaying && curFadeMode == FadeMode.FadeOutEarly) {
			_audio.volume -= stepVolumeDown;			
			yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
		}
		
		_audio.volume = 0f;
		Stop();
		
		if (curFadeMode != FadeMode.FadeOutEarly) {
			yield break; // in case another fade cancelled this one
		}
		
		curFadeMode = FadeMode.None;
	}
	
	private IEnumerator FadeInOut() {
		var fadeOutStartTime = _audio.clip.length - (fadeOutTime * _audio.pitch);
		
		yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL); // wait for the clip to start playing :)

		var stepVolumeUp = fadeMaxVolume / fadeInTime * MasterAudio.INNER_LOOP_CHECK_INTERVAL;
		
		curFadeMode = FadeMode.FadeInOut; // wait to set this so it stops the previous one if it's still going.

		if (fadeInTime > 0f) {
			while (_audio.isPlaying && curFadeMode == FadeMode.FadeInOut && _audio.time < fadeInTime) {
				_audio.volume += stepVolumeUp;			
				yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
			}
		}
		
		if (curFadeMode != FadeMode.FadeInOut) {
			yield break; // in case another fade cancelled this one
		}
		
		_audio.volume = fadeMaxVolume; // just in case it didn't align exactly
		
		if (fadeOutTime == 0f) {
			yield break; // nothing more to do!
		}
		
		// wait for fade out time.
		while (_audio.isPlaying && curFadeMode == FadeMode.FadeInOut && _audio.time < fadeOutStartTime) {
			yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
		}

		if (curFadeMode != FadeMode.FadeInOut) {
			yield break; // in case another fade cancelled this one
		}
		
		var stepVolumeDown = fadeMaxVolume / fadeOutTime * MasterAudio.INNER_LOOP_CHECK_INTERVAL;

		while (_audio.isPlaying && curFadeMode == FadeMode.FadeInOut) {
			_audio.volume -= stepVolumeDown;			
			yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
		}
		
		audio.volume = 0f;
		Stop();
		
		if (curFadeMode != FadeMode.FadeInOut) {
			yield break; // in case another fade cancelled this one
		}

		curFadeMode = FadeMode.None;
	}
	
	private IEnumerator DetectSoundFinished(float delaySound) {
		if (delaySound > 0f) {
			yield return new WaitForSeconds(delaySound);
		}
		
		_audio.Play();
		
		while (curDetectEndMode == DetectEndMode.DetectEnd) {
			//yield return new WaitForSeconds(MasterAudio.INNER_LOOP_CHECK_INTERVAL);
			yield return new WaitForSeconds(1);
			
			if (this._audio.time == 0f || this._audio.clip.length - this._audio.time < Time.deltaTime * FRAMES_EARLY_TO_TRIGGER) {
				break;
			}
		}

		if (SoundFinished != null) {
			SoundFinished(); // parameters aren't used
		}
		
		var playSnd = playSndParams;
		
		if (!_audio.loop || (playSndParams != null && playSndParams.isChainLoop)) {
			Stop();
		}
		
		if (curDetectEndMode != DetectEndMode.DetectEnd) {
			yield break;
		}
		
		if (playSnd != null && playSnd.isChainLoop) {
			if (playSnd.attachToSource || playSnd.sourceTrans != null) {
				MasterAudio.PlaySound3D(playSnd.soundType, playSnd.sourceTrans, playSnd.attachToSource, playSnd.volumePercentage, playSnd.pitch, playSnd.delaySoundTime);
			} else {
				MasterAudio.PlaySound(playSnd.soundType, playSnd.volumePercentage, playSnd.pitch, playSnd.delaySoundTime);
			}
		}
	}
	
	public Transform OriginalParent {
		get {
			return originalParent;
		}
	}
	
	public AudioDistortionFilter DistortionFilter {
		get {
			if (distFilter == null) {			
				distFilter = this.GetComponent<AudioDistortionFilter>();
			}
			
			return distFilter;
		}
	}
	
	public AudioReverbFilter ReverbFilter {
		get {
			if (reverbFilter == null) {			
				reverbFilter = this.GetComponent<AudioReverbFilter>();
			}
			
			return reverbFilter;
		}
	}

	public AudioChorusFilter ChorusFilter {
		get {
			if (chorusFilter == null) {			
				chorusFilter = this.GetComponent<AudioChorusFilter>();
			}
			
			return chorusFilter;
		}
	}

	public AudioEchoFilter EchoFilter {
		get {
			if (echoFilter == null) {			
				echoFilter = this.GetComponent<AudioEchoFilter>();
			}
			
			return echoFilter;
		}
	}

	public AudioLowPassFilter LowPassFilter {
		get {
			if (lpFilter == null) {			
				lpFilter = this.GetComponent<AudioLowPassFilter>();
			}
			
			return lpFilter;
		}
	}

	public AudioHighPassFilter HighPassFilter {
		get {
			if (hpFilter == null) {			
				hpFilter = this.GetComponent<AudioHighPassFilter>();
			}
			
			return hpFilter;
		}
	}
}
