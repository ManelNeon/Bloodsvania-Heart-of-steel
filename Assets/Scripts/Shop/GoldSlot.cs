using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoldSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Player playerStats;

    [SerializeField] TextMeshProUGUI quantity;

    [SerializeField] Sprite goldSprite;

    // ---- Item Description ---- //

    [SerializeField] GameObject descriptionBox;

    [SerializeField] Image descriptionImage;

    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] TextMeshProUGUI descriptionTitle;

    [SerializeField] TextMeshProUGUI descriptionEffect;

    private void OnEnable()
    {
        quantity.text = playerStats.gold.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        descriptionBox.SetActive(true);

        descriptionImage.sprite = goldSprite;

        descriptionText.text = "These are Gold Pounds, the currency here. Use it to buy items or even some Special Upgrades for yourself!!";

        descriptionTitle.text = "Gold Pounds";

        descriptionEffect.text = "Unknown";
    }
}
