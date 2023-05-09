using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInspectorPreview : MonoBehaviour
{
    [SerializeField] private Image previewImage = default;

    public void FillPreview(ItemSO item)
    {
        previewImage.gameObject.SetActive(true);
        previewImage.sprite = item.PreviewImage;
    }
}