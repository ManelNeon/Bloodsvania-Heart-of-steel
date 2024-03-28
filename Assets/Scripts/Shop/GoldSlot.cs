using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//a gold slot in the inventory that tells the player how much gold he has
public class GoldSlot : MonoBehaviour, IPointerClickHandler
{
    //getting the player stats where the gold is
    [SerializeField] Player playerStats;

    //getting the quantity in the UI
    [SerializeField] TextMeshProUGUI quantity;

    //getting the gold sprite
    [SerializeField] Sprite goldSprite;

    // ---- Item Description ---- //

    //getting the descritpion box to activate or deactivate it
    [SerializeField] GameObject descriptionBox;

    //getting the description image to put the gold image in it
    [SerializeField] Image descriptionImage;

    //getting the description text to put the description of gold
    [SerializeField] TextMeshProUGUI descriptionText;

    //getting the descriptions item name (gold in this casE)
    [SerializeField] TextMeshProUGUI descriptionTitle;

    //getting the effects of gold, unknwon cus capitalism funny aha
    [SerializeField] TextMeshProUGUI descriptionEffect;

    //when we enable the game object we put the quantity text equal to the players gold
    private void OnEnable()
    {
        quantity.text = playerStats.gold.ToString();
    }

    //if the player clicks on it the description will appear
    public void OnPointerClick(PointerEventData eventData)
    {
        descriptionBox.SetActive(true);

        descriptionImage.sprite = goldSprite;

        descriptionText.text = "These are Gold Pounds, the currency here. Use it to buy items or even some Special Upgrades for yourself!!";

        descriptionTitle.text = "Gold Pounds";

        descriptionEffect.text = "Unknown";
    }
}
