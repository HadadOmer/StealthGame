using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEngine : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject head;
    public Camera playerCamera;
    public InGameMenu gameMenu;
    public GameEngine gameEngine;
   
    [Header("Components")]
    public PlayerMovement moveScript;
    public HealthEngine health;
    public GunEngine handgun;

    [Header("HeadRotation")]
    public float rotationSpeed;//The speed of the look movement
    public float xRotation;
    public float yRotation;

    [Header("State")]
    public bool isRunning;
    public float shootingCooldown;//The time between player's shots
    public bool keyTaken;
    public bool itemTaken;

    [Header("Values")]
    public int ammo;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu = GameObject.Find("Canvas").GetComponent<InGameMenu>();
        gameEngine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
        //Resets the fire location to center of the player camera
        xRotation = transform.localEulerAngles.x;
        yRotation = 0;

        //Sets the ammo amount based on difficulty
        ammo = 4 - 2 * (int)Menu.difficulty;

    }

    // Update is called once per frame
    void Update()
    {
               
        //Prevents the player from inserting input after death
        if (health.alive)
            InputEnabled();
        else
        {
            MoveCameraToHead();
            gameEngine.EndGame(4.5f);
            Destroy(this);
        }
        
    }
    void InputEnabled()
    {
        //Sets is running true as long as the player presses shift
        isRunning = Input.GetKey(KeyCode.LeftShift);

        //Toggles the crouch on and off on player pressing c
        if (Input.GetKeyDown(KeyCode.C))
            moveScript.ToggleCrouched();

        //Fire the handgun
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isRunning && shootingCooldown <= 0&&ammo>0)
        {
            Fire();
            shootingCooldown = 1.5f;
        }
        shootingCooldown -= Time.deltaTime;

        RotationInput();
        MovementInput();

        //Opens the pause menu when the player presses escape
        if (Input.GetKeyDown(KeyCode.Escape))
            gameMenu.OpenPanel("Pause");


        //Keeps the camera in front of the player face
        playerCamera.transform.position = head.transform.position;
    }
    void RotationInput()
    {
        float rotateX = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
        float rotateY = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;

        //Defines camera rotation on x axis
        xRotation = Mathf.Clamp(xRotation + rotateX, -70, 60);

        //Defines camera rotation on the y axis
        yRotation = yRotation + rotateY;

        transform.eulerAngles = new Vector3(0, yRotation, 0);

        playerCamera.transform.localEulerAngles = new Vector3(xRotation, 0);
        head.transform.localEulerAngles = new Vector3(xRotation,0);
    }

    void MovementInput()
    {
        float x= Input.GetAxis("Horizontal")
            , z= Input.GetAxis("Vertical");
        
        moveScript.Move(x, z, isRunning);
    }

    void MoveCameraToHead()
    {
        //Moves the camera to behind the head view 
        Camera deathCamera;
        deathCamera=Instantiate(playerCamera, head.transform);
        deathCamera.transform.localPosition = new Vector3(0, 0, -1);
        Destroy(playerCamera.gameObject);
    }

    void Fire()
    {
        //Takes down one from the ammo count
        ammo--;
        //Enebles the fire animation on the player and the gun
        handgun.Fire(playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0))
               , playerCamera.transform.forward);
        moveScript.FireAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player is at the gate and he has the item the game is won
        if (other.CompareTag("Gate") && itemTaken)
        {
            GameWon();
        }
    }

    public void GameWon()
    {
        //Enables the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //Loads the credits scene
        SceneManager.LoadScene(2);
    }
}
