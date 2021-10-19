using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class tileRule
{
    public int minimumTiles = 1;
    public int maximumTiles = 1;
    [Space]
    public List<GameObject> tilesToUse = new List<GameObject>();
}


// Basically these are the options for a tile set.
[System.Serializable]
public class ChunkControls // this is an individual tile
{
    public string name;
    public List<tileRule> tileRules;
}

[System.Serializable]
public struct TerrainControls
{
    public string name;
    public List<ChunkControls> chunks; // These are the chunks that this biome can spawn
}

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private GameObject testPlayer; // Refrence to spawn in the player
    [SerializeField] private GameObject playerBoundries; // refence to spawn in the movement bounds
    [Space]
    [SerializeField] private string startingTileSetname;
    [SerializeField] private List<TerrainControls> biomes = new List<TerrainControls>(); // all the data for each of the tile sets also this is the BIOME
    [Space]
    [SerializeField] private int tilesAhead = 10; // the number of tiles the generator should be ahead of the player;
    [SerializeField] private int tilesBehind = 10; // the number of tiles behind the player that stay generated;
    [SerializeField] private int minBiomeTiles = 75;
    [SerializeField] private int maxBiomeTiles = 125;
    [Space]
    [SerializeField] private float TileStartOffset = -2; // the number of tiles that should spawn before the player at the start; used to hide void before the player.
    [SerializeField] private float TileSpacer = 1; // the distance between two tiles, by default should be the width of all tiles.

    // Dealing with generating tiles
    private float newTileXPos;
    private float newBiomeXPos = 0;
    private int biomeId = 0;
    private List<GameObject> LoadedTiles = new List<GameObject>();

    // Dealing with where the player is
    private GameObject player;
    private GameObject playerBounds;
    private int playerXTile = 0;


    // Public interface for the terrain gen simply put in the player's pos every jump and the terrain gen will do the rest
    public Vector3 playerPos 
    {
        set
        {
            playerXTile = Mathf.RoundToInt(value.x / TileSpacer);            
            if (playerXTile + tilesAhead > newTileXPos)
            {
                GenerateTerrainChunk();
            }
            if (playerXTile > playerBounds.transform.position.x)
            {
                playerBounds.transform.position = new Vector3(playerXTile, 1, 0);
            }
            RemovePassedTiles();
        }
    }

    private void Start()
    {
        // checks to insure that no issues with tile sets can occure
        bool goodToGo = true;
        if (biomes.Count < 1)
        {
            Debug.LogWarning("Warning, no biomes found!");
            goodToGo = false;
        }
        foreach (var biome in biomes)
        {
            if (biome.chunks.Count < 1) 
            {
                Debug.LogWarning(biome.name + " is missing chunks it can use!");
                goodToGo = false;
            }
            foreach (var chunk in biome.chunks)
            {
                if (chunk.tileRules.Count < 1) 
                {
                    Debug.LogWarning(biome.name + "." + chunk.name + " is missing rules for it to use!");
                    goodToGo = false;
                }
                int ruleNumber = 0;
                foreach (var rule in chunk.tileRules)
                {
                    if (rule.tilesToUse.Count < 1)
                    {
                        Debug.LogWarning(biome.name + "." + chunk.name + "." + ruleNumber + " is missing tiles it can use!");
                        goodToGo = false;
                    }
                    ruleNumber += 1;
                }
            }
        }
        if (!goodToGo) { return; }

        SpawnPlayer();
        newTileXPos = TileStartOffset * TileSpacer;
        GenerateNewBiome(0);
        GenerateTerrainChunk(startingTileSetname); // Note: protection against bad values for any generating terrain chunk is built in when it gets the data for the tile set
        while (newTileXPos <= tilesAhead) // generates remaining terrain that is visible
        {
            GenerateTerrainChunk();
        }
    }

    private void SpawnPlayer()
    {
        player = Instantiate(testPlayer, transform);
        player.transform.position = new Vector3(0, 1, 0);

        playerBounds = Instantiate(playerBoundries, transform);
        playerBounds.transform.position = new Vector3(7.5f, 1, 0);
    }

    // Handles Generateing a chunk of terrain
    private void GenerateTerrainChunk(string genType = "")
    {
        ChunkControls chunkData = GetGenerationData(genType);
        //print(chunkData.name + " : " + newTileXPos);
        foreach ( tileRule rule in chunkData.tileRules)
        {
            int tileCount = Random.Range(rule.minimumTiles, rule.maximumTiles);
            int currentTileNumber = 0;
            while (currentTileNumber < tileCount)
            {
                // generates a tile from the list of useable tiles for this loop
                GenerateTerrainTile(rule.tilesToUse);

                currentTileNumber += 1;
            }
        }
        if (newTileXPos > newBiomeXPos)
        {
            GenerateNewBiome();
        }
    }

    // handles the spawning of individual tiles
    private GameObject GenerateTerrainTile(List<GameObject> allowedTiles,int specificTile = -1)
    {
        GameObject tilePrefab = getRandomTilePrefab(tiles: allowedTiles, tileNum: specificTile);
        GameObject newTile = Instantiate(tilePrefab, transform);
        LoadedTiles.Add(newTile);
        newTile.transform.position = new Vector3(newTileXPos, 0, 0);
        newTileXPos += TileSpacer;

        return newTile;
    }

    // Returns a NonInstanced prefab of a tile from a given list ( random or specific )
    private GameObject getRandomTilePrefab(List<GameObject> tiles, int tileNum = -1)
    {
        if (tileNum != -1)
        {
            return tiles[tileNum];
        }
        else
        {
            return tiles[Random.Range(0, tiles.Count)];
        }
    }

    // Handles the GenerateTerrainChunk()'s need for the generation data; returns random if not specified
    private ChunkControls GetGenerationData(string genType = "")
    {
        if (genType == "")
        {
            int randomGenerationInt = Random.Range(0, biomes[biomeId].chunks.Count);
            return biomes[biomeId].chunks[randomGenerationInt];
        }
        else
        {
            foreach (var genData in biomes[biomeId].chunks)
            {
                if (genData.name == genType)
                {
                    return genData;
                }
            }
            return biomes[biomeId].chunks[0];
        }
    }

    private void GenerateNewBiome(int forceBiomeId = -1)
    {
        if (forceBiomeId == -1)
        {
            forceBiomeId = Random.Range(0, biomes.Count);
        }
        biomeId = forceBiomeId;
        newBiomeXPos += Random.Range(minBiomeTiles, maxBiomeTiles + 1);
    }

    // Removes old tiles behind the player.
    private void RemovePassedTiles()
    {
        int unloadPoint = playerXTile - tilesBehind;
        List<GameObject> tilesToUnload = new List<GameObject>();
        foreach (GameObject tile in LoadedTiles)
        {
            if (tile.transform.position.x < unloadPoint)
            {
                tilesToUnload.Add(tile);
            }
            else
            {
                break;
            }
        }
        foreach (GameObject tile in tilesToUnload)
        {
            LoadedTiles.Remove(tile);
            Destroy(tile);
        }
    }


}