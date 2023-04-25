using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private NPCMovementConfigSO npcMovementConfig;
    [SerializeField] private NPCMovementEventChannelSO channel;

    private void OnEnable()
    {
        if (channel != null) channel.OnEventRaised += Respond;
    }

    private void Respond(NPCMovementConfigSO value) => npcMovementConfig = value;
}