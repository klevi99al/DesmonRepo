using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Movement : EnemyMovement
{

    public float moveSpeed;
    public float maxDuration = 2f;
    public float activeDuration = 0.5f;
    public float inactiveDuration = 0.5f;
    public GameObject swipeAttackArea;
    public GameObject ShootAttackArea;
    public Vector3 TargetPosition;
    private bool isAttacking = false;
    private bool isMoveing = false;
    public BoxCollider2D visionCollider;

    private bool EnterAttackZone = false;
    protected override void CustomStart()
    {
        StartCoroutine(MoveAndStallRoutine());
    }

    void Update()
    {
        if (anim.GetLayerWeight(anim.GetLayerIndex("Die")) == 1 && !takingDamage)
        {
            isMoveing = false;
            StopAllCoroutines();
            Die();
        }
        else if (anim.GetLayerWeight(anim.GetLayerIndex("Damage")) == 1 && !takingDamage)
        {
            isMoveing = false;
            StopAllCoroutines();
            StartCoroutine(takeDamage());

        }
        else{ 
        }
    }

    protected IEnumerator MoveAndStallRoutine()
    {
        while (true) // LoopD continuously move and stall
        {
            swipeAttackArea.SetActive(false);
            ShootAttackArea.SetActive(false);
            isMoveing = true;
            TargetPosition = GetRandomHorizontalTile();
            while(Mathf.Abs(transform.position.x - TargetPosition.x)> 0.1f)
            {
                if(TargetPosition.x < transform.position.x && !isFlipped || TargetPosition.x > transform.position.x && isFlipped)
                {
                    isFlipped = !isFlipped;
                    Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                    FlipRecursively(transform, centralAxis, isFlipped);
                }

                int direction = TargetPosition.x < transform.position.x ? -1: 1; 
                anim.SetInteger("state", 1);
                rb.velocity = new Vector2(direction*moveSpeed, rb.velocity.y);
                yield return null;
            }

            anim.SetInteger("state", 0);
            rb.velocity = new Vector2(0, rb.velocity.y); 
            yield return new WaitForSeconds(Random.Range(0.5f, maxDuration));
            
           
        }
    }
    
    private void Die()
    {
        swipeAttackArea.SetActive(false);
        ShootAttackArea.SetActive(false);
        if (!takingDamage) anim.SetBool("Die", true);
        takingDamage = true;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name=="Protagonist"){
            Debug.Log("Protagonist entered");
            StopAllCoroutines();
            StartCoroutine(Attack());
        }
    }
     private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.name=="Protagonist" && !takingDamage){
            Debug.Log("Protagonist Existed");
            StopAllCoroutines();
            StartCoroutine(MoveAndStallRoutine());
        }
    }

    private IEnumerator takeDamage()
    {
        if (!takingDamage) DamageBounce(2.5f,7f);
        takingDamage = true;
        yield return new WaitForSeconds(0.5f);
        anim.SetLayerWeight(anim.GetLayerIndex("Damage"), 0);
        takingDamage = false;
        StartCoroutine(Attack());
    }

    protected IEnumerator Attack()
    {
        while(true){
            isMoveing = false;
            FaceProtagonist();
            bool shouldShoot = Random.Range(0, 2) == 0;
            
            if (!shouldShoot)
            {
                while(Mathf.Abs(protagonist.transform.position.x- transform.position.x) > 0.65f)
                {
                    moveWithProtagonist(true);
                    yield return null;
                }
                anim.SetInteger("state", 0);
                rb.velocity = new Vector2(0, rb.velocity.y);
                yield return new WaitForSeconds(1f);
                anim.SetInteger("state", 3);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                while(Mathf.Abs(protagonist.transform.position.x - transform.position.x) > 1f){
                    moveWithProtagonist(true);
                    yield return null;
                }
                while(Mathf.Abs(protagonist.transform.position.x - transform.position.x) < 0.65f)
                { 
                    moveWithProtagonist(false);
                    yield return null;
                }
                anim.SetInteger("state", 0);
                rb.velocity = new Vector2(0, rb.velocity.y);
                yield return new WaitForSeconds(1f);
                anim.SetInteger("state", 4);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void moveWithProtagonist(bool Towards = true)
    {
       TargetPosition = protagonist.transform.position;
        int direction = TargetPosition.x < transform.position.x ? -1: 1; 
        if (!Towards) direction *= -1;
        anim.SetInteger("state", 1);
        rb.velocity = new Vector2(direction*moveSpeed, rb.velocity.y);
    }
    
    public void StartSwipe(){
        swipeAttackArea.SetActive(true);
    }
    public void EndSwipe(){
        swipeAttackArea.SetActive(false);
        anim.SetInteger("state", 0);
    }
    public void StartShoot(){
        ShootAttackArea.SetActive(true);
    }
    public void FinishShoot(){
        ShootAttackArea.SetActive(false);
        anim.SetInteger("state", 0);
    }
    public void DestroySelf(){
        Destroy(gameObject);
    }

}