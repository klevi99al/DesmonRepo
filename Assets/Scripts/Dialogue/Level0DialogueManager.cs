using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class Level0DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public BasicDialogue dialogueManager;
    public Animator protagonistAnim;
    public SpriteRenderer spriteRenderer;
    public GameObject enemy01;
    public GameObject protagonist;
    public ProtagonistBaseMovements protagonistMovements;
    public LevelLoader levelLoader;
    public Tilemap tilemap;
    void Start()
    {
        //Start with first dialogue
        dialogueManager.StartDialogue();
        StartCoroutine(PlayInitialDialogue());
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    IEnumerator PlayInitialDialogue()
    {
        yield return new WaitForSeconds(5f);
        dialogueManager.IncreaseDialogue(); // this is bad 
        yield return new WaitForSeconds(6f);
        dialogueManager.IncreaseDialogue(); // I can't remember anything 
        yield return new WaitForSeconds(10f);
       
        // freeze movement
        dialogueManager.IncreaseDialogue(); // ...
        protagonistMovements.canMove = false;
        protagonistAnim.SetInteger("state", 0);
        
        yield return new WaitForSeconds(4f);
        dialogueManager.IncreaseDialogue();  // am I ... is this the garden?? 
        yield return new WaitForSeconds(5f);
        dialogueManager.IncreaseDialogue(); // ... 

        if (IsCloserToFurthestLeftTile()) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        StartCoroutine(SpawnEnemyFourTilesAhead());
        protagonistMovements.canMove = true;
        
        yield return new WaitForSeconds(0.5f);
        dialogueManager.IncreaseDialogue(); // what is that? 
       
        yield return new WaitForSeconds(3f);
        dialogueManager.IncreaseDialogue(); // looks evil 
        
        yield return new WaitForSeconds(3f);
        dialogueManager.IncreaseDialogue(); // Kill it. [press a] 

        protagonistMovements.canMove = true;
 
    }

    // private void spawnEnemy()
    // {
    //     Vector3 spawnPosition = protagonistMovements.transform.position + protagonistMovements.transform.forward;
    //     Instantiate(Enemy01, spawnPosition, Quaternion.identity);
    // }
    public bool IsCloserToFurthestLeftTile()
    {
        Vector3 protagonistPosition = protagonist.transform.position;
        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log("bounds: " + bounds);
        Vector3 furthestLeftTileWorldPos = tilemap.GetCellCenterWorld(new Vector3Int(bounds.x, 0, 0));
        Vector3 furthestRightTileWorldPos = tilemap.GetCellCenterWorld(new Vector3Int(bounds.x + bounds.size.x - 1, 0, 0));

        float distanceToLeftTile = Vector3.Distance(protagonistPosition, furthestLeftTileWorldPos);
        float distanceToRightTile = Vector3.Distance(protagonistPosition, furthestRightTileWorldPos);

        return distanceToLeftTile < distanceToRightTile;
    }

    IEnumerator SpawnEnemyFourTilesAhead()
    {
        Vector3 protagonistPosition = protagonist.transform.position;
        Vector3Int protagonistCellPos = tilemap.WorldToCell(protagonistPosition);

        // Calculate the position three tiles in front of the protagonist
        Vector3Int spawnPosition = protagonistCellPos + new Vector3Int(spriteRenderer.flipX? -2 : 2, 0, 0);

        // Convert the cell position back to world position
        Vector3 spawnWorldPosition = tilemap.GetCellCenterWorld(spawnPosition);

        enemy01.SetActive(true);
        enemy01.transform.position = spawnWorldPosition;
        
        // Instantiate Enemy01 at the calculated position
        // GameObject enemy01 = Instantiate(enemy01, spawnWorldPosition, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        
        // GameObject enemy01AttackArea = GameObject.Find("enemy_01(Clone)/Enemy01AttackArea");
        // enemy01AttackArea.setActive(false);
        while(GameObject.Find("Enemy01(Clone)") != null) yield return null;

        Debug.Log("Enemy01 is dead");
        dialogueManager.IncreaseDialogue();
        yield return new WaitForSeconds(3f);

        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("UnlockLevel", 1);
        levelLoader.LoadNextLevel();
        
    }
}

