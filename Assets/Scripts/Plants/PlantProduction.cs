using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProduction : MonoBehaviour
{
    public float timerDuration = 10.0f; // Duration in seconds, can be set in the Inspector
    public GameObject harvestObject; 

    public GameObject harvest;
    public bool readyForHarvest; 
    void Start()
    {
        
        // Start the countdown as soon as the GameObject is active
        StartCoroutine(StartCountdown());
    }

    public void Harvest(){
        if (readyForHarvest)
        {
            harvest.GetComponent<Harvest>().CompleteHarvest();
            StartCoroutine(StartCountdown());
        }
         
    }
    public IEnumerator StartCountdown()
    {
        float currentTime = timerDuration;
        readyForHarvest = false;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }

        readyForHarvest = true;
        harvest = Instantiate(harvestObject, transform.position + new Vector3(0.05f,0.6f,0), Quaternion.identity);
        harvest.GetComponent<Harvest>().plant = this.gameObject;
    }
}
