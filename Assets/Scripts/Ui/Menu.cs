using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Objects")]
    public Text difficultyText;
    public Dropdown difficultyDP;
    public Slider volumeSlider;

    [Header("SettingsValues")]
    public static Difficulty difficulty;//The games difficulty
    public static float volume;//The audio volume of the game 


    // Start is called before the first frame update
    void Start()
    {
        //Displays the main menu screen at start
        DisplayScreen(GameObject.Find("MenuScreen"));

        difficultyDP.value =(int) difficulty;
        //Resets the volume value
        if (volume == 0)
            volume = 50;
        volumeSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Display the screen which matches the screen name value
    public void DisplayScreen(GameObject screen)
    {
        HideAllScreens();
        screen.SetActive(true);
    }
    //Hides all the game objects under this transform
    public void HideAllScreens()
    {
        foreach (Transform screen in transform)
            screen.gameObject.SetActive(false);
    }
    //Load the scene with the declered scene build
    public void LoadScene(int sceneBuild)
    {
        SceneManager.LoadScene(sceneBuild);
    }
    //Closes the game 
    public void QuitGame()
    {
        Application.Quit();
    }
    //Changes the difficulty value and difficulty text based on the dropdown value
    public void ChangeDifficulty(Dropdown sender)
    {
        difficulty = (Difficulty)sender.value;
        if ((int)difficulty == 0)
            difficultyText.text = "For stealth beginners\nLong time for detection\nlow damaging enemies";
        else if((int)difficulty ==1)
            difficultyText.text = "For more experinced stealth players\nMedium time for detection\nMedium damaging enemies";
        else
            difficultyText.text = "For the expert stealth players\nShort time for detection\nGame lost upon detection";
    }
    //Changes the volume value based on the slider value
    public void ChangeVolume(Slider sender)
    {
        volume = sender.value;
    }
}
public enum Difficulty
{
    Easy=0,
    Medium=1,
    Hard=2
}
