using UnityEngine;
using StateMachine;
using StateMachine.ScriptableObjects;

[CreateAssetMenu(fileName = "DropReward", menuName = "State Machines/Actions/Drop Reward")]
public class DropRewardSO : StateActionSO
{
    protected override StateAction CreateAction() => new DropReward();
}

public class DropReward : StateAction
{
    private DroppableRewardConfigSO dropRewardConfig;
    private Transform currentTransform;


    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        dropRewardConfig = stateMachine.GetComponent<Damageable>().DroppableRewardConfig;
        currentTransform = stateMachine.transform;
    }

    public override void OnUpdate()
    {
    }

    public override void OnStateEnter()
    {
        DropAllRewards(currentTransform.position);
    }

    private void DropAllRewards(Vector3 position)
    {
        DropGroup specialDropItem = dropRewardConfig.DropSpecialItem();
        if (specialDropItem != null) // drops a special item if any 
            DropOneReward(specialDropItem, position);
        // Drop items
        foreach (DropGroup dropGroup in dropRewardConfig.DropGroups)
        {
            float randValue = Random.value;
            if (dropGroup.DropRate >= randValue)
            {
                DropOneReward(dropGroup, position);
            }
            else
            {
                break;
            }
        }
    }

    private void DropOneReward(DropGroup dropGroup, Vector3 position)
    {
        float dropDice = Random.value;
        float _currentRate = 0.0f;

        ItemSO item = null;
        GameObject itemPrefab = null;

        foreach (DropItem dropItem in dropGroup.Drops)
        {
            _currentRate += dropItem.ItemDropRate;
            if (_currentRate >= dropDice)
            {
                item = dropItem.Item;
                itemPrefab = dropItem.Item.Prefab;
                break;
            }
        }

        float randAngle = Random.value * Mathf.PI * 2;
        GameObject collectibleItem = GameObject.Instantiate(itemPrefab,
            position + itemPrefab.transform.localPosition +
            dropRewardConfig.ScatteringDistance *
            (Mathf.Cos(randAngle) * Vector3.forward + Mathf.Sin(randAngle) * Vector3.right),
            Quaternion.identity);
        collectibleItem.GetComponent<CollectableItem>().AnimateItem();
    }
}