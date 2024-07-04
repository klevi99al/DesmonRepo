using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_area : MonoBehaviour
{
    // Start is called before the first frame update
    private int damage = 1; 

    public Rigidbody2D rb; 

    public Animator anim; 

    public SpriteRenderer spriteRenderer;
    public PlayerActions playerActions;

    private bool attackMaxReached = false;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (playerActions.isAttacking)
        {
            if (collider.GetComponent<Health>() != null)
            {
                // attackMaxReached = true;
                // StartCoroutine(ResetAttackMax());
                //TODO: protagonist can attack multiple at once
                //protagonistBaseMovement.AttackObject(collider);
                Health componentHealth = collider.GetComponent<Health>();
                componentHealth.Damage(damage);
                if (componentHealth.health <= 0)
                {
                    PlayerStats.Instance.currentLevelKills++;
                    PlayerStats.Instance.playerKills++;
                }
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ResetAttackMax()
    {
        // todo: change this so it aligns with the attack animation perfectly
        yield return new WaitForSeconds(0.5f);
        attackMaxReached = false;
    }
}
