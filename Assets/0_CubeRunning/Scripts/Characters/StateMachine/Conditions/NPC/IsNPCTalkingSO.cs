﻿using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machines/Conditions/Is NPC Talking")]
public class IsNPCTalkingSO : StateConditionSO<IsNPCTalkingCondition>
{
}

public class IsNPCTalkingCondition : Condition
{
    //Component references
    private NPC _npcScript;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        _npcScript = stateMachine.GetComponent<NPC>();
    }

    protected override bool Statement()
    {
        if (_npcScript.npcState == NPCState.Talk)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}