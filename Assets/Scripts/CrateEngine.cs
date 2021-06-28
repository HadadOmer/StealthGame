using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateEngine : MonoBehaviour
{
    public Hud hud;
    public bool opened;
    private void Awake()
    {
        hud = GameObject.Find("Hud").GetComponent<Hud>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //If the player is near the crate displays the relevant alert
        if (other.tag == "Player")
        {
            if(opened)
                hud.DisplayAlert("Item Taken");
            else if (other.GetComponent<PlayerEngine>().keyTaken)
            {
                hud.DisplayAlert("Press 'E' to open the crate");
            }
            else
                hud.DisplayAlert("You need the key to open the crate");
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Changes the alert if the player takes the item
        if (other.tag == "Player"&&
            other.GetComponent<PlayerEngine>().keyTaken && Input.GetKeyDown(KeyCode.E))
        {
            opened = true;
            other.GetComponent<PlayerEngine>().itemTaken = true;
            hud.DisplayAlert("Item Taken");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Deletes the alert if the player is no more near the crate
        hud.DisplayAlert("");
    }
}
