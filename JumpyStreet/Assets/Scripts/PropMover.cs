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

    private float RoadLeftEnd = 49f;
    private float roadRightEnd = -49f;

    private GameObject terrainManager;

    private void Start()
    {
        terrainManager = GameObject.FindWithTag("TerrainManager");
        transform.Rotate(new Vector3(0, ((movementVector.z / Mathf.Abs(movementVector.z)) * 90) - 90, 0));
    }

    void Update()
    {
        if (!runMovement) { return; }
        transform.position = transform.position + (movementVector * Time.deltaTime);
        if (transform.position.z < roadRightEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, RoadLeftEnd);
        }
        else if (transform.position.z > RoadLeftEnd)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, roadRightEnd);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerCanStandOn && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (playerCanStandOn && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = terrainManager.transform;
        }
    }
}
