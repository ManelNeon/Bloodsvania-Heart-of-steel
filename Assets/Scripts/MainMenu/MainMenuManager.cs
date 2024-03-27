using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;

    [SerializeField] GameObject game;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NewGameButton()
    {
        mainMenu.SetActive(false);
        game.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadGameButton()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
