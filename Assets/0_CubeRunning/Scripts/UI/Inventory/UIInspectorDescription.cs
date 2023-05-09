using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class UIInspectorDescription : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent textDes = default;
    [SerializeField] private TextMeshProUGUI textAmount = default;
    [SerializeField] private LocalizeStringEvent textName = default;

    public void FillDescription(ItemSO item)
    {
        textName.StringReference = item.Name;
        textName.StringReference.Arguments = new[] { new { Purpose = 0, Amount = 1 } };
        textDes.StringReference = item.Description;

        if (item.Amount > 0)
            textAmount.text = "+" + item.Amount;
        else
            textAmount.text = "";
        textName.gameObject.SetActive(true);
        textDes.gameObject.SetActive(true);
    }
}