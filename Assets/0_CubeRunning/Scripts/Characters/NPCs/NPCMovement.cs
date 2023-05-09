using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public NPCMovementConfigSO npcMovementConfig;
    public NPCMovementEventChannelSO channel;

    private void OnEnable()
    {
        if (channel != null) channel.OnEventRaised += Respond;
    }

    private void Respond(NPCMovementConfigSO value) => npcMovementConfig = value;
}