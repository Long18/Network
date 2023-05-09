﻿using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class BoolEvent : UnityEvent<bool, GameObject>
{
}


/// <summary>
/// A generic class for a "zone", that is a trigger collider that can detect if an object of a certain type (layer) entered or exited it.
/// Implements <code>OnTriggerEnter</code> and <code>OnTriggerExit</code> so it needs to be on the same object that holds the Collider.
/// </summary>
public class ZoneTriggerController : MonoBehaviour
{
    [SerializeField] private BoolEvent enterZone = default;
    [SerializeField] private LayerMask layers = default;

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & layers) != 0)
            enterZone.Invoke(true, other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & layers) != 0)
            enterZone.Invoke(false, other.gameObject);
    }
}