using UnityEngine;
using UnityEngine.Serialization;

public class ProtagonistAudio : CharacterAudio
{
    [FormerlySerializedAs("liftoff")] [Header("Audio")] [SerializeField] private AudioCueSO liftJump;
    [SerializeField] private AudioCueSO land;
    [SerializeField] private AudioCueSO footsteps;
    [SerializeField] private AudioCueSO getHit;
    [SerializeField] private AudioCueSO die;

    public void PlayFootstep() => PlayAudio(footsteps, audioConfig, transform.position);
    public void PlayJumpLiftoff() => PlayAudio(liftJump, audioConfig, transform.position);
    public void PlayJumpLand() => PlayAudio(land, audioConfig, transform.position);
    public void PlayGetHit() => PlayAudio(getHit, audioConfig, transform.position);
    public void PlayDie() => PlayAudio(die, audioConfig, transform.position);
}