FREE VERSION

AntiLunchBox Studios
www.antilunchbox.com

SoundManagerPro 2.0 is a plugin to specifically designed to help you handle music in your game quickly and easily--with style.

We want all games to have a certain quality of music so we decided to provide you this plugin for free!
No features are held back! You just have a watermark at the bottom right hand corner of your screen if you have
SoundManager in the scene.

To Get Rid of the Watermark:
-Purchase the full version of SoundManagerPro here: http://u3d.as/content/anti-lunch-box-studios/sound-manager-pro

A How-To video is available here:
http://www.youtube.com/watch?v=2hYA8O5VdqE

Previous video from 1.0 is here:
http://www.youtube.com/watch?v=rxDxtJtDSn4

And the documentation is available here and below:
http://www.antilunchbox.com/soundmanagerpro.html

Happy Gaming,
	AntiLunchBox Studios






















**************************************************************************************************

Important note: Avoid running functions in Awake()

For a How-To video, visit: http://www.youtube.com/watch?v=2hYA8O5VdqE

---------------- Static Functions Related to Music ----------------
// Adds the SoundConnection to the manager.  It will not add multiple SoundConnections to one level.  
public static void AddSoundConnection(SoundConnection sc) 
	
//  Removes the SoundConnection for level.  It will not remove anything if that level does not exist.
public static void RemoveSoundConnectionForLevel(string lvl)

// Replaces the SoundConnection at that level.  If a SoundConnection doesn't exist, it will just add it.
public static void ReplaceSoundConnection(SoundConnection sc) 

// Checks if a level has a SoundConnection.  If it does, it returns the index.  If it doesn't it returns constant SOUNDMANAGER_FALSE(-1)
public static int SoundConnectionsContainsThisLevel(string lvl)

// Gets the sound connection for this level.
public static SoundConnection GetSoundConnectionForThisLevel(string lvl) 
	
// Toggle Mutes/Unmutes all music.  Returns a bool if it's muted or not.
public static bool MuteMusic()

// Sets Mute to music. Returns a bool if it's muted or not.
public static bool MuteMusic(bool toggle)
	
// Mutes/Unmutes all sounds.  Returns a bool if it's muted or not, priority is given to the music mute
public static bool Mute()
	
// Sets Mute to all sounds
public static bool Mute(bool toggle)

// Sets the maximum volume of music in the game relative to the global volume.
public static void SetVolumeMusic(float setVolume)
	
// Sets the pitch of music in the game.
public static void SetPitchMusic(float setPitch)
	
// Sets the maximum volume of all sound in the game.
public static void SetVolume(float setVolume)
	
// Sets the pitch of all sound in the game and the default pitch to any new sounds coming in.
public static void SetPitch(float setPitch)

// Plays the SoundConnection right then, regardless of what you put at the level parameter of the SoundConnection.
// It adds the SoundConnection for the current level from then on out.
// If the current level already has a SoundConnection, it replaces it.
public static void PlayConnection(SoundConnection sc)
	
// Plays the clip immediately regardless of a SoundConnection with an option to loop.  Calls an event once the clip is done.
// You can resume a SoundConnection afterwards if you so choose, using SoundManager.Instance.currentSoundConnection.  However, it will not resume on it's own.
// Callbacks will only fire once.
public static void PlayImmediately(AudioClip clip2play, bool loop, SongCallBack runOnEndFunction)

// Plays the clip immediately regardless of a SoundConnection with an option to loop.  It will not resume on it's own.
public static void PlayImmediately(AudioClip clip2play, bool loop)
	
// Plays the clip immediately regardless of a SoundConnection.  It will not resume on it's own.
public static void PlayImmediately(AudioClip clip2play)
	
// Plays the clip by crossing out what's currently playing regardless of a SoundConnection with an option to loop.  Calls an event once the clip is done.
// You can resume a SoundConnection afterwards if you so choose, using SoundManager.Instance.currentSoundConnection.  However, it will not resume on it's own.
// Callbacks will only fire once.
public static void Play(AudioClip clip2play, bool loop, SongCallBack runOnEndFunction)

// Plays the clip by crossing out what's currently playing regardless of a SoundConnection with an option to loop.  It will not resume on it's own.
public static void Play(AudioClip clip2play, bool loop)

// Plays the clip by crossing out what's currently playing regardless of a SoundConnection.  It will not resume on it's own.
public static void Play(AudioClip clip2play)

// Stops all sound immediately.
public static void StopImmediately()
	
// Crosses out all AudioSources.
public static void Stop()
	
// Sets ignore level load, which will decide whether or not to use SoundManagerPro's level load AI.
public static void SetIgnoreLevelLoad(bool ignore)



---------------- Static Functions Related to SFX ----------------
// Path to folder where SFX are held in resources
public static string RESOURCES_PATH = "Sounds/SFX";
	
// Sets the SFX cap.
public static void SetSFXCap(int cap)
    
// Plays the SFX on an owned object, will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlaySFX(AudioClip clip, float volume, float pitch, Vector3 location)

// Plays the SFX on an owned object, will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlaySFX(AudioClip clip, float volume, float pitch)

// Plays the SFX on an owned object, will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlaySFX(AudioClip clip, float volume)

