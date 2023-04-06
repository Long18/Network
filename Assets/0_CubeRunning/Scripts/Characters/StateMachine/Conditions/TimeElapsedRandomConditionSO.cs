using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed random")]
public class TimeElapsedRandomConditionSO : StateConditionSO<TimeElapsedRandomCondition>
{
    public float minTimerLength = .5f;
    public float maxTimerLength = .5f;
}

public class TimeElapsedRandomCondition : Condition
{
    private float startTime;
    private float timerLength = .5f;

    private TimeElapsedRandomConditionSO originSO =>
        (TimeElapsedRandomConditionSO)base.OriginSO; // The SO this Condition spawned from

    public override void OnStateEnter()
    {
        startTime = Time.time;
        timerLength = Random.Range(originSO.minTimerLength, originSO.maxTimerLength);
    }

    protected override bool Statement() => Time.time >= startTime + timerLength;
}