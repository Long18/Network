﻿using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "HorizontalMove", menuName = "State Machines/Actions/Horizontal Move")]
public class HorizontalMoveActionSO : StateActionSO<HorizontalMoveAction>
{
	[Tooltip("Horizontal XZ plane speed multiplier")]
	public float speed = 8f;
}

public class HorizontalMoveAction : StateAction
{
	//Component references
	private Protagonist _protagonistScript;
	private HorizontalMoveActionSO _originSO => (HorizontalMoveActionSO)base.OriginSO; // The SO this StateAction spawned from

	public override void Awake(StateMachine.StateMachine stateMachine)
	{
		_protagonistScript = stateMachine.GetComponent<Protagonist>();
	}

	public override void OnUpdate()
	{
		_protagonistScript.movementVector.x = _protagonistScript.movementInput.x * _originSO.speed;
		_protagonistScript.movementVector.z = _protagonistScript.movementInput.z * _originSO.speed;
	}
}
