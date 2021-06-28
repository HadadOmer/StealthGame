using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditorInternal;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    Animator animator;
    [Header("Values")]
    public float movementSpeed;//The speed this charachter moves

    [Header("State")]
    public bool isCrouching;
    // Start is called before the first frame update
    void Start()
    {      
        animator = GetComponent<Animator>();
        //If this object doesnt have animtor destroys this script to prevent erros
        if (animator == null)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isCrouching",isCrouching);
    }
    //Moves the object based on the given values
    public void Move(float x,float z,bool isRunning)
    {
        //Sets the relevent animator bool values
        animator.SetBool("isWalking", Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0);
        animator.SetBool("isRunning", isRunning);

        //The speed is based on the base movement speed,delta time,is running and is crouching
        float speed = movementSpeed * Time.deltaTime*
                    (isRunning?3:1)*(isCrouching?0.5f:1);
              
        transform.Translate(new Vector3(x*speed ,0, z * speed));
    }
    
    //Changes the crouched value
    public void ToggleCrouched()
    {
        isCrouching = !isCrouching;
    }

    //Triggers this object fire animation
    public void FireAnimation()
    {
        animator.SetTrigger("Fire");
    }

    //Copies the animator values from this object's animator to the desired animator
    public void CopyAnimatorValues(Animator animatorCopy)
    {
       
        animatorCopy.runtimeAnimatorController = animator.runtimeAnimatorController;

        animatorCopy.SetBool("isWalking", animator.GetBool("isWalking"));
        animatorCopy.SetBool("isRunning", animator.GetBool("isRunning"));
        animatorCopy.SetBool("isCrouching", animator.GetBool("isCrouching"));
    }
}
