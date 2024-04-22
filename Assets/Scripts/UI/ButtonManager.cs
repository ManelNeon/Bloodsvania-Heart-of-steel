using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

//Script that contains all of the button functionality, will get big very fast
public class ButtonManager : MonoBehaviour
{
    //getting the player script
    [SerializeField] Player player;

    //getting the player controller, the movement controller
    [SerializeField] PlayerController playerController;

    //the pause ui description
    [SerializeField] GameObject pauseUI;

    [SerializeField] GameObject tutorialUI;

    //similar header for all the corresponding UI's
    [Header("Stats UI Configuration")]

    //we get the whole menu
    [SerializeField] GameObject statsMenu;

    //and the image for the buttons
    [SerializeField] Image statsButton;

    [Header("Specials UI Configuration")]

    [SerializeField] GameObject specialsMenu;

    [SerializeField] Image specialsButton;

    //for the specials and the inventory we also get the description box
    [SerializeField] GameObject specialsDescription;

    //and the back button, this back button should only activate if the player is in the fight scene
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

    //the shop UI acts a bit different in the following code, because it can only be activated when the player talks to merchants, therefore there's no need to constantly deactivate it
    [Header("Shop UI Configuration")]

    [SerializeField] GameObject shopMenu;

    [SerializeField] GameObject shopDescription;

    [Header("Save UI Configuration")]

    [SerializeField] GameObject saveWarningDisplay;

    //this is the color the buttons assume when we click on them
    Color buttonPressedColor = new Color(1, .81f, .19f);

    //in here we resume our game
    public void ResumeGame()
    {
        //we get the disasbling hand function from the game manager
        GameManager.Instance.DisablingHand();

        //we firstly deactivate the back buttons
        backButtonSpecials.SetActive(false);
        backButtonItems.SetActive(false);

        //then the description boxs
        specialsDescription.SetActive(false);
        itemsDescription.SetActive(false);
        questsDescription.SetActive(false);
        shopDescription.SetActive(false);

        //then the menus
        questsMenu.SetActive(false);
        specialsMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        statsMenu.SetActive(false);
        shopMenu.SetActive(false);
        saveWarningDisplay.SetActive(false);

        //then we put the button's colors back to normal
        statsButton.color = Color.white;
        specialsButton.color = Color.white;
        inventoryButton.color = Color.white;
        questsButton.color = Color.white;

        //and finally we deactivate the UI
        pauseUI.SetActive(false);

        //if the player is not on the fight scene, we deactivate the cursor
        if (!GameManager.Instance.playerTurn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //for the main menu press, we only restart the scene
    public void MainMenuPress()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseTutorial()
    {
        tutorialUI.SetActive(false);
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SaveGamePress()
    {
        string path = Application.persistentDataPath + "/savePlayerData.json";

        if (File.Exists(path))
        {
            saveWarningDisplay.SetActive(true);

            Time.timeScale = 0;
        }
        else
        {
            DataManager.Instance.SavePlayerData();
        }
    }

    public void NoSaveGame()
    {
        saveWarningDisplay.SetActive(false);

        Time.timeScale = 1;
    }

    public void SaveSaveGame()
    {
        DataManager.Instance.SavePlayerData();

        ResumeGame();

        saveWarningDisplay.SetActive(false);

        Time.timeScale = 1;
    }

    //for when we click stats on the pause menu we
    public void StatsPausePress()
    {
        //deactivate the description boxs
        specialsDescription.SetActive(false);
        questsDescription.SetActive(false);
        itemsDescription.SetActive(false);

        //we deactivate the menus
        questsMenu.SetActive(false);
        specialsMenu.SetActive(false);
        inventoryMenu.SetActive(false);

        //we set the buttons colors to white
        questsButton.color = Color.white;
        specialsButton.color = Color.white;
        inventoryButton.color = Color.white;

        //and in here we set the buttons color to the color we defined for the buttons and we activate the stat menus
        statsButton.color = buttonPressedColor;
        statsMenu.SetActive(true);
    }

    //for the specials button we do the same as in the stats, this time for the specials
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

    //the inventory the same
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

    //and the quests the same
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

    //this code is for when we deactivate the specials, clicking on the back button on the player fighting turn
    public void DeactivateSpecials()
    {
        GameManager.Instance.DisablingHand();
        backButtonSpecials.SetActive(false);
        specialsDescription.SetActive(false);
        specialsMenu.SetActive(false);
    }

    //the same code this time for deactivating the inventory
    public void DeactivateInventory()
    {
        GameManager.Instance.DisablingHand();
        backButtonItems.SetActive(false);
        itemsDescription.SetActive(false);
        inventoryMenu.SetActive(false);
    }

    //in here we deactivate the shop, only possible with the back button, or when the player tries to buy a item
    public void DeactivateShop()
    {
        GameManager.Instance.DisablingHand();
        shopDescription.SetActive(false);
        shopMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //in here we reactivate the players control of the character as we do not intend for him to keep moving when hes in the menus
        playerController.enabled = true;
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
