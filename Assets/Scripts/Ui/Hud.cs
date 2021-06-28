using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    [Header("Objects")]
    public GameObject player;
    public Transform enemies;
    public GameEngine gameEngine;
    public Text ammo;
    public Text health;
    public RectTransform detectionMeter;
    public Text instructions;
    public Text alert;

    [Header("Icons")]
    public GameObject standingIcon;
    public GameObject crouchIcon;
    public GameObject itemTakenIcon;
    public GameObject itemNotTakenIcon;

    float detectionValue;
    float width;//The width of the detection meter
    bool isCrouched;//Is the player crouched
    bool itemTaken;
    bool keyTaken;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemies = GameObject.Find("Enemies").transform;
        gameEngine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        //Displays the players stats in hud if he is alive
        if(player!=null&&player.GetComponent<HealthEngine>().HP>0)
        {
            ammo.text = player.GetComponent<PlayerEngine>().ammo.ToString();
            health.text = player.GetComponent<HealthEngine>().HP.ToString();
        }

        //Gets the highest detection value and applies it to detection meter
        if(enemies.childCount>0)
        {
            detectionValue = gameEngine.HighestDetector().GetComponent<EnemyEngine>().detectionValue;
            width = detectionValue >= 1 ? 100 : detectionValue * 100;
            detectionMeter.sizeDelta = new Vector2(width, 10);
        }
        //Displays the stand or crouch icon depending on the player state
        isCrouched = player.GetComponent<PlayerMovement>().isCrouching;
        standingIcon.SetActive(!isCrouched);
        crouchIcon.SetActive(isCrouched);

        //Displays the relevent icon to inform the player if the item was taken
        itemTaken= player.GetComponent<PlayerEngine>().itemTaken;
        itemNotTakenIcon.SetActive(!itemTaken);
        itemTakenIcon.SetActive(itemTaken);

        //Displays the relevent instruction
        keyTaken = player.GetComponent<PlayerEngine>().keyTaken;
        if (!keyTaken)
            instructions.text = "Find the key";
        else if (!itemTaken)
            instructions.text = "Find the Crate";
        else
            instructions.text = "Get out";
    }

    //Displays alert in the alert object
    public void DisplayAlert(string text)
    {
        alert.text = text;
    }
}
