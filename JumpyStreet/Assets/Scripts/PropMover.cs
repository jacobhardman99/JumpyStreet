using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropMover : MonoBehaviour
{
    public Vector3 movement
    {
        set
        {
            if (!runMovement)
            {
                movementVector = value;
                runMovement = true;
            }
        }
    }

    public bool playerCanStandOn = false;

    private Vector3 movementVector;
    private bool runMovement = false;
    private GameObject terrainManager;

    private float tileLeftEnd = 49f;
    private float tileRightEnd = -49f;

    private void Start()
    {
        terrainManager = GameObject.FindWithTag("TerrainManager");
        transform.Rotate(new Vector3(0, ((movementVector.z / Mathf.Abs(movementVector.z)) * 90) - 90, 0));
    }

    void Update()
    {
        if (!runMovement) { return; }
        transform.position = transform.position + (movementVector * Time.deltaTime);
        if (transform.position.z < tileRightEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, tileLeftEnd);
        }
        else if (transform.position.z > tileLeftEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, tileRightEnd);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerCanStandOn)
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerCanStandOn)
        {
            collision.gameObject.transform.parent = terrainManager.transform;
        }
    }

}
