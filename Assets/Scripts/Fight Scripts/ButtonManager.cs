using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Script that contains all of the button functionality, will get big very fast
public class ButtonManager : MonoBehaviour
{
    //getting the player script
    [SerializeField] Player player;

    //the pause ui description
    [SerializeField] GameObject pauseUI;

    [Header("Stats UI Configuration")]

    [SerializeField] GameObject statsMenu;

    [SerializeField] Image statsButton;

    [Header("Specials UI Configuration")]

    [SerializeField] GameObject specialsMenu;

    [SerializeField] Image specialsButton;

    [SerializeField] GameObject specialsDescription;

    [SerializeField] GameObject backButtonSpecials;

    [Header("Inventory UI Configuration")]

    [SerializeField] GameObject inventoryMenu;

    [SerializeField] Image inventoryButton;

    [SerializeField] GameObject itemsDescription;

    [SerializeField] GameObject backButtonItems;

    [Header("Quests UI Configuration")]

    [SerializeField] GameObject questsMenu;

    [SerializeField] Image questsButton;

    [SerializeField] GameObject questsDescription;

    Color buttonPressedColor = new Color(1, .81f, .19f);

    public void ResumeGame()
    {
        GameManager.Instance.DisablingHand();

        questsDescription.SetActive(false);
        backButtonSpecials.SetActive(false);
        backButtonItems.SetActive(false);

        specialsDescription.SetActive(false);
        itemsDescription.SetActive(false);

        questsMenu.SetActive(false);
        specialsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        statsMenu.SetActive(false);

        statsButton.color = Color.white;
        specialsButton.color = Color.white;
        inventoryButton.color = Color.white;
        questsButton.color = Color.white;

        pauseUI.SetActive(false);
    }

    public void StatsPausePress()
    {
        specialsDescription.SetActive(false);
        questsDescription.SetActive(false);
        itemsDescription.SetActive(false);

        questsMenu.SetActive(false);
        specialsMenu.SetActive(false);
        inventoryMenu.SetActive(false);

        questsButton.color = Color.white;
        specialsButton.color = Color.white;
        inventoryButton.color = Color.white;

        statsButton.color = buttonPressedColor;
        statsMenu.SetActive(true);
    }

    public void SpecialsPausePress()
    {
        itemsDescription.SetActive(false);
        questsDescription.SetActive(false);

        inventoryMenu.SetActive(false);
        statsMenu.SetActive(false);
        questsMenu.SetActive(false);

        statsButton.color = Color.white;
        inventoryButton.color = Color.white;
        questsButton.color = Color.white;

        specialsButton.color = buttonPressedColor;
        specialsMenu.SetActive(true);
    }

    public void InventoryPausePress()
    {
        specialsDescription.SetActive(false);
        questsDescription.SetActive(false);

        specialsMenu.SetActive(false);
        questsMenu.SetActive(false);
        statsMenu.SetActive(false);

        statsButton.color = Color.white;
        specialsButton.color = Color.white;
        questsButton.color = Color.white;

        inventoryButton.color = buttonPressedColor;
        inventoryMenu.SetActive(true);
    }

    public void QuestsPausePress()
    {
        specialsDescription.SetActive(false);
        itemsDescription.SetActive(false);

        specialsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        statsMenu.SetActive(false);

        statsButton.color = Color.white;
        specialsButton.color = Color.white;
        inventoryButton.color = Color.white;

        questsButton.color = buttonPressedColor;
        questsMenu.SetActive(true);
    }

    public void DeactivateSpecials()
    {
        GameManager.Instance.DisablingHand();
        backButtonSpecials.SetActive(false);
        specialsDescription.SetActive(false);
        specialsMenu.SetActive(false);
    }

    public void DeactivateInventory()
    {
        GameManager.Instance.DisablingHand();
        backButtonItems.SetActive(false);
        itemsDescription.SetActive(false);
        inventoryMenu.SetActive(false);
    }

    //code that plays when you select the items
    public void ItemsSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            specialsDescription.SetActive(false);
            specialsMenu.SetActive(false);
            backButtonSpecials.SetActive(false);
            backButtonItems.SetActive(true);
            inventoryMenu.SetActive(true);
        }
    }

    //code that plays when you select the specials
    public void SpecialsSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            itemsDescription.SetActive(false);
            inventoryMenu.SetActive(false);
            backButtonItems.SetActive(false);
            backButtonSpecials.SetActive(true);
            specialsMenu.SetActive(true);
        }
    }

    //when we select the attack on the attack screen this plays, sets the UI to inavctive in case the player selected specials but didnt commit to it
    public void AttackSelected()
    {
        if (GameManager.Instance.playerTurn)
        {
            ResumeGame();
            //starting the coroutine for the player attacking the enemy
            GameManager.Instance.StartCoroutine(GameManager.Instance.AttackEnemy());
        }
    }
}
