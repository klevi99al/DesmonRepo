using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class ProtagonistHealth : MonoBehaviour
{
    public int health = 5;
    public int MAX_HEALTH = 5;

    public Animator anim;
    public Rigidbody2D rb;
    public HealthDisplay healthDisplay;
    public LevelLoader levelLoader;
    public PlayerMovement playerMovement;
    public PlayerActions playerActions;

    private bool canTakeDamage = true; // we use this bool as sometimes the player gets damaged so fast that almost makes him uncontrollable, lets add a little bit of damage cooldown
    public float damageCooldown = 0.5f;
    void Start()
    {
        StartCoroutine(RechargeHeartsAtInterval(45f));
    }

    public void Damage(int amount, GameObject enemy)
    {
        Debug.Log("Damaging player:  " + enemy.name);
        if (canTakeDamage && amount > 0)
        {
            health -= amount;

            if (health > 0)
            {
                if ((enemy.transform.position - rb.transform.position).x > 0) rb.velocity = new Vector2(-5, 10);
                else rb.velocity = new Vector2(5, 10);

                StartCoroutine(PlayDamageAnimation());

                canTakeDamage = false;
                StartCoroutine(nameof(DamageCooldown));
            }
            else
            {
                Die();
            }

            if (healthDisplay != null)
            {
                Debug.Log("Damaging Health Display");
                healthDisplay.updateHealthDisplay();
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }


    public void Heal(int amount)
    {
        if (amount > 0)
        {
            health = Mathf.Min(health + amount, MAX_HEALTH);
            healthDisplay.updateHealthDisplay();
        }
    }

    private void Die()
    {
        playerMovement.canMove = false;
        playerActions.canAttack = false;

        anim.SetLayerWeight(1, 1f);
        anim.SetTrigger("Die");
        levelLoader.LoadNextLevel();
    }

    private IEnumerator PlayDamageAnimation()
    {
        anim.SetLayerWeight(1, 1f); // Activate the layer
        anim.SetTrigger("DamagePlayer");
        yield return new WaitForSeconds(0.3f); // Wait for the specified duration
        anim.SetLayerWeight(1, 0f); // Activate the layer
    }

    private IEnumerator RechargeHeartsAtInterval(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (health < MAX_HEALTH)
            {
                Heal(1);
                healthDisplay.updateHealthDisplay();
            }
        }
    }
}
