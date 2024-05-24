using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : EnemyMovement
{
    [SerializeField] public int health; 
    private int MAX_HEALTH = 6;
    public bool CanDamage = true;

    public Animator enemyAnimator; 

    public HealthDisplay healthDisplay; 

    void Start(){

    }

    void update(){
        CanDamage = enemyAnimator.GetLayerWeight(enemyAnimator.GetLayerIndex("Damage")) == 0 && enemyAnimator.GetLayerWeight(enemyAnimator.GetLayerIndex("Die")) == 0;
    }
    public void Damage(int amount){
        if (CanDamage)
        {
            if (amount > 0) {
                SetDamageLayer();
                this.health -= amount;
            }

            if (health <= 0) Die();

            if (healthDisplay != null)
            {
                healthDisplay.updateHealthDisplay();
            }
        }
        
    }
    public void Heal(int amount){
         if(amount > 0)
        {
            this.health = Mathf.Min(this.health+amount,MAX_HEALTH);
        }
    }
    private void Die(){
        enemyAnimator.SetLayerWeight(enemyAnimator.GetLayerIndex("Die"), 1);

    }

    private void SetDamageLayer(){
        enemyAnimator.SetLayerWeight(enemyAnimator.GetLayerIndex("Damage"), 1);
    }

}
