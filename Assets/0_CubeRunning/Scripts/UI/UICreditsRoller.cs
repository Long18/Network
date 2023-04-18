using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UICreditsRoller : MonoBehaviour
{
    [SerializeField, Tooltip("Speed set to rolling effect")]
    private float speedPreset = 100f; //normal rolling speed 

    [SerializeField, Tooltip("Actual speed of rolling")]
    private float speed = 100f; //actual speed of rolling

    [SerializeField] private bool rollAgain = false;

    [Header("References")] [SerializeField]
    private InputReaderSO inputReader = default;

    [SerializeField] private RectTransform textCredits = default;
    [SerializeField] private RectTransform mask = default;

    public event UnityAction OnRollingEnded;
    private float expectedFinishingPoint;

    public void StartRolling()
    {
        speed = speedPreset;
        StartCoroutine(InitialOffset()); //This offset is needed to get true informations about rectangle and his mask
    }

    private void OnEnable() => inputReader.MoveEvent += OnMove;
    private void OnDisable() => inputReader.MoveEvent -= OnMove;

    private void Update()
    {
        if (textCredits.anchoredPosition.y < expectedFinishingPoint)
            textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x,
                textCredits.anchoredPosition.y + speed * Time.deltaTime);
        else if (expectedFinishingPoint != 0) //this happend when rolling reach to end
            RollingEnd();
    }

    private IEnumerator InitialOffset()
    {
        yield return new WaitForSecondsRealtime(0.02f);

        inputReader.EnableGameplayInput();
        expectedFinishingPoint = (textCredits.rect.height + mask.rect.height) / 2;

        textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x,
            -((textCredits.rect.height + mask.rect.height) / 2));
    }

    private void OnMove(Vector2 direction)
    {
        if (direction.y == 0f) speed = speedPreset;
        else if (direction.y > 0f) speed = speed * 2;
        else speed = -speedPreset;
    }

    private void RollingEnd()
    {
        if (rollAgain)
            textCredits.anchoredPosition = new Vector2(textCredits.anchoredPosition.x,
                -((textCredits.rect.height + mask.rect.height) / 2));
        else
            OnRollingEnded?.Invoke();
    }
}