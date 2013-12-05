using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterAudioGroup : MonoBehaviour {
	public const string NO_BUS = "[NO BUS]";
	
	public Texture logoTexture;
	public Texture settingsTexture;
	public Texture deleteTexture;
	public int busIndex = -1;
	
	public bool isExpanded = true;
	public float groupMasterVolume = 1f;
	public int retriggerPercentage = 50;
	public VariationMode curVariationMode = VariationMode.Normal;
	public List<SoundGroupVariation> groupVariations = new List<SoundGroupVariation>();
	public SoundGroupVariation.AudioLocation bulkVariationMode = SoundGroupVariation.AudioLocation.Clip;
	
	public LimitMode limitMode = LimitMode.None;
	public int limitPerXFrames = 1;
	public float minimumTimeBetween = 0.1f;
	
	public bool limitPolyphony = false;
	public int voiceLimitCount = 1;
	
	public bool isSoloed = false;
	public bool isMuted = false;
	
	public enum VariationMode {
		Normal,
		LoopedChain				
	}
	
	public enum LimitMode {
		None,
		FrameBased,
		TimeBased
	}
}
