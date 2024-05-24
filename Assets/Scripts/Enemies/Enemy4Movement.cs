using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Movement : EnemyMovement
{

    public float moveSpeed;
    public float maxDuration = 2f;
    public float activeDuration = 0.5f;
    public float inactiveDuration = 0.5f;
    private bool isMoveing = false;
    private bool isAttacking = false;

    public StaticAttackArea staticAttackArea;
    public Health Health;

    protected override void CustomStart()
    {
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
            anim.SetInteger("state", 2);
            StartCoroutine(takeDamage());
            StartCoroutine(Attack());

        }
        else if (IsProtagonistWithinTileBox(7, 1) && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
        else if (!IsProtagonistWithinTileBox(7, 1)) isAttacking = false;
    }


    private IEnumerator DieCoroutine()
    {
        if (!takingDamage) anim.SetBool("die", true);
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private IEnumerator takeDamage()
    {
        if (!takingDamage) DamageBounce(-5f,10f);

        takingDamage = true;
        yield return new WaitForSeconds(1);
        anim.SetLayerWeight(anim.GetLayerIndex("Damage"), 0);
        takingDamage = false;
    }

    private IEnumerator Attack()
    {
        while (isAttacking)
        {

            yield return new WaitForSeconds(1);

            float moveDirection = !isFlipped ? 1 : -1;
            FaceProtagonist();

            if (anim.GetInteger("state") == 2 || anim.GetInteger("state") == 5) Health.CanDamage = true;
            else Health.CanDamage = false;

            if( anim.GetInteger("state") == 0 ) anim.SetInteger("state", 1);

            if (anim.GetInteger("state") == 4) if(IsProtagonistWithinTileBox(3, 1)) staticAttackArea.DamageProtagonist();
   
            if (anim.GetInteger("state") == 5)
            {
                yield return new WaitForSeconds(1);
                anim.SetInteger("state", 6);
                yield return new WaitForSeconds(5);
                isAttacking = false;

            }

            if (IsProtagonistWithinTileBox(3, 1) && anim.GetInteger("state") == 2)  anim.SetInteger("state", 3);
            else  yield return null;
        }


    }

    public void IncreaseState()
    {
        anim.SetInteger("state", anim.GetInteger("state") + 1);
        
    }
    public void ResetState()
    {
        anim.SetInteger("state", 0);
    }

    public void DestroySelf(){
        Destroy(gameObject);
    }
}

