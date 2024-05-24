using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultStartingInts : MonoBehaviour
{
    public Animator protagonistAnimator;
    public Animator sunsetAnimator;

    void Start()
    {
        // Get the name of the currently active scene
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Scene name: " + sceneName);
        // Set the Animator parameter based on the current scene
        switch (sceneName)
        {
            case "Level0":
                protagonistAnimator.SetInteger("state", -1);
                sunsetAnimator.SetBool("HalfLevel", true);
                break;
            case "Level1":
                sunsetAnimator.SetBool("HalfLevel", true);
                break;
            case "Level2":
                sunsetAnimator.SetBool("HalfLevel", true);
                break;
            // Add cases for other scenes as needed
        }
    }
}
