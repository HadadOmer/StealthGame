using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngine : MonoBehaviour
{

    [Header("Components")]
    public Transform player;
    public Transform head;
    public EnemyMovement movement;
    public GunEngine gun;
    public GameEngine gameEngine;

    [Header("Values")]   
    public float fov;
    public float detectionValue;
    Vector3 playerRealLocation;
    Vector3 playerLastKnownLocation;


    // Start is called before the first frame update
    void Start()
    {
        detectionValue = 0;
        gameEngine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
        player = GameObject.Find("Player").transform;

        //Sets the enemy damage based on difficulty
        gun.damage = 25 + 25 * (int)Menu.difficulty;

        //Sets the desired volume
        AudioListener.volume = Menu.volume;
    }

    // Update is called once per frame
    void Update()
    {
        playerRealLocation = player.transform.position;
        SetDetectionValue();

        if (detectionValue >= 5)
            Fire();
        else if (detectionValue >= 1)
        {
            //Sends and alert to the game engine
            gameEngine.NewAlert(playerLastKnownLocation);
        }
    }
    void SetDetectionValue()
    {
        Vector3 playerDir = playerRealLocation - transform.position;
        float angle = Vector3.Angle(playerDir, transform.forward);//The angle between this enemy and the player
        //Increase the detection value only if the player is in enemy's field of view and visible
        if (angle < fov && CheckPlayerVisible(playerDir,out playerLastKnownLocation))
            detectionValue += Time.deltaTime*0.6f *(1+(int)Menu.difficulty);
        //Decrease the detection value if the player isn't in enemy's field of view
        else if (detectionValue > 0)
            detectionValue -= Time.deltaTime ;
        //Keeps the detection value zero or above
        else
            detectionValue = 0;
        
    }
    //Checks if the player isnt behind a solid object which prevents its visiblity
    bool CheckPlayerVisible(Vector3 playerDir,out Vector3 playerPosition)
    {
        RaycastHit hit;
        if(Physics.Raycast(head.position, playerDir, out hit, 20)
                && hit.transform.tag == "Player")
        {
            playerPosition = hit.transform.position;
            return true;
        }
        playerPosition = playerLastKnownLocation;
        return false;      
    }
    
    void Fire()
    {
        //Triggers the fire animation
        StartCoroutine(movement.FireAnimation());
        //Fires at the player
        Vector3 direction = playerLastKnownLocation - transform.position;
        gun.Fire(transform.position,direction);
        //Changes the detection to create time gap between shots
        detectionValue = 1.1f;
    }
}
