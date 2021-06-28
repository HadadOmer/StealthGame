using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEngine : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform enemies;
    public GameObject player;
    public GameObject playerClone;
    public InGameMenu gameMenu;
    public Transform keySpawns;
    [Header("Prefabs")]
    public GameObject keyPrefab;
    [Header("Values")]
    public Vector3 playerLastKnownLocation;

    [Header("DifficultyValues")]
    public float alertDistance;
    // Start is called before the first frame update
    void Start()
    {       
        player = GameObject.Find("Player");
        playerClone = GameObject.Find("PlayerClone");
        
        playerClone.transform.position = new Vector3(0, -200, 0);

        gameMenu = GameObject.Find("Canvas").GetComponent<InGameMenu>();

        keySpawns = GameObject.Find("KeySpawns").transform;
        SpawnKey();
    }
    void SpawnKey()
    {
        Vector3 position = keySpawns.GetChild(Random.Range(0, 4)).position;
        Instantiate(keyPrefab, position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void NewAlert(Vector3 location)
    {
        //Ends the game if alerted and difficulty is hard
        if (Menu.difficulty == Difficulty.Hard)
            EndGame(0);
        else
        {
            //Moves the last known location clone
            playerLastKnownLocation = location;
            StartCoroutine(MoveClone());

            AlertNearbyEnemies();
        }
    }
    public void AlertNearbyEnemies()
    {
        foreach(Transform enemy in enemies.transform)
            if(Vector3.Distance(enemy.transform.position,playerLastKnownLocation)<alertDistance
                && enemy.GetComponent<EnemyMovement>()!=null)
                enemy.GetComponent<EnemyMovement>().state = EnemyMovement.State.alerted;
    }
    //Returns the enemy with the highest detection value
    public Transform HighestDetector()
    {        
        Transform detector = enemies.GetChild(0);
        float maxValue = detector.GetComponent<EnemyEngine>().detectionValue;
        foreach (Transform enemy in enemies.transform)
            if (maxValue < enemy.GetComponent<EnemyEngine>().detectionValue)
            {
                maxValue = enemy.GetComponent<EnemyEngine>().detectionValue;
                detector = enemy;
            }
        return detector;
    }

    //Copies the location,rotation and animator values of the player to the clone
    public IEnumerator MoveClone()
    {
        playerClone.transform.position = playerLastKnownLocation;
        playerClone.transform.rotation = player.transform.rotation;
        //Copys the player animator values and freezes the clone animator
        Animator cloneAnimator = playerClone.GetComponent<Animator>();              
        player.GetComponent<PlayerMovement>().CopyAnimatorValues(cloneAnimator);

        yield return new WaitForSeconds(1);

        cloneAnimator.speed = 0;        
    }
  
    //Ends the game after the declared delay
    public void EndGame(float delay)
    {
        StartCoroutine(EnumEndGame(delay));
    }
    public IEnumerator EnumEndGame(float delay)
    {
        print("in");
        yield return new WaitForSeconds(delay);
        print("end");
        gameMenu.OpenPanel("End");
    }
}
