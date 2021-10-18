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
    public GameObject gameOverPanel;
    public GameObject pauseMenu;

    public Text scoreText;
    public Text highScoreText;

    private Rigidbody rb; //As 3D tradition, rigidbody is needed for movement
    private int score; //Every safe step is worth one point
    private int highScore; //High score will be the highest number of steps in one game

    [SerializeField] private float movementSpacer = 1f;

    private TerrainManager tm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        highScore = 0; //0 will be the default score for first time and whatever points players have scored, will replace the previous scores
        gameOverPanel.SetActive(false);
        pauseMenu.SetActive(false);
        tm = GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainManager>();
        if (tm == null)
        {
            print("Error no Terrain Manager Found!");
            Destroy(this);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position = new Vector3(transform.position.x + movementSpacer, transform.position.y, transform.position.z);
            tm.playerPos = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position = new Vector3(transform.position.x - movementSpacer, transform.position.y, transform.position.z);
            tm.playerPos = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + movementSpacer);
            tm.playerPos = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - movementSpacer);
            tm.playerPos = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.position.y <= 1.05f)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * 150);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            gameOverPanel.SetActive(true);
        }

        if (other.gameObject.CompareTag("FloorStep"))
        {

        }
    }

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
}
