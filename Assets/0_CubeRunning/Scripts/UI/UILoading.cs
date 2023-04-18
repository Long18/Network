using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen = default;
    [SerializeField] private Image loadingBar = default;
    [SerializeField] private TextMeshProUGUI loadingText = default;

    [Header("Listening to")] [SerializeField]
    private FloatEventChannelSO loadingProgressChannel = default;

    [SerializeField] private BoolEventChannelSO toggleLoadingScreen = default;

    private void OnEnable()
    {
        toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
        loadingProgressChannel.OnEventRaised += UpdateLoadingBar;
    }

    private void OnDisable()
    {
        toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
        loadingProgressChannel.OnEventRaised -= UpdateLoadingBar;
    }

    private void ToggleLoadingScreen(bool state)
    {
        loadingScreen.gameObject.SetActive(state);
    }

    private void UpdateLoadingBar(float progress)
    {
        loadingBar.fillAmount = progress;
        loadingText.text = (progress * 100).ToString("F0") + "%";
    }
}