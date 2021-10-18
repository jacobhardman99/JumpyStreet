using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinMax
{
    public float minimum;
    public float maximum;

    public static MinMax one
    {
        get
        {
            return new MinMax(1, 1);
        }
    }

    public static MinMax zero
    {
        get
        {
            return new MinMax(0, 0);
        }
            
       
    }

    public MinMax(float min, float max)
    {
        minimum = min;
        maximum = max;
    }

    public float toRandomfloat()
    {
        return Random.Range(minimum, maximum);
    }

    public int toRandomInt()
    {
        return Mathf.RoundToInt(Random.Range(minimum, maximum));
    }
}

[System.Serializable]
public class MinMax3
{
    public Vector3 minimum;
    public Vector3 maximum;

    public static MinMax3 one
    {
        get
        {
            return new MinMax3(Vector3.one, Vector3.one);
        }
    }

    public MinMax3(Vector3 min, Vector3 max)
    {
        minimum = min;
        maximum = max;
    }

    public Vector3 toRandomVector3()
    {
        return new Vector3(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y), Random.Range(minimum.z, maximum.z));
    }

    public Vector3Int toRandomVector3Int()
    {
        return new Vector3Int(Mathf.RoundToInt(Random.Range(minimum.x, maximum.x)), Mathf.RoundToInt(Random.Range(minimum.y, maximum.y)), Mathf.RoundToInt(Random.Range(minimum.z, maximum.z)));
    }
}
[System.Serializable]
public class MinMax3Int
{
    public Vector3Int minimum;
    public Vector3Int maximum;

    public static MinMax3Int one
    {
        get
        {
            return new MinMax3Int(Vector3Int.one, Vector3Int.one);
        }
    }

    public MinMax3Int(Vector3Int min, Vector3Int max)
    {
        minimum = min;
        maximum = max;
    }

    public Vector3Int toRandomVector3Int()
    {
        return new Vector3Int(Mathf.RoundToInt(Random.Range(minimum.x, maximum.x)), Mathf.RoundToInt(Random.Range(minimum.y, maximum.y)), Mathf.RoundToInt(Random.Range(minimum.z, maximum.z)));
    }
}

public class RandomTileObjectSpawner : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private List<GameObject> allowedObjects;
    [Header("Tile Variables")]
    [SerializeField, Tooltip("Should Be odd")] private int tileWidth = 81;
    [SerializeField] private float distanceBetweenAdjacentObjects = 1f;
    [SerializeField] private MinMax objectDencity = MinMax.one;
    [Header("Prop Variables")]    
    [SerializeField] private MinMax PropMovementSpeed = MinMax.zero;
    [SerializeField] private bool RandomZMovementDirection = false;
    [SerializeField] private bool playerCanStandOn = false;
    [SerializeField] private bool randomizePropRotaion = true;
    [Space(5)]
    [SerializeField] private MinMax3 scaleVariation = MinMax3.one;


    private List<float> spawnPoints = new List<float>();

    private float minZPos = 0;
    private float maxZPos = 0;

    private bool useMovingProps = false;

    private void Start()
    {
        if (allowedObjects.Count < 1) { return; }
        if (transform.position.x == 0) { return; }

        SolveMinMaxZPos();
        if ((PropMovementSpeed.minimum + PropMovementSpeed.maximum) / 2 != 0)
        {
            useMovingProps = true;
        }

        SpawnObjects(Mathf.RoundToInt(spawnPoints.Count * objectDencity.toRandomfloat()));        
    }

    // handles spawning the objects in the correct XZ location. Has protection from spawning 2 objects in the same XZ Pos.
    private void SpawnObjects(int count)
    {
        int objCounter = 0;
        List<float> avalableZPos = spawnPoints;

        float newSpeed = Random.Range(PropMovementSpeed.minimum, PropMovementSpeed.maximum);
        if (RandomZMovementDirection)
        {
            newSpeed *= (Random.Range(0, 2) * 2 - 1);
        }

        while (objCounter < count)
        {
            // choose random object and location; add the Z pos to the blacklist; Instance and set pos, rot, parent

            // This chunk loads the valid spots from a seprate list, then removes the item from that list and uses that spot 
            float generatedZPos = avalableZPos[Random.Range(0, avalableZPos.Count)];
            avalableZPos.Remove(generatedZPos);
            GameObject randomObject = allowedObjects[Random.Range(0, allowedObjects.Count)];
            Vector3 randomSpot = new Vector3(transform.position.x, 1, generatedZPos);

            GameObject newObsticle = Instantiate(randomObject, transform);
            newObsticle.transform.SetParent(transform);
            newObsticle.transform.position = randomSpot;
            newObsticle.transform.localScale = scaleVariation.toRandomVector3Int();
            if (useMovingProps)
            {
                PropMover newMover = newObsticle.AddComponent<PropMover>();
                newMover.movement = new Vector3(0,0,newSpeed);
                newMover.playerCanStandOn = playerCanStandOn;
            }
            if (randomizePropRotaion)
            {
                newObsticle.transform.GetChild(0).transform.Rotate(0.0f, Random.Range(0.0f, 360.0f), 0.0f);
            }

            objCounter += 1;
            if (objCounter > tileWidth) { return; } // protection form infinite loop; should be useless
        }
    }

    // solves for the max and min Z pos for a given XY pos; handles if odd or even, if even it basically adds a spot to force the number to be odd
    private void SolveMinMaxZPos()
    {
        if (tileWidth % 2 == 1)
        {
            minZPos = (tileWidth - 1) / -2;
            maxZPos = (tileWidth - 1) / 2;
        }
        else
        {
            minZPos = tileWidth / -2;
            maxZPos = tileWidth / 2;
        }

        float currentZPos = minZPos;
        while (currentZPos <= maxZPos)
        {
            spawnPoints.Add(currentZPos);
            currentZPos += distanceBetweenAdjacentObjects;
        }
    }
}
