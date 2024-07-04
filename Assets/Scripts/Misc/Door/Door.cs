using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    // public ScoreManager scoreManager;
    public bool canCheckForPlayer = true;
    private GameObject protagonist;
    private Score protagonistScore;
    private SpriteRenderer protagonistSpriteRenderer;
    private Animator anim;
    private LevelLoader levelLoader;
    void Start()
    {
        protagonist = GameObject.Find("Protagonist");
        protagonistScore = protagonist.GetComponent<Score>();
        protagonistSpriteRenderer = protagonist.GetComponent<SpriteRenderer>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        anim = GetComponent<Animator>();
    }

   private void OnTriggerEnter2D(Collider2D collider)
    {
        // if its the player
        if(collider.gameObject.layer == 9 && canCheckForPlayer)
        {
            if (protagonistScore.score >= protagonistScore.winningScore)
            {
                anim.SetTrigger("DoorClose");
                //protagonistMovement.canMove = false;
                StartCoroutine(LoadNextLevel());
            }
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

    

