using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;
    public GameEngine gameEngine;
    public HealthEngine health;

    [Header("State")]
    public State state;   
    public bool toPatrolEnd;//is the enemy moves to the patrol end point

    [Header("Values")]
    public Vector3 patrolStart;
    public Vector3 patrolEnd;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the relevant components
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<HealthEngine>();

        gameEngine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
        //Resets the start values
        state = State.patroling;
        patrolStart = transform.position;
        agent.SetDestination(patrolEnd);       
        toPatrolEnd = true;
    }

    // Update is called once per frame
    void Update()
    {
       //Destorys the nav mesh agent and this script if not alive
        if (!health.alive)
        {
            Destroy(agent);
            Destroy(this);
        }
        else
        {
            //Triggers the relevant animation based on the object state
            animator.SetBool("isWalking", state != State.standing && state != State.firing);

            animator.SetBool("isRunning", state == State.alerted);
            //Sets the relevant values to the nav mesh agent
            agent.speed = state == State.alerted ? 3 : 1;

            agent.isStopped = state == State.standing || state == State.firing;

            
            CheckAlerted();

            
            if (state == State.alerted)
            {
                //Sets the object destination to the player last known location
                agent.SetDestination(gameEngine.playerLastKnownLocation);
            }
            else if (state == State.patroling)
                Patrol();

        }
    }
    //Patrols between the the start position of the object and its end position
    public void Patrol()
    {
        Vector3 position = transform.position;
                if((toPatrolEnd&&Vector3.Distance(position,patrolEnd)<0.7f)||
            (!toPatrolEnd && Vector3.Distance(position, patrolStart) < 0.7f))
        {
            toPatrolEnd = !toPatrolEnd;
            //Stops for 6 seconds at the end of the patrol
            StartCoroutine(PausePatrol(6f));          
        }
    }
    //Checks if the enemy is still alerted
    public void CheckAlerted()
    {
        Vector3 playerLocation = gameEngine.playerLastKnownLocation;
        if (Vector3.Distance(transform.position, playerLocation) < 0.7f&& state == State.alerted)
        { 
            state=State.patroling;
            StartCoroutine(PausePatrol(5f));
        }
    }
    public IEnumerator PausePatrol(float time)
    {
        //Makes the object stand for the declared time and triggers the look animation
        state = State.standing;
        animator.SetTrigger("Look");
        yield return new WaitForSeconds(time);
        //Continue the patrol
        state = State.patroling;
        agent.SetDestination(toPatrolEnd ? patrolEnd : patrolStart);
    }
    //Triggers the fire animation and changes the state to firing for a second
    public IEnumerator FireAnimation()
    {
        state= State.firing;
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(1);
        state = State.alerted;
    }

    public enum State
    {
        standing,
        patroling,
        alerted,
        firing
    }
}
