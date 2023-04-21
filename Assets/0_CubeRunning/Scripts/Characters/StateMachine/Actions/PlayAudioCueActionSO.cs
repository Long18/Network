using System.Collections;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Actions/Play AudioCue")]
public class PlayAudioCueActionSO : StateActionSO<PlayAudioCueAction>
{
    public AudioCueSO audioCue = default;
    public AudioCueEventChannelSO audioCueEventChannel = default;
    public AudioConfigurationSO audioConfiguration = default;
}

public class PlayAudioCueAction : StateAction
{
    private Transform stateMachineTransform;

    private PlayAudioCueActionSO originSO =>
        (PlayAudioCueActionSO)base.OriginSO; // The SO this StateAction spawned from

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        stateMachineTransform = stateMachine.transform;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        originSO.audioCueEventChannel.RaisePlayEvent(originSO.audioCue, originSO.audioConfiguration,
            stateMachineTransform.position);
    }
}