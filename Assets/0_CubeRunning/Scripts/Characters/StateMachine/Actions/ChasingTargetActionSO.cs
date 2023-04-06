using UnityEngine;
using UnityEngine.AI;
using StateMachine;
using StateMachine.ScriptableObjects;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ChasingTargetAction", menuName = "State Machines/Actions/Chasing Target Action")]
public class ChasingTargetActionSO : StateActionSO
{
    [FormerlySerializedAs("_targetTransform")] [Tooltip("Target transform anchor.")] [SerializeField]
    private TransformAnchor targetTransform = default;

    [FormerlySerializedAs("_chasingSpeed")] [Tooltip("NPC chasing speed")] [SerializeField]
    private float chasingSpeed = default;

    public Vector3 TargetPosition => targetTransform.Value.position;
    public float ChasingSpeed => chasingSpeed;

    protected override StateAction CreateAction() => new ChasingTargetAction();
}

public class ChasingTargetAction : StateAction
{
    private Critter critter;
    private ChasingTargetActionSO config;
    private NavMeshAgent agent;
    private bool isActiveAgent;


    public override void Awake(StateMachine.StateMachine stateMachine)
    {
        config = (ChasingTargetActionSO)OriginSO;
        agent = stateMachine.gameObject.GetComponent<NavMeshAgent>();
        isActiveAgent = agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh;
    }

    public override void OnUpdate()
    {
        if (isActiveAgent)
        {
            agent.isStopped = false;
            agent.SetDestination(config.TargetPosition);
        }
    }

    public override void OnStateEnter()
    {
        if (isActiveAgent)
        {
            agent.speed = config.ChasingSpeed;
        }
    }
}