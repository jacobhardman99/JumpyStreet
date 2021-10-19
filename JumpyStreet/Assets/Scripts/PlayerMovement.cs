//////////////////////////////////////////////
//Assignment/Lab/Project: Jumpy Street
//Name: Malcolm Coronado
//Section: 2021FA.SGD.285.2141
//Instructor: Aurore Wold
//Date: 9/15/2021
/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    /*
    public GameObject gameOverPanel;
    public GameObject pauseMenu;

    public Text scoreText;
    public Text highScoreText;
    */
    private Rigidbody rb; //As 3D tradition, rigidbody is needed for movement
    private int score; //Every safe step is worth one point
    private int highScore; //High score will be the highest number of steps in one game

    [SerializeField] private float movementSpacer = 1f;

    private TerrainManager tm;
    private Vector3 raycastOffset = new Vector3(0, 1, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        highScore = 0; //0 will be the default score for first time and whatever points players have scored, will replace the previous scores
        /*
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        */
        tm = GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainManager>();
        if (tm == null)
        {
            print("Error no Terrain Manager Found!");
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + raycastOffset, transform.TransformDirection(Vector3.right), out hit, movementSpacer))
            {
                transform.position = new Vector3(Mathf.RoundToInt(transform.position.x + movementSpacer), transform.position.y, Mathf.RoundToInt(transform.position.z));
                tm.playerPos = transform.position;
            }          
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + raycastOffset, transform.TransformDirection(Vector3.left), out hit, movementSpacer))
            {
                transform.position = new Vector3(Mathf.RoundToInt(transform.position.x - movementSpacer), transform.position.y, Mathf.RoundToInt(transform.position.z));
                tm.playerPos = transform.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + raycastOffset, transform.TransformDirection(Vector3.forward), out hit, movementSpacer))
            {
                transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z + movementSpacer));
                tm.playerPos = transform.position;
            }            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position + raycastOffset, transform.TransformDirection(Vector3.back), out hit, movementSpacer))
            {
                transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z - movementSpacer));
                tm.playerPos = transform.position;
            }            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            Debug.LogWarning("Game Over");
            //gameOverPanel.SetActive(true);
        }

        if (other.gameObject.CompareTag("FloorStep"))
        {

        }
    }
    /*
    public void OnPauseButtonClick()
    {
        pauseMenu.SetActive(true);
    }

    public void OnResumeButtonClick()
    {
        pauseMenu.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("MainGame");
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        score = 0;
    }
    */
}
