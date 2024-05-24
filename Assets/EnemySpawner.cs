using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefab = new List<GameObject>(); // Set this to your enemy prefab in the inspector
    public GameObject spawner; 
    public float averageRadius = 2f; // Average radius from the GameObject
    public float radiusVariance = 1f; // Variance in the radius
    public Tilemap tilemap;
    public bool isActive = true;
    public int spawnInterval; 

    private GameObject protagonist; 
    void Start()
    {
        protagonist = GameObject.Find("Protagonist");
        GameObject gridGameObject = GameObject.Find("Grid");
        tilemap = gridGameObject.transform.Find("ContactMap").GetComponent<Tilemap>();   
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemyNearby()
    {
        if (!isActive) return;
        
        Vector3 spawnPosition = GetRandomPositionInNearbyNonFilledTile();
        

        if(spawnPosition != Vector3.zero) // Check if a valid position was found
        {
            Debug.Log("Spawning");
            GameObject newSpawner = Instantiate(spawner, spawnPosition, Quaternion.identity);
            SpawningAnimation spawnerScript = newSpawner.GetComponent<SpawningAnimation>();

            int randomIndex = Random.Range(0, enemyPrefab.Count);
            GameObject randomEnemy = enemyPrefab[randomIndex];
            spawnerScript.gameObjectToSpawn = randomEnemy;

            // Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("No valid position found for spawning");
        }
    }
    public Vector3 GetRandomPositionInNearbyNonFilledTile()
    {
        Vector3Int currentTile = tilemap.WorldToCell(transform.position);
        List<Vector3Int> nonFilledTiles = FindNonFilledHorizontalAdjacentTiles(currentTile);

        if (nonFilledTiles.Count == 0)
        {
            Debug.LogError("No non-filled tiles found above or below");
            return Vector3.zero;
        }

        Vector3Int selectedTile = nonFilledTiles[Random.Range(0, nonFilledTiles.Count)];
        return  tilemap.GetCellCenterWorld(selectedTile);
        // return GetRandomPositionInTile(selectedTile);
    }

    private List<Vector3Int> FindNonFilledHorizontalAdjacentTiles(Vector3Int currentTile)
    {
        List<Vector3Int> nonFilledTiles = new List<Vector3Int>();

        // Check only the tiles above and below
        for (int x = -2; x <= 2; x++)
        {
            if (x == 0) continue; // Skip the current tile
            
            // TODO: Make sure enimies don't spawn over Tiles

            Vector3Int checkingTile = new Vector3Int(currentTile.x+x, currentTile.y , currentTile.z);
            if (!tilemap.HasTile(checkingTile))
            {
                nonFilledTiles.Add(checkingTile);
            }
        }

        return nonFilledTiles;
    }
    private Vector3 GetRandomPositionInTile(Vector3Int tile)
    {
        Vector3 tileCenter = tilemap.GetCellCenterWorld(tile);
        Vector3 tileSize = tilemap.cellSize;

        float randomX = Random.Range(tileCenter.x - tileSize.x / 2, tileCenter.x + tileSize.x / 2);
        float randomY = tileCenter.y - tileSize.y / 2;

        return new Vector3(randomX, randomY, tileCenter.z);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) // Infinite loop to keep the coroutine running
        {
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval
            SpawnEnemyNearby(); // Call the spawn function

        }
    }
}


