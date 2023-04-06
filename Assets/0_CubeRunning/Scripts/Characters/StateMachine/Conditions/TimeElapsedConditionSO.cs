using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Time elapsed")]
public class TimeElapsedConditionSO : StateConditionSO<TimeElapsedCondition>
{
	public float timerLength = .5f;
}

public class TimeElapsedCondition : Condition
{
	private float startTime;
	private TimeElapsedConditionSO originSO => (TimeElapsedConditionSO)base.OriginSO; // The SO this Condition spawned from

	public override void OnStateEnter()
	{
		startTime = Time.time;
	}

	protected override bool Statement() => Time.time >= startTime + originSO.timerLength;
}
