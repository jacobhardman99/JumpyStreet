using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer_DONOTUSE : MonoBehaviour
{
    [SerializeField] private float movementSpacer = 1f;

    private TerrainManager tm;
    private void Start()
    {
        tm = GameObject.FindGameObjectWithTag("TerrainManager").GetComponent<TerrainManager>();
        if (tm == null)
        {
            print("Error no Terrain Manager Found!");
            Destroy(this);
        }
    }

    // Update is called once per frame
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
}
