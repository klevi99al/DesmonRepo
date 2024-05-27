using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultStartingInts : MonoBehaviour
{
    [SerializeField] private Animator protagonistAnimator;
    [SerializeField] private Animator sunsetAnimator;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "level10")
        {
            protagonistAnimator.SetInteger("state", -1);
        }
        sunsetAnimator.SetBool("HalfLevel", true);
    }
}