// Plays the SFX on an owned object, will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlaySFX(AudioClip clip)

// Plays the SFX IFF other SFX with the same cappedID are not over the cap limit. Will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlayCappedSFX(AudioClip clip, string cappedID, float volume, float pitch, Vector3 location)

// Plays the SFX IFF other SFX with the same cappedID are not over the cap limit. Will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlayCappedSFX(AudioClip clip, string cappedID, float volume, float pitch)

// Plays the SFX IFF other SFX with the same cappedID are not over the cap limit. Will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlayCappedSFX(AudioClip clip, string cappedID, float volume)

// Plays the SFX IFF other SFX with the same cappedID are not over the cap limit. Will default the location to (0,0,0), pitch to 1f, volume to 1f
public static AudioSource PlayCappedSFX(AudioClip clip, string cappedID)

// Plays the SFX another gameObject of your choice, will default the looping to false, pitch to 1f, volume to 1f
public static AudioSource PlaySFX(GameObject objp, AudioClip clip, bool looping, float volume, float pitch)
	
// Plays the SFX another gameObject of your choice, will default the looping to false, pitch to 1f, volume to 1f
public static AudioSource PlaySFX(GameObject objp, AudioClip clip, bool looping, float volume)

// Plays the SFX another gameObject of your choice, will default the looping to false, pitch to 1f, volume to 1f
public static AudioSource PlaySFX(GameObject objp, AudioClip clip, bool looping)

// Plays the SFX another gameObject of your choice, will default the looping to false, pitch to 1f, volume to 1f
public static AudioSource PlaySFX(GameObject objp, AudioClip clip)

// Stops the SFX on another gameObject
public static void StopSFX(GameObject objp)

// Plays the SFX in a loop on another gameObject of your choice.  This function is cattered more towards customizing a loop.
// You can set the loop to end when the object dies or a maximum duration, whichever comes first.
// tillDestroy defaults to true, volume to 1f, pitch to 1f, maxDuration to 0f
public static AudioSource PlaySFXLoop(GameObject objp, AudioClip clip, bool tillDestroy, float volume, float pitch, float maxDuration)

// Plays the SFX in a loop on another gameObject of your choice.  This function is cattered more towards customizing a loop.
// You can set the loop to end when the object dies or a maximum duration, whichever comes first.
// tillDestroy defaults to true, volume to 1f, pitch to 1f, maxDuration to 0f
public static AudioSource PlaySFXLoop(GameObject objp, AudioClip clip, bool tillDestroy, float volume, float pitch)

// Plays the SFX in a loop on another gameObject of your choice.  This function is cattered more towards customizing a loop.
// You can set the loop to end when the object dies or a maximum duration, whichever comes first.
// tillDestroy defaults to true, volume to 1f, pitch to 1f, maxDuration to 0f
public static AudioSource PlaySFXLoop(GameObject objp, AudioClip clip, bool tillDestroy, float volume)

// Plays the SFX in a loop on another gameObject of your choice.  This function is cattered more towards customizing a loop.
// You can set the loop to end when the object dies or a maximum duration, whichever comes first.
// tillDestroy defaults to true, volume to 1f, pitch to 1f, maxDuration to 0f

// Plays the SFX in a loop on another gameObject of your choice.  This function is cattered more towards customizing a loop.
// You can set the loop to end when the object dies or a maximum duration, whichever comes first.
// tillDestroy defaults to true, volume to 1f, pitch to 1f, maxDuration to 0f
public static AudioSource PlaySFXLoop(GameObject objp, AudioClip clip)

// Sets mute on all the SFX to 'mute'. Returns the result.
public static bool MuteSFX(bool toggle)

// Toggles mute on SFX. Returns the result.
public static bool MuteSFX()
	
// Sets the maximum volume of SFX in the game relative to the global volume.
public static void SetVolumeSFX(float setVolume)

// Sets the pitch of SFX in the game.
public static void SetPitchSFX(float setPitch)

// Saves the SFX to the SoundManager prefab for easy access for frequently used SFX.
public static void SaveSFX(AudioClip clip)

// Load the specified clipname, at a custom path if you do not want to use SOUNDMANAGER.RESOURCES_PATH.
// If custompath fails or is empty/null, it will query the stored SFXs.  If that fails, it'll query the default
// SOUNDMANAGER.RESOURCES_PATH.  If all else fails, it'll return null.
public static AudioClip Load(string clipname, string customPath)

// Load the specified clipname from the stored SFXs.  If that fails, it'll query the default
// SOUNDMANAGER.RESOURCES_PATH.  If all else fails, it'll return null.
public static AudioClip Load(string clipname)

// Reset a SFX Object to it's default parameters
public static void ResetSFXObject(GameObject sfxObj)

---------------- Useful Instance Variables ----------------
// Access these from SoundManager.Instance
public SoundConnection currentSoundConnection;
public float crossDuration;
public bool showDebug;
public bool offTheBGM;
public bool offTheSFX;
	
public delegate void SongCallBack();
public SongCallBack OnSongEnd;
public SongCallBack OnSongBegin;
public SongCallBack OnCrossOutBegin;
public SongCallBack OnCrossInBegin;	

