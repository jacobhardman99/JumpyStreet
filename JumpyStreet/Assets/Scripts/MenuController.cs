//////////////////////////////////////////////
//Assignment/Lab/Project: Jumpy Street
//Name: Malcolm Coronado
//Section: 2021FA.SGD.285.2141
//Instructor: Aurore Wold
//Date: 9/15/2021
/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    public GameObject instructionsMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        instructionsMenu.SetActive(false);
    }

    public void OnMainMenuButtonClick()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        instructionsMenu.SetActive(false);
    }

    public void OnCreditsButtonClick()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        instructionsMenu.SetActive(false);
    }

    public void OnControlsButtonClick()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        instructionsMenu.SetActive(false);
    }

    public void OnInstructionsButtonClick()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        instructionsMenu.SetActive(true);
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
