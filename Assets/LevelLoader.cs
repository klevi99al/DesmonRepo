using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class LevelLoader : MonoBehaviour
{
   [SerializeField] private Animator anim;
   [SerializeField] private Animator protagonistAnimator;
   [SerializeField] private float transitionTime = 10f; 

 
    public void LoadNextLevel(){
        StartCoroutine(ReturnToNavigate());
    }

    public IEnumerator ReturnToNavigate(){
        // yield return StartCoroutine(PlayDieAnimation());
        anim.SetTrigger("start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("LevelNavigator");
    }

    private IEnumerator PlayDieAnimation()
    {
        // Set the Damage layer weight to 1
        protagonistAnimator.SetLayerWeight(protagonistAnimator.GetLayerIndex("damage"), 1);
        protagonistAnimator.SetBool("die",true);
        anim.SetTrigger("start");
        // Wait for 3 seconds
        yield return new WaitForSeconds(5);
    }
}
