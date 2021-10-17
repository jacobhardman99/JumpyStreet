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

    private Vector3 movementVector;
    private bool runMovement = false;

    private float RoadLeftEnd = 49f;
    private float roadRightEnd = -49f;

    private void Start()
    {
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
}