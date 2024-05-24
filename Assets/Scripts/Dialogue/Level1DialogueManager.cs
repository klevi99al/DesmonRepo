using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class Level1DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    public BasicDialogue dialogueManager;
    public Animator protagonistAnim;
    public SpriteRenderer spriteRenderer;
    // public GameObject Enemy01;
    public GameObject protagonist;
    public ProtagonistBaseMovements protagonistMovements;
    public LevelLoader levelLoader;
    public Tilemap tilemap;
    private Score score; 
    void Start()
    {
        score = protagonist.GetComponent<Score>();
        //Start with first dialogue
        dialogueManager.StartDialogue();
        StartCoroutine(PlayInitialDialogue());
    
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Harvest(Clone)")!= null && dialogueManager.index < 7) 
        {
            // todo: deactivate box collider
            BoxCollider2D box = GameObject.Find("Harvest(Clone)").GetComponent<BoxCollider2D>();
            // box.IsTrigger = false;
        } 
    } 

    IEnumerator PlayInitialDialogue()
    {
        yield return new WaitForSeconds(5f);
        dialogueManager.IncreaseDialogue(); // the sun will go down soon 
        yield return new WaitForSeconds(6f);
        dialogueManager.IncreaseDialogue(); // I should start the garden 

        yield return new WaitForSeconds(3f);
        dialogueManager.IncreaseDialogue();  // [press p to place plant]

        while (!CheckIfGhostPlantExists()) yield return null;

        dialogueManager.IncreaseDialogue(); // [press p to plant]

        while (!CheckIfPlantExists()) yield return null;

        deactivatePlantSpawner();
        dialogueManager.IncreaseDialogue(); // Now I'll wait for harvest

        yield return new WaitForSeconds(10f);

        dialogueManager.IncreaseDialogue(); // looks like my plant is ready 
        yield return new WaitForSeconds(3f); 
        dialogueManager.IncreaseDialogue(); // move so selector is beneath plant 
        yield return new WaitForSeconds(2f); 
        while (!isSelectorUnderneathPlant()) yield return null; 

        dialogueManager.IncreaseDialogue(); // press H to harvest 
        yield return new WaitForSeconds(2f); 

        while (GameObject.Find("Harvest(Clone)") != null) yield return null; 
        
        dialogueManager.IncreaseDialogue(); // another harvest and I'll be ready 
        yield return new WaitForSeconds(2f); 
        while(score.score < score.winningScore) yield return null;
        dialogueManager.IncreaseDialogue(); // head to door

    }

    private bool isSelectorUnderneathPlant()
    {
        GameObject selectedPlant = protagonist.GetComponent<Planting>().GetSelectedPlant();
        Debug.Log("SelectedPlant" + selectedPlant);
        return selectedPlant != null;
    }
    private bool CheckIfGhostPlantExists()
    {
        // Use GameObject.Find to check for the existence of "Ghost Plant"
        GameObject ghostPlant = GameObject.Find("ghostPlant(Clone)");

        // If a GameObject with the name "Ghost Plant" is found, return true
        return ghostPlant != null;
    }

    private void deactivatePlantSpawner()
    {
        GameObject plant = GameObject.Find("plant");

        if (plant != null)
        {
            Debug.Log("Deactivating plant spawner");
            EnemySpawner enemySpawnerScript = plant.GetComponent<EnemySpawner>();
            if (enemySpawnerScript != null)
            {
                Debug.Log("Deactivating plant spawner");
                enemySpawnerScript.isActive = false;
            }
        }
    }
    private bool CheckIfPlantExists()
    {
        GameObject plant = GameObject.Find("plant");
        return plant != null;
    }
 

}

