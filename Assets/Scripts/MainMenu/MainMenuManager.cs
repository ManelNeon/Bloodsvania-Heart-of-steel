using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//code that manages the main menu
public class MainMenuManager : MonoBehaviour
{
    //getting the main menu object
    [SerializeField] GameObject mainMenu;

    //getting the game object
    [SerializeField] GameObject game;

    //when starting the game, let the player control the mouse
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //when you click New Game, the main menu will deactivate and the game will activate, we deactivate the mouse
    public void NewGameButton()
    {
        mainMenu.SetActive(false);
        game.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //when you click Load Game, we will get the data manager and load the game, we do the same as the new Game Button, with the difference that we load the game first
    public void LoadGameButton()
    {
        //load the game data

        //activate the fight scene

        //deactivate the game scene

        //deactivate the mouse
    }

    //we press the exit button and the application quits
    public void ExitButton()
    {
        Application.Quit();
    }
}
