using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class UISettingItemFiller : MonoBehaviour
{
    [SerializeField] private SettingFieldType fieldType = default;
    [SerializeField] private UIPaginationFiller pagination = default;
    [SerializeField] private LocalizeStringEvent currentSelectedOptionEvent = default;
    [SerializeField] private LocalizeStringEvent title = default;
    [SerializeField] private TextMeshProUGUI currentSelectedOptionText = default;
    [SerializeField] private Image background = default;
    [SerializeField] private Color colorSelected = default;
    [SerializeField] private Color colorUnselected = default;
    [SerializeField] private Sprite backgroundSelected = default;
    [SerializeField] private Sprite backgroundUnselected = default;
    [SerializeField] private MultiInputButton buttonNext = default;
    [SerializeField] private MultiInputButton buttonPrevious = default;

    public event UnityAction OnNextOption = delegate { };
    public event UnityAction OnPreviousOption = delegate { };

    public void FillSettings_Localized(int paginationCount, int selectedPaginationIndex, string selectedOption)
    {
        pagination.SetPagination(paginationCount, selectedPaginationIndex);
        title.StringReference.TableEntryReference = fieldType.ToString();
        currentSelectedOptionEvent.StringReference.TableEntryReference = fieldType + "_" + selectedOption;

        currentSelectedOptionEvent.enabled = true;

        buttonNext.interactable = (selectedPaginationIndex < paginationCount - 1);
        buttonPrevious.interactable = (selectedPaginationIndex > 0);
    }

    public void FillSettings(int paginationCount, int selectedPaginationIndex, string selectedOption_int)
    {
        pagination.SetPagination(paginationCount, selectedPaginationIndex);
        title.StringReference.TableEntryReference = fieldType.ToString();

        currentSelectedOptionEvent.enabled = false;
        currentSelectedOptionText.text = selectedOption_int.ToString();

        buttonNext.interactable = (selectedPaginationIndex < paginationCount - 1);
        buttonPrevious.interactable = (selectedPaginationIndex > 0);
    }

    public void SelectItem()
    {
        background.sprite = backgroundSelected;
        title.GetComponent<TextMeshProUGUI>().color = colorSelected;
        currentSelectedOptionText.color = colorSelected;
    }

    public void UnselectItem()
    {
        background.sprite = backgroundUnselected;

        title.GetComponent<TextMeshProUGUI>().color = colorUnselected;
        currentSelectedOptionText.color = colorUnselected;
    }

    public void NextOption() => OnNextOption?.Invoke();
    public void PreviousOption() => OnPreviousOption?.Invoke();

    public void SetNagivetion(MultiInputButton buttonSelectOnDown, MultiInputButton buttonSelectOnUp)
    {
        MultiInputButton[] buttonNavigation = GetComponentsInChildren<MultiInputButton>();
        foreach (MultiInputButton button in buttonNavigation)
        {
            Navigation navigation = new();
            navigation.mode = Navigation.Mode.Explicit;

            if (buttonSelectOnDown != null) navigation.selectOnDown = buttonSelectOnDown;
            if (buttonSelectOnUp != null) navigation.selectOnUp = buttonSelectOnUp;

            navigation.selectOnLeft = button.navigation.selectOnLeft;
            navigation.selectOnRight = button.navigation.selectOnRight;

            button.navigation = navigation;
        }
    }
}