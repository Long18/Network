using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [Header("Configs")] [SerializeField] protected AudioCueEventChannelSO sfxEventChannel = default;
    [SerializeField] protected AudioConfigurationSO audioConfig = default;
    [SerializeField] protected GameStateSO gameState = default;

    protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
        Vector3 positionInSpace = default)
    {
        if (gameState.CurrentGameState != GameState.Cutscene)
            sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
    }
}