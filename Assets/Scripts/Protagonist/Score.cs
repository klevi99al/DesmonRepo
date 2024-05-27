using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] public int score = 0;
    [SerializeField] public int winningScore = 10;

    public Animator scoreAnimator;
    [SerializeField] private GameObject scoreObject;


    void Start()
    {
        Vector2 leftCorner = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        scoreObject.transform.position = leftCorner + new Vector2(1f, -0.2f);
        scoreAnimator.speed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
    }

    private void UpdateAnimation()
    {
        AnimatorClipInfo[] clipInfo = scoreAnimator.GetCurrentAnimatorClipInfo(0);
        float animationProgress = (float)score / winningScore;

        if (clipInfo.Length > 0)
        {
            AnimationClip clip = clipInfo[0].clip;

            float frameRate = clip.frameRate;
            float currentTime = scoreAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * clip.length;
            float currentFrame = Mathf.FloorToInt(currentTime * frameRate) % clip.length;

            if (currentFrame < animationProgress)
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
