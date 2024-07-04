using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Planting : MonoBehaviour
{
    public Tilemap tilemap;
    private GameObject protagonist;
    private Rigidbody2D protagonistRB;
    private SpriteRenderer spriteRenderer;
    public GameObject BasicPlant;
    public GameObject GhostPlant;
    public GameObject Selector;
    public Animator anim;
    private bool GhostPlaced = false;
    public Tile DiggableTile;
    public GameObject Plant2;
    private PlayerActions playerActions;
    void Start()
    {
        // Find the Protagonist GameObject
        protagonist = GameObject.Find("Protagonist");
        protagonistRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerActions = GetComponent<PlayerActions>();

        if (protagonist == null)
        {
            Debug.LogError("Protagonist GameObject not found.");
        }

        // Optionally find the Tilemap dynamically if not set in the inspector
        if (tilemap == null)
        {
            tilemap = FindObjectOfType<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError("Tilemap not found.");
            }
        }

        if(Selector == null)
        {
            Selector = GameObject.Find("Selector");
        }
    }

    void Update()
    {
        DisplaySelector();

        //if (Mathf.Abs(protagonistRB.velocity.x) > 0.1f || Mathf.Abs(protagonistRB.velocity.x) > 0.1f)
        //{
        //    GhostPlaced = false;
        //    GameObject ghostPlant = GameObject.Find("ghostPlant(Clone)");
        //    Destroy(ghostPlant);
        //}
        //if (CanPlant() && !GhostPlaced && Input.GetKeyDown(KeyCode.P))
        //{
        //    DisplayGhost();
        //}
        //else if (CanPlant() && GhostPlaced && Input.GetKeyDown(KeyCode.P))
        //{
        //    ReplaceGhostPlantWithPlant();
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    Dig();
        //}
        //else if (Input.GetKeyDown(KeyCode.H))
        //{
        //    Harvest();
        //}
        //else if (Input.GetKeyDown(KeyCode.U) && !GhostPlaced)
        //{
        //    DisplayGhost();
        //}
        //else if (Input.GetKeyDown(KeyCode.U) && GhostPlaced)
        //{
        //    Upgrade();
        //}

    }

    public void PlantingAction()
    {
        if (CanPlant())
        {
            if (!GhostPlaced) DisplayGhost();
            else ReplaceGhostPlantWithPlant();

            StartCoroutine(PlayerPlantState());
        }
    }

    private IEnumerator PlayerPlantState()
    {
        playerActions.canPlant = false;
        yield return new WaitForSeconds(0.5f);
        playerActions.canPlant = true;
    }

    public bool CanPlant()
    {
        if (protagonist != null && tilemap != null)
        {
            Vector3Int protagonistTilePosition = tilemap.WorldToCell(protagonist.transform.position);
            Vector3Int tilePosition = spriteRenderer.flipX ?
                new Vector3Int(protagonistTilePosition.x - 1, protagonistTilePosition.y - 1, protagonistTilePosition.z)
                :
                new Vector3Int(protagonistTilePosition.x + 1, protagonistTilePosition.y - 1, protagonistTilePosition.z);
            // Check if there's plantable dirt at this position
            // return tilemap.HasTile(tilePosition); 


            return tilemap.HasTile(tilePosition);

        }

        return false;

    }
    public void DisplaySelector()
    {
        if (CanPlant() || GetSelectedPlant() != null)
        {
            Selector.SetActive(true);
            // calculate spawn position on tilemap
            Vector3Int protagonistTilePosition = tilemap.WorldToCell(protagonist.transform.position);
            Vector3 spawnPosition = spriteRenderer.flipX ?
                tilemap.CellToWorld(new Vector3Int(protagonistTilePosition.x, protagonistTilePosition.y, protagonistTilePosition.z)) - new Vector3(tilemap.cellSize.x / 2, 0, 0)
                :
                tilemap.CellToWorld(new Vector3Int(protagonistTilePosition.x + 1, protagonistTilePosition.y, protagonistTilePosition.z)) + new Vector3(tilemap.cellSize.x / 2, 0, 0);

            Selector.transform.position = new Vector3(spawnPosition.x, spawnPosition.y - 0.02f);

        }
        else
        {
            Selector.SetActive(false);
        }
    }
    public void DisplayGhost()
    {
        // calculate spawn position on tilemap
        Vector3Int protagonistTilePosition = tilemap.WorldToCell(protagonist.transform.position);
        Vector3 spawnPosition = spriteRenderer.flipX ?
            tilemap.CellToWorld(new Vector3Int(protagonistTilePosition.x, protagonistTilePosition.y, protagonistTilePosition.z)) - new Vector3(tilemap.cellSize.x / 2, 0, 0)
            :
            tilemap.CellToWorld(new Vector3Int(protagonistTilePosition.x + 1, protagonistTilePosition.y, protagonistTilePosition.z)) + new Vector3(tilemap.cellSize.x / 2, 0, 0);

        // spawn a plant at the target position
        Instantiate(GhostPlant, spawnPosition, Quaternion.identity);
        GhostPlaced = true;
        PlayerStats.Instance.numberOfGhosts++;
    }
    public void ReplaceGhostPlantWithPlant()
    {
        PlayerStats.Instance.nummberOfPlants++;
        StartCoroutine(PlayPlantingAnimation());
        GameObject ghostPlant = GameObject.Find("ghostPlant(Clone)");

        Vector3Int ghostPlantTilePosition = tilemap.WorldToCell(ghostPlant.transform.position);
        Vector3Int blockBelowSpawnPosition = new(ghostPlantTilePosition.x, ghostPlantTilePosition.y - 1, ghostPlantTilePosition.z);


        // Instantiate the Plant prefab at the position and rotation of ghostPlant
        GameObject newPlant = Instantiate(BasicPlant, ghostPlant.transform.position, ghostPlant.transform.rotation);
        newPlant.name = "plant";
        // Optionally, set the new plant's parent to ghostPlant's parent
        newPlant.transform.parent = ghostPlant.transform.parent;
        tilemap.SetTile(blockBelowSpawnPosition, null);

        // Destroy the ghostPlant
        Destroy(ghostPlant);
        GhostPlaced = false;
    }
    public GameObject GetSelectedPlant()
    {
        // TODO: Change 
        Vector2 checkAreaSize = new(1f, 1f);
        float checkDistance = 0.5f;
        // Calculate the position to check
        Vector2 checkPosition = spriteRenderer.flipX ? (Vector2)protagonist.transform.position - (Vector2)protagonist.transform.right * checkDistance : (Vector2)protagonist.transform.position + (Vector2)protagonist.transform.right * checkDistance;

        // Check for a plant in the defined area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPosition, checkAreaSize, protagonist.transform.eulerAngles.z);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.name == "plant")
            {
                return collider.gameObject;
            }
        }
        return null;
    }
    public void Dig()
    {
        GameObject selectedPlant = GetSelectedPlant();
        if (selectedPlant)
        {
            StartCoroutine(PlayDiggingAnimation());
            Vector3Int plantTilePosition = tilemap.WorldToCell(selectedPlant.transform.position);
            Vector3Int TileBelowPlantTilePosition = new(plantTilePosition.x, plantTilePosition.y - 1, plantTilePosition.z);

            tilemap.SetTile(TileBelowPlantTilePosition, DiggableTile);
            Destroy(selectedPlant);
        }

    }
    public void Harvest()
    {
        GameObject selectedPlant = GetSelectedPlant();
        PlantProduction plantProduction = selectedPlant.GetComponent<PlantProduction>();
        if (plantProduction.readyForHarvest)
        {
            plantProduction.Harvest();
            StartCoroutine(PlayDiggingAnimation());
        }
    }
    public void Upgrade()
    {
        GameObject selectedPlant = GetSelectedPlant();
        if (selectedPlant)
        {
            StartCoroutine(PlayPlantingAnimation());
            selectedPlant.transform.GetPositionAndRotation(out Vector3 plantPosition, out Quaternion plantRotation);
            Destroy(selectedPlant);
            GameObject upgradedPlant = Instantiate(Plant2, plantPosition, plantRotation);
            upgradedPlant.name = "plant";
        }
    }
    private IEnumerator PlayPlantingAnimation()
    {
        int layerIndex = anim.GetLayerIndex("Driven Events");
        anim.SetLayerWeight(layerIndex, 1f); // Activate the layer
        anim.Play("Plant");
        yield return new WaitForSeconds(0.45f); // Wait for the specified duration
        anim.SetLayerWeight(layerIndex, 0f); // Deactivate the layer
    }
    private IEnumerator PlayDiggingAnimation()
    {
        // right now digging and planting are the same, change to different in the future 
        int layerIndex = anim.GetLayerIndex("Driven Events");
        anim.SetLayerWeight(layerIndex, 1f); // Activate the layer
        anim.Play("Plant");
        yield return new WaitForSeconds(0.45f); // Wait for the specified duration
        anim.SetLayerWeight(layerIndex, 0f); // Deactivate the layer
    }
}