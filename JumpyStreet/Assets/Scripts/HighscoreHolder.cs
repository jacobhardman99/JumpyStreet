using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreHolder : MonoBehaviour
{
    public int highscore = 0;
    private static GameObject highscoreHolderGO;

    private void Start()
    {
        if (highscoreHolderGO == null)
        {
            highscoreHolderGO = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameObject != highscoreHolderGO)
        {
            Destroy(gameObject);
        }
    }
}
