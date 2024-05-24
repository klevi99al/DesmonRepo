using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Movement : EnemyMovement
{

    public GameObject enemyFlame;
    public float moveSpeed;
    public float maxDuration = 2f;
    public float activeDuration = 0.5f;
    public float inactiveDuration = 0.5f;

    protected override void CustomStart()
    {
        StartCoroutine(MoveAndStallRoutine());
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetLayerWeight(anim.GetLayerIndex("Die")) == 1 && !takingDamage)
        {
            StopAllCoroutines();
            StartCoroutine(DieCoroutine());
        }
        else if (anim.GetLayerWeight(anim.GetLayerIndex("Damage")) == 1 && !takingDamage)
        {
            StopAllCoroutines();
            StartCoroutine(takeDamage());

        }
        else if (IsProtagonistWithinXTiles(2) && !takingDamage && !ProtagonistAbove())
        {
            FaceProtagonist();
        }

    }

    private IEnumerator MoveAndStallRoutine()
    {
        while (true) // Loop to continuously move and stall
        {
            // Randomly choose moving direction
            

            bool shouldMove = Random.Range(0, 2) == 0;

            if (shouldMove)
            {
                bool shouldFlip = Random.Range(0, 2) == 0;

                enemyFlame.SetActive(false);
                

                if (IsProtagonistWithinXTiles(2) && !ProtagonistAbove())
                {
                    FaceProtagonist();
                }
                else if (shouldFlip && !ProtagonistAbove())
                {
                    isFlipped = !isFlipped;
                    Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                    FlipRecursively(transform, centralAxis, isFlipped);
                }
                yield return StartCoroutine(MoveEnemyCoroutine());
            }
            else 
            {
                if (IsProtagonistWithinXTiles(2) && !ProtagonistAbove())
                {
                    FaceProtagonist();
                    anim.SetTrigger("Shoot");
                    yield return new WaitForSeconds(0.2f);
                    enemyFlame.SetActive(true);
                    yield return new WaitForSeconds(inactiveDuration);
                    anim.ResetTrigger("Shoot");
                }
                else
                {
                    // Rest for a random duration
                    enemyFlame.SetActive(false);
                    float restDuration = Random.Range(0f, maxDuration);
                    yield return new WaitForSeconds(restDuration);
                }

            }
        }
    }

    private IEnumerator MoveEnemyCoroutine()
    {
        // Move for a random duration
        float moveDuration = Random.Range(0f, maxDuration);
        float moveTimer = 0;
        while (moveTimer < moveDuration)
        {
            // Move the enemy left or right based on 'moveRight'
            float moveDirection = !isFlipped ? 1 : -1;
            Vector3 directionVec = new Vector3(moveDirection * moveSpeed, 0, 0);
            (bool leftEmpty, bool rightEmpty) = AdjacentTilesEmpty();
            if (IsProtagonistWithinXTiles(2) && !takingDamage && !ProtagonistAbove())
            {
                FaceProtagonist();
                moveDirection = GetProtagonistDirection();
            }

            else
            {

                if (moveDirection == 1 && rightEmpty && !takingDamage)
                {
                    transform.Translate(moveSpeed * moveDirection * Time.deltaTime, 0, 0);
                }
                else if (moveDirection == -1 && leftEmpty && !takingDamage)
                {
                    transform.Translate(moveSpeed * moveDirection * Time.deltaTime, 0, 0);
                }
             
            }
            

            moveTimer += Time.deltaTime;
            yield return null; // Wait for the next frame

        }
    }

    private IEnumerator DieCoroutine()
    {
        if (!takingDamage) anim.SetBool("Die", true);
        takingDamage = true;
        enemyFlame.SetActive(false);
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private IEnumerator takeDamage()
    {
        if (!takingDamage) DamageBounce(-5f,10f);

        takingDamage = true;
        enemyFlame.SetActive(false);
        yield return new WaitForSeconds(1);
        anim.SetLayerWeight(anim.GetLayerIndex("Damage"), 0);
        takingDamage = false;
        StartCoroutine(MoveAndStallRoutine());
    }


}
