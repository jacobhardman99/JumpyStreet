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
    public float speed;

    public GameObject gameOverPanel; 

    public Text scoreText;
    public Text highScoreText;

    private Rigidbody rb; //As 3D tradition, rigidbody is needed for movement
    private int score; //Every safe step is worth one point
    private int highScore; //High score will be the highest number of steps in one game

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score = 0;
        highScore = 0; //0 will be the default score for first time and whatever points players have scored, will replace the previous scores
        gameOverPanel.SetActive(false);
    }

    void FixedUpdate() 
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical"); 

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); 

        rb.AddForce(movement * speed);
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

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("MainGame");
        score = 0;
    }
}
