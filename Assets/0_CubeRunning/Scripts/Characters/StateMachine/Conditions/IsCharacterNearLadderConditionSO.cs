using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine;

public class IsCharacterNearLadderConditionSO : StateConditionSO<IsCharacterNearLadderCondition>
{
}

public class IsCharacterNearLadderCondition : Condition
{
    private Protagonist protagonist;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        protagonist = stateMachine.GetComponent<Protagonist>();
    }

    protected override bool Statement()
    {
        if (protagonist.boxCollider.gameObject.CompareTag("Ladder"))
        {
            // Debug.Log("IsCharacterNearLadderCondition: true");
            return true;
        }
        // if (protagonist.lastHit.collider.gameObject.CompareTag("Ladder"))
        else
        {
            // Debug.Log("IsCharacterNearLadderCondition: false");
            return false;
        }
    }
}