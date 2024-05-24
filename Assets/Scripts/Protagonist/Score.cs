using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] public int score = 0;
    [SerializeField] public int winningScore = 10;
    public int currentLevel; 
    public Animator scoreAnimator;
    private GameObject scoreObject; 
    public LevelLoader levelLoader;

    void Start()
    {
        scoreObject = GameObject.Find("Main Camera/ScoreDisplay");
        if (scoreObject != null)
        {
            Vector2 leftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0,1));
            scoreObject.transform.position =leftCorner +  new Vector2(1f,-0.2f);
            scoreAnimator.speed = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreObject = GameObject.Find("Main Camera/ScoreDisplay");
        if (scoreObject != null)
        {
            // Vector2 leftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0,1));
            // scoreObject.transform.position = leftCorner +  new Vector2(0.4f,-0.2f);
            // scoreAnimator.speed = 0f;
        }
        UpdateAnimation();
    }

    public void IncreaseScore(int amount ){
        score += amount;
        
    //    if(score >= winningScore){
    //         PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    //         PlayerPrefs.SetInt("UnlockLevel", currentLevel+1);
    //         levelLoader.LoadNextLevel();
    //     }
    }

    private void UpdateAnimation()
    {
        AnimatorClipInfo[] clipInfo = scoreAnimator.GetCurrentAnimatorClipInfo(0);
        float animationProgress = (float) score / winningScore;

        if (clipInfo.Length > 0)
        {
            AnimationClip clip = clipInfo[0].clip;

            float frameRate = clip.frameRate;
            float currentTime = scoreAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * clip.length;
            float currentFrame = Mathf.FloorToInt(currentTime * frameRate) % clip.length;

            if(currentFrame < animationProgress)
            {
                scoreAnimator.speed = 1f;
            }
            else 
            {
                scoreAnimator.speed = 0f;
            }
        }
    }
}
