using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    public Animator anim; 
    public string animationName = "Blue";
    public float targetDuration = 5f; // Change this to your desired duration in seconds

    private void Start()
    {
        anim.Play(animationName);
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("stateInfo.length: " + stateInfo.length);
        float currentDuration = stateInfo.length;
        float normalizedTime = stateInfo.normalizedTime;
        float speedMultiplier = currentDuration / targetDuration;
        Debug.Log("speedMultiplier: " + speedMultiplier);
        anim.speed = speedMultiplier;
    }

    private void EndLevel() 
    {
        SceneManager.LoadScene("LevelNavigator"); 
    }

}

