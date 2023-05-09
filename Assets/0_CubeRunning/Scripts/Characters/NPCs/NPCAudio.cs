using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudio : CharacterAudio
{
    [SerializeField] private AudioCueSO talk, footstep;

    //when we have the ground detector script, we should check the type to know which footstep sound to play
    public void PlayFootstep() => PlayAudio(footstep, audioConfig, transform.position);
    public void PlayTalk() => PlayAudio(talk, audioConfig, transform.position);
}