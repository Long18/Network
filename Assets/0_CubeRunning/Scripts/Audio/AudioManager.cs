using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    [Header("Config")] [SerializeField] private SettingsSO settings = default;
    [SerializeField] private SaveSystem saveSystem = default;

    [Header("SoundEmitters pool")] [SerializeField]
    private SoundEmitterPoolSO pool = default;

    [SerializeField] private int initialSize = 10;

    [Header("Listening on channels")]
    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
    [SerializeField]
    private AudioCueEventChannelSO SFXEventChannel = default;

    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")] [SerializeField]
    private AudioCueEventChannelSO musicEventChannel = default;


    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change SFXs volume")]
    [SerializeField]
    private FloatEventChannelSO SFXVolumeEventChannel = default;


    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Music volume")]
    [SerializeField]
    private FloatEventChannelSO musicVolumeEventChannel = default;


    [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to change Master volume")]
    [SerializeField]
    private FloatEventChannelSO masterVolumeEventChannel = default;


    [Header("Audio control")] [SerializeField]
    private AudioMixer audioMixer = default;

    [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;

    private SoundEmitterVault soundEmitterVault;
    private SoundEmitter musicSoundEmitter;

    private void Awake()
    {
        //TODO: Get the initial volume levels from the settings
        soundEmitterVault = new SoundEmitterVault();

        pool.Prewarm(initialSize);
        pool.SetParent(this.transform);

        if (saveSystem.LoadSaveDataFromDisk())
        {
            masterVolumeEventChannel.RaiseEvent(settings.MasterVolume);
            musicVolumeEventChannel.RaiseEvent(settings.MusicVolume);
            SFXVolumeEventChannel.RaiseEvent(settings.SfxVolume);
        }
    }

    private void OnEnable()
    {
        SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
        SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
        SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

        musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
        musicEventChannel.OnAudioCueStopRequested += StopMusic;

        masterVolumeEventChannel.OnEventRaised += ChangeMasterVolume;
        musicVolumeEventChannel.OnEventRaised += ChangeMusicVolume;
        SFXVolumeEventChannel.OnEventRaised += ChangeSFXVolume;
    }

    private void OnDestroy()
    {
        SFXEventChannel.OnAudioCuePlayRequested -= PlayAudioCue;
        SFXEventChannel.OnAudioCueStopRequested -= StopAudioCue;

        SFXEventChannel.OnAudioCueFinishRequested -= FinishAudioCue;
        musicEventChannel.OnAudioCuePlayRequested -= PlayMusicTrack;

        musicVolumeEventChannel.OnEventRaised -= ChangeMusicVolume;
        SFXVolumeEventChannel.OnEventRaised -= ChangeSFXVolume;
        masterVolumeEventChannel.OnEventRaised -= ChangeMasterVolume;
    }

    /// <summary>
    /// This is only used in the Editor, to debug volumes.
    /// It is called when any of the variables is changed, and will directly change the value of the volumes on the AudioMixer.
    /// </summary>
#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (saveSystem.LoadSaveDataFromDisk())
            {
                masterVolume = settings.MasterVolume;
                musicVolume = settings.MusicVolume;
                sfxVolume = settings.SfxVolume;
            }

            SetGroupVolume("MasterVolume", masterVolume);
            SetGroupVolume("MusicVolume", musicVolume);
            SetGroupVolume("SFXVolume", sfxVolume);
        }
    }
#endif

    void ChangeMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        SetGroupVolume("MasterVolume", masterVolume);
    }

    void ChangeMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        SetGroupVolume("MusicVolume", musicVolume);
    }

    void ChangeSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
        SetGroupVolume("SFXVolume", sfxVolume);
    }

    public void SetGroupVolume(string parameterName, float normalizedVolume)
    {
        bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(normalizedVolume));
        if (!volumeSet)
            Debug.LogError("The AudioMixer parameter was not found");
    }

    public float GetGroupVolume(string parameterName)
    {
        if (audioMixer.GetFloat(parameterName, out float rawVolume))
        {
            return MixerValueToNormalized(rawVolume);
        }
        else
        {
            Debug.LogError("The AudioMixer parameter was not found");
            return 0f;
        }
    }

    // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
    /// when using UI sliders normalized format
    private float MixerValueToNormalized(float mixerValue)
    {
        // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
        return 1f + (mixerValue / 80f);
    }

    private float NormalizedToMixerValue(float normalizedValue)
    {
        // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
        // This doesn't allow values over 0dB
        return (normalizedValue - 1f) * 80f;
    }

    private AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
        Vector3 positionInSpace)
    {
        float fadeDuration = 2f;
        float startTime = 0f;

        if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
        {
            AudioClip songToPlay = audioCue.GetClips()[0];
            if (musicSoundEmitter.GetClip() == songToPlay)
                return AudioCueKey.Invalid;

            //Music is already playing, need to fade it out
            startTime = musicSoundEmitter.FadeMusicOut(fadeDuration);
        }

        musicSoundEmitter = pool.Request();
        musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
        musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;

        return AudioCueKey.Invalid; //No need to return a valid key for music
    }

    private bool StopMusic(AudioCueKey key)
    {
        if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
        {
            musicSoundEmitter.Stop();
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Only used by the timeline to stop the gameplay music during cutscenes.
    /// Called by the SignalReceiver present on this same GameObject.
    /// </summary>
    public void TimelineInterruptsMusic()
    {
        StopMusic(AudioCueKey.Invalid);
    }

    /// <summary>
    /// Plays an AudioCue by requesting the appropriate number of SoundEmitters from the pool.
    /// </summary>
    public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
    {
        AudioClip[] clipsToPlay = audioCue.GetClips();
        SoundEmitter[] soundEmitterArray = new SoundEmitter[clipsToPlay.Length];

        int nOfClips = clipsToPlay.Length;
        for (int i = 0; i < nOfClips; i++)
        {
            soundEmitterArray[i] = pool.Request();
            if (soundEmitterArray[i] != null)
            {
                soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings, audioCue.looping, position);
                if (!audioCue.looping)
                    soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }

        return soundEmitterVault.Add(audioCue, soundEmitterArray);
    }

    public bool FinishAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                soundEmitters[i].Finish();
                soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }
        else
        {
            Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
        }

        return isFound;
    }

    public bool StopAudioCue(AudioCueKey audioCueKey)
    {
        bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

        if (isFound)
        {
            for (int i = 0; i < soundEmitters.Length; i++)
            {
                StopAndCleanEmitter(soundEmitters[i]);
            }

            soundEmitterVault.Remove(audioCueKey);
        }

        return isFound;
    }

    private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
    {
        StopAndCleanEmitter(soundEmitter);
    }

    private void StopAndCleanEmitter(SoundEmitter soundEmitter)
    {
        if (!soundEmitter.IsLooping())
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;

        soundEmitter.Stop();
        pool.Return(soundEmitter);

        //TODO: is the above enough?
        //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
        //How is the key removed from the vault?
    }

    private void StopMusicEmitter(SoundEmitter soundEmitter)
    {
        soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
        pool.Return(soundEmitter);
    }
}