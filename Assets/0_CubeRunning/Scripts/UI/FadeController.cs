using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private FadeChannelSO fadeChannelSO = default;
    [SerializeField] private Image imageComponent = default;

    private void OnEnable() => fadeChannelSO.OnEventRaised += InitiateFade;

    private void OnDisable() => fadeChannelSO.OnEventRaised -= InitiateFade;

    private void InitiateFade(bool fadeIn, float duration, Color desiredColor) =>
        imageComponent.DOBlendableColor(desiredColor, duration);
}