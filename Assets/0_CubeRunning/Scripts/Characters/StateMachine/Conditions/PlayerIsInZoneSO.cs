using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;


public enum ZoneType
{
    Alert,
    Attack
}

[CreateAssetMenu(fileName = "PlayerIsInZone", menuName = "State Machines/Conditions/Player Is In Zone")]
public class PlayerIsInZoneSO : StateConditionSO
{
    public ZoneType zone;

    protected override Condition CreateCondition() => new PlayerIsInZone();
}

public class PlayerIsInZone : Condition
{
    private Critter critter;

    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        critter = stateMachine.GetComponent<Critter>();
    }

    protected override bool Statement()
    {
        bool result = false;
        if (critter != null)
        {
            switch (((PlayerIsInZoneSO)OriginSO).zone)
            {
                case ZoneType.Alert:
                    result = critter.isPlayerInAlertZone;
                    break;
                case ZoneType.Attack:
                    result = critter.isPlayerInAttackZone;
                    break;
                default:
                    break;
            }
        }

        return result;
    }
}