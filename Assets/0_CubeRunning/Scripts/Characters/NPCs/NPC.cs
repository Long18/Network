using System.Collections;
using UnityEngine;

public enum NPCState
{
    Idle = 0,
    Walk = 1,
    Talk = 2
}

public class NPC : MonoBehaviour
{
    public NPCState npcState; //This is checked by conditions in the StateMachine
    public GameObject[] talkingTo;

    public void SwitchToWalkState()
    {
        StartCoroutine(WaitBeforeSwitch());
    }

    private IEnumerator WaitBeforeSwitch()
    {
        int waitTime = Random.Range(0, 3);
        yield return new WaitForSeconds(waitTime);
        npcState = NPCState.Walk;
    }
}