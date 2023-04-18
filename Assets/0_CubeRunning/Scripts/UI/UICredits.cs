using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CreditsList
{
    public List<ContributerProfile> Contributors = new List<ContributerProfile>();
}

[Serializable]
public class ContributerProfile
{
    public string Name;
    public string Contribution;

    public override string ToString()
    {
        return Name + " - " + Contribution;
    }
}

public class UICredits : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReaderSO = default;
    [SerializeField] private TextAsset creditsAsset = default;
    [SerializeField] private TextMeshProUGUI creditsText = default;
    [SerializeField] private UICreditsRoller creditsRoller = default;

    [Header("Listening on")] [SerializeField]
    private VoidEventChannelSO creditsRollEndEvent = default;

    public UnityAction OnCloseCredits;

    private CreditsList _creditsList;

    private void OnEnable()
    {
        inputReaderSO.MenuCloseEvent += CloseCreditsScreen;
        SetCreditsScreen();
    }

    private void OnDisable()
    {
        inputReaderSO.MenuCloseEvent -= CloseCreditsScreen;
    }

    private void SetCreditsScreen()
    {
        creditsRoller.OnRollingEnded += EndRolling;
        FillCreditsRoller();
        creditsRoller.StartRolling();
    }

    public void CloseCreditsScreen()
    {
        creditsRoller.OnRollingEnded -= EndRolling;
        OnCloseCredits.Invoke();
    }

    private void FillCreditsRoller()
    {
        _creditsList = new CreditsList();
        string json = creditsAsset.text;
        _creditsList = JsonUtility.FromJson<CreditsList>(json);
        SetCreditsText();
    }

    private void SetCreditsText()
    {
        string text = "";
        for (int i = 0; i < _creditsList.Contributors.Count; i++)
        {
            if (i == 0)
                text = text + _creditsList.Contributors[i].ToString();
            else
                text = text + "\n" + _creditsList.Contributors[i].ToString();
        }

        creditsText.text = text;
    }

    private void EndRolling() => creditsRollEndEvent.RaiseEvent();
}