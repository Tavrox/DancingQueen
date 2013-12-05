using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class VariationInfo
{
    public int weight = 1;
    public float randomPitch = 0f;
    public float randomVolume = 0f;
    public string possibleName = string.Empty;
    public SoundGroupVariation.AudioLocation audLocation = SoundGroupVariation.AudioLocation.Clip;
    public string resourceFileName;
    public bool useFades = false;
    public float fadeInTime = 0f;
    public float fadeOutTime = 0f;
    public float originalPitch;

    public Vector3 originalPosition;
    public string variationName;
    public Transform groupParent;
    public string groupName;

    public float audPitch;
    public float audVolume;
    public bool audLoop;
    public AudioClip audClip;
    public int originalInstanceId;

    public bool audMute;
    public bool audBypassFx;
    public bool audBypassListenerFx;
    public bool audBypassReverbZones;
    public bool audPlayOnAwake;
    public int audPriority;
    public float audDoppler;
    public AudioRolloffMode audVolumeRolloffMode;
    public float audMinDistance;
    public float audPanLevel;
    public float audPan;
    public float audSpread;
    public float audMaxDistance;

    // FX
    public bool hasLPFilter;
    public bool lpFilterEnabled;
    public float lpResonanceQ;
    public float lpCutoffFrequency;

    public bool hasHPFilter;
    public bool hpFilterEnabled;
    public float hpResonanceQ;
    public float hpCutoffFrequency;

    public bool hasEchoFilter;
    public bool echoEnabled;
    public float echoDelay;
    public float echoDecayRatio;
    public float echoWetMix;
    public float echoDryMix;

    public bool hasDistFilter;
    public bool distEnabled;
    public float distLevel;

    public bool hasReverbFilter;
    public bool reverbEnabled;
    public AudioReverbPreset reverbPreset;
    public float reverbDryLevel;
    public float reverbRoom;
	public float reverbRoomHF;
	public float reverbRoomLF;
    public float reverbDecayTime;
    public float reverbDecayHFRatio;
    public float reverbReflectionsLevel;
    public float reverbReflectionsDelay;
    public float reverbLevel;
    public float reverbDelay;
    public float reverbHfReference;
    public float reverbLFReference;
    public float reverbDiffusion;
    public float reverbDensity;

    public bool hasChorusFilter;
    public bool chorusEnabled;
    public float chorusDryMix;
    public float chorusWetMix1;
    public float chorusWetMix2;
    public float chorusWetMix3;
    public float chorusDelay;
    public float chorusRate;
    public float chorusDepth;
}
