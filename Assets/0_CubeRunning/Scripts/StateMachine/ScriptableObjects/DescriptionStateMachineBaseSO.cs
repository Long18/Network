using UnityEngine;

namespace StateMachine.ScriptableObjects
{
    /// <summary>
    /// Base class for StateMachine ScriptableObjects that need a public description field.
    /// </summary>
    public class DescriptionStateMachineBaseSO : ScriptableObject
    {
        [TextArea] public string description;
    }
}