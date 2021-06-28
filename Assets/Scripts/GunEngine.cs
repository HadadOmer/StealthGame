using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunEngine : MonoBehaviour
{
    [Header("Values")]
    public float damage;//The damage of this gun


    [Header("Components")]
    public Animator animator;
    public AudioSource fireSound;

    RaycastHit hit;

    public void Fire(Vector3 fireLocation,Vector3 direction)
    {      
        //Sets the fire animator trigger if this object has an animator
        if(animator!=null)
            animator.SetTrigger("Fire");
        //Activates the gun sound if this object has fire sound audio source
        if (fireSound != null)
            fireSound.Play();

        if (Physics.Raycast(fireLocation, direction, out hit, 200))         
            //Takes down hp if game object that hit have health engine
            if (hit.transform.GetComponent<HealthEngine>() != null)
            {
                hit.transform.GetComponent<HealthEngine>().TakeDownHP(damage);
            }
    }
}
