using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPaginationFiller : MonoBehaviour
{
    [SerializeField] private Image imagePaginationPrefab = default;
    [SerializeField] private Transform parentPagination = default;

    [SerializeField] private Sprite emptyPagination = default;
    [SerializeField] private Sprite filledPagination = default;

    HorizontalLayoutGroup horizontalLayout = default;

    private List<Image> instantiatedImages = default;
    [SerializeField] private int maxSpacing = 10;
    [SerializeField] private int minSpacing = 1;

    public void SetPagination(int paginationCount, int selectedPaginationIndex)
    {
        if (instantiatedImages == null) instantiatedImages = new List<Image>();

        int maxCount = Mathf.Max(paginationCount, instantiatedImages.Count);

        if (maxCount <= 0) return;

        for (int i = 0; i < maxCount; i++)
        {
            if (i >= instantiatedImages.Count)
            {
                Image instantiatedImage = Instantiate(imagePaginationPrefab, parentPagination);
                instantiatedImages.Add(instantiatedImage);
            }

            if (i < paginationCount)
            {
                instantiatedImages[i].gameObject.SetActive(true);
            }
            else
            {
                instantiatedImages[i].gameObject.SetActive(false);
            }
        }

        SetCurrentPagination(selectedPaginationIndex);

        horizontalLayout = GetComponent<HorizontalLayoutGroup>();
        if (paginationCount < 10)
        {
            horizontalLayout.spacing = maxSpacing;
        }
        else if (paginationCount >= 10 && paginationCount < 20)
        {
            horizontalLayout.spacing = (maxSpacing - minSpacing) / 2;
        }
        else if (paginationCount >= 20 && paginationCount < 30)
        {
            horizontalLayout.spacing = minSpacing;
        }
    }

    public void SetCurrentPagination(int selectedPaginationIndex)
    {
        if (instantiatedImages.Count <= selectedPaginationIndex) return;
        for (int i = 0; i < instantiatedImages.Count; i++)
        {
            if (i == selectedPaginationIndex)
            {
                instantiatedImages[i].sprite = filledPagination;
            }
            else
            {
                instantiatedImages[i].sprite = emptyPagination;
            }
        }
    }
}