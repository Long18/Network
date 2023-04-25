using UnityEngine;
using UnityEngine.Serialization;

public class NPCMovementConfigSO : ScriptableObject
{
    [SerializeField] private float stopDuration;
    [SerializeField] private float speed;

    public float Speed => speed;
    public float StopDuration => stopDuration;
}