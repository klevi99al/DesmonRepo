using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject plant; 
    public Score score;
    private Animator anim;
    public int increaseAmount = 5;

    private bool completingHarvest = false;
    void Start()
    {
        GameObject protagonist = GameObject.Find("Protagonist");
        score = protagonist.GetComponent<Score>();
        anim = GetComponent<Animator>();
    }
    public void CompleteHarvest(){
        completingHarvest = true;
        score.IncreaseScore(increaseAmount);
        // plant.GetComponent<PlantProduction>().BeginCountdown();
        anim.Play("CollectHarvest");
        PlayerStats.Instance.numberOfHarvests++;
        PlayerStats.Instance.numberOfGhosts--;
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.name == "Protagonist" && !completingHarvest) CompleteHarvest();
    // }

    public void Destroy(){
        Destroy(gameObject);
    }
}
