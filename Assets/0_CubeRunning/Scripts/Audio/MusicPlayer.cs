using UnityEngine;
using UnityEngine.Serialization;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onSceneReady = default;
    [SerializeField] private AudioCueEventChannelSO playMusicOn = default;
    [SerializeField] private GameSceneSO thisSceneSO = default;

    [SerializeField] private AudioConfigurationSO audioConfig = default;

    [Header("Pause menu music")] [SerializeField]
    private AudioCueSO pauseMusic = default;

    [SerializeField] private BoolEventChannelSO onPauseOpened = default;

    private void OnEnable()
    {
        onPauseOpened.OnEventRaised += PlayPauseMusic;
        onSceneReady.OnEventRaised += PlayMusic;
    }

    private void OnDisable()
    {
        onSceneReady.OnEventRaised -= PlayMusic;
        onPauseOpened.OnEventRaised -= PlayPauseMusic;
    }

    private void PlayMusic()
    {
        playMusicOn.RaisePlayEvent(thisSceneSO.musicTrack, audioConfig);
    }

    private void PlayPauseMusic(bool open)
    {
        if (open)
            playMusicOn.RaisePlayEvent(pauseMusic, audioConfig);
        else
            PlayMusic();
    }
}