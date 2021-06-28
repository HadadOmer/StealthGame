using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEngine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Destroys this object and changes the key taken value in player engine if the player is near this object
        if(other.tag=="Player")
        {
            other.GetComponent<PlayerEngine>().keyTaken = true;
            Destroy(gameObject);
        }
    }
}
