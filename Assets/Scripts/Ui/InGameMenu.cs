using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject endPanel;
    // Start is called before the first frame update
    void Start()
    {
        HidePanels();
    }   
    //Hides all the game panels and continues the game
    public void HidePanels()
    {
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;

        //Locks the cursor for smoother movement
        Cursor.lockState = CursorLockMode.Locked;
    }
    //If the panel exists shows it and pauses the game
    public void OpenPanel(string panelName)
    {
        HidePanels();
        if (panelName == "Pause")
            pausePanel.SetActive(true);
        else if (panelName == "End")
            endPanel.SetActive(true);
        else
            return;
        //Pauses the game
        Time.timeScale = 0;
        //Makes the cursor visible for using the menu
        Cursor.visible = true;

        //Unlocks the cursor for using the menu
        Cursor.lockState = CursorLockMode.None;
    }
    //Restart the current scene
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Open the scene with the declared build index
    public void OpenScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
