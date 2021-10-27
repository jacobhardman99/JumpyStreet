//////////////////////////////////////////////
//Assignment/Lab/Project: Jumpy Street
//Name: Malcolm Coronado, Jacob Hardman
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
    public AudioSource Jumping; //This sound effect will play when the player makes any form of movement
    public AudioSource Death; //This sound effect will play when the player collides with any form of hazard (Water, vehicle crashes)

    [HideInInspector] public GameObject gameOverPanel;
    [HideInInspector] public Text scoreText;
    [HideInInspector] public Text highScoreText;
    [HideInInspector] public Text gameOverText;

    private Rigidbody rb; //As 3D tradition, rigidbody is needed for movement
    private int score = 0; //Every safe step is worth one point (Players cannot spam the movement on the same terrain rows to gain more points).

    [SerializeField] private float movementSpacer = 1f;
    [SerializeField] private float movementTimer = .2f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private GameObject playerMesh;

    private TerrainManager tm;
    private HighscoreHolder hsh;
    private Vector3 raycastOffset = new Vector3(0, 1, 0);
    private bool allowInput = true;
    private float furthestXPos = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        gameOverPanel = GameObject.Find("gameOverPanel");
        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        highScoreText = GameObject.Find("highScoreText").GetComponent<Text>();
        gameOverText = GameObject.Find("gameOverText").GetComponent<Text>();
        gameOverPanel.SetActive(false);

        UpdateScore();

        tm = GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainManager>();
        hsh = GameObject.FindWithTag("HighscoreHolder").GetComponent<HighscoreHolder>();
        if (tm == null || hsh == null)
        {
            print("Error no Terrain Manager Found!");
            Destroy(gameObject);
        }

        highScoreText.text = "High Score: " + hsh.highscore.ToString();
    }

    void Update() //Every movement will have the jump sound effect player. 
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && allowInput)
        {
            StartCoroutine(runMovement(Vector3.right));
        }

        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && allowInput)
        {
            StartCoroutine(runMovement(Vector3.left));
        }

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && allowInput)
        {
            StartCoroutine(runMovement(Vector3.back));
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && allowInput)
        {
            StartCoroutine(runMovement(Vector3.forward));
        }
    }

    private IEnumerator runMovement(Vector3 movementOffset)
    {
        Vector3 startPos = transform.position;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + raycastOffset, transform.TransformDirection(movementOffset), movementSpacer) && Physics.Raycast(transform.position + movementOffset + raycastOffset, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            allowInput = false;            
            float startTime = Time.time;
            Vector3 fixedHitPoint = new Vector3(Mathf.RoundToInt(hit.point.x), hit.point.y, Mathf.RoundToInt(hit.point.z));
            transform.GetChild(0).transform.rotation = Quaternion.LookRotation(movementOffset, Vector3.up);

            while (Time.time < startTime + movementTimer)
            {
                Vector3 newpos = Vector3.Lerp(startPos,fixedHitPoint, (Time.time - startTime) / movementTimer);
                newpos.y = Mathf.Lerp(startPos.y,fixedHitPoint.y, (Time.time - startTime) / movementTimer) + Mathf.Sin((Time.time - startTime) / movementTimer * Mathf.PI) * jumpHeight;
                transform.position = newpos;
                yield return new WaitForFixedUpdate();
            }

            transform.position = fixedHitPoint;

            if (transform.position.x > furthestXPos)
            {
                furthestXPos = transform.position.x + .5f;
                score += 1;
                UpdateScore();
            }
            tm.playerPos = transform.position;
            allowInput = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard")) //With vehicles, players need to be in front of them to die. Hitting the side of one does not count as a death.
        {
            StopAllCoroutines();
            StartCoroutine(deathSequence());
        }
    }

    private IEnumerator deathSequence()
    {
        score -= 1;
        UpdateScore();
        deathParticles.Play();
        playerMesh.SetActive(false);

        gameObject.transform.parent = tm.transform;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        allowInput = false;

        yield return new WaitForSeconds(deathParticles.main.duration);

        Debug.LogWarning("Game Over");
        gameOverPanel.SetActive(true);
        if (score > hsh.highscore)
        {
            hsh.highscore = score;
        }
        gameOverText.text = "GAME OVER.\n Your score in the game was " + (score).ToString() + "\n Your highscore is " + hsh.highscore.ToString();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("MainGame");
    }
}
