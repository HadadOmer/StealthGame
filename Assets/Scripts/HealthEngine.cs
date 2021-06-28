using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEngine : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;

    [Header("Values")]
    public float HP;
    public bool alive;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        alive = true;
    }
    // Update is called once per frame
    void Update()
    {
        //If this object falls out of the map he is killed
        if (transform.position.y < -10)
            TakeDownHP(HP);
    }
    //Takes down the hp based on the value
    public void TakeDownHP(float value)
    {
        HP -= value;
        if (HP <= 0 )
            Kill();
    }
    public void Kill()
    {
        //Triggers the death animation if there is one
        if (animator != null)
            animator?.SetTrigger("Dead");
        alive = false;
        //Despawns the object after 20 seconds 
        Destroy(gameObject,20);

    }

}
