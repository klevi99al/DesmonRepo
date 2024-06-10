using UnityEngine;

public class Health : EnemyMovement
{
    public int health;
    private readonly int MAX_HEALTH = 6;
    public bool canDamage = true;

    public Animator enemyAnimator;

    public HealthDisplay healthDisplay;

    //void update()
    //{
    //    canDamage = enemyAnimator.GetLayerWeight(enemyAnimator.GetLayerIndex("Damage")) == 0 && enemyAnimator.GetLayerWeight(enemyAnimator.GetLayerIndex("Die")) == 0;
    //}
    public void Damage(int amount)
    {
        if (canDamage)
        {
            if (amount > 0)
            {
                SetDamageLayer();
                health -= amount;
            }

            if (health <= 0) Die();

            if (healthDisplay != null)
            {
                healthDisplay.updateHealthDisplay();
            }
        }

    }
    public void Heal(int amount)
    {
        if (amount > 0)
        {
            health = Mathf.Min(health + amount, MAX_HEALTH);
        }
    }
    private void Die()
    {
        enemyAnimator.SetLayerWeight(enemyAnimator.GetLayerIndex("Die"), 1);

    }

    private void SetDamageLayer()
    {
        enemyAnimator.SetLayerWeight(enemyAnimator.GetLayerIndex("Damage"), 1);
    }

}
