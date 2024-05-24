using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    // public ScoreManager scoreManager;
    private GameObject protagonist;
    private Score protagonistScore;
    private ProtagonistBaseMovements protagonistMovement;
    private ProtagonistHealth protagonistHealth;
    private SpriteRenderer protagonistSpriteRenderer;
    private Animator anim;
    private LevelLoader levelLoader;
    void Start()
    {
        protagonist = GameObject.Find("Protagonist");
        protagonistScore = protagonist.GetComponent<Score>();
        protagonistMovement = protagonist.GetComponent<ProtagonistBaseMovements>();
        protagonistHealth = protagonist.GetComponent<ProtagonistHealth>();
        protagonistSpriteRenderer = protagonist.GetComponent<SpriteRenderer>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        anim = GetComponent<Animator>();
    }

   private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name=="Protagonist" && protagonistScore.score >= protagonistScore.winningScore){
            anim.SetTrigger("DoorClose");
            protagonistMovement.canMove = false;
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {   
        PlayerPrefs.SetInt("UnlockLevel", PlayerPrefs.GetInt("CurrentLevel")+1);
        yield return new WaitForSeconds(1f);
        levelLoader.LoadNextLevel();
    }
    public void DissapearProtagonist(){
        protagonistSpriteRenderer.enabled = false;

    }
}

    

