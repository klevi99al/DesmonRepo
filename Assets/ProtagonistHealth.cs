using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistHealth : MonoBehaviour
{
    [SerializeField] public int health = 5; 
    public int MAX_HEALTH = 5; 

    public Animator anim; 
    public Rigidbody2D rb;
    public HealthDisplay healthDisplay; 

    void Start(){
        StartCoroutine(RechargeHeartsAtInterval(45f));
    }

    void Update(){
        Debug.Log("HEALTH" + health);
        if (health <= 1)
        {
            Debug.Log("Protagonist is dead");
            Die();
        }
    }
    public void Damage(int amount,GameObject enemy){
        if(amount > 0)
        {
            this.health -= amount;
            if (anim != null)
            {
                if((enemy.transform.position-rb.transform.position).x>0) rb.velocity = new Vector2(-5,10);
                // negative attack is to left (bounce right )
                else rb.velocity = new Vector2(5,10);

                StartCoroutine(PlayDamageAnimation());
                // move up animation layer to produce damage
                
            } 
        }
       

        if(healthDisplay != null)
        {
            Debug.Log("Damaging Health Display");
            healthDisplay.updateHealthDisplay();
        }
        
        
    }

    public void Heal(int amount){
         if(amount > 0)
        {
            this.health = Mathf.Min(this.health+amount,MAX_HEALTH);
            healthDisplay.updateHealthDisplay();
        }
    }

    private void Die(){
        anim.SetLayerWeight(anim.GetLayerIndex("damage"), 1f); // Activate the layer
        anim.SetTrigger("die");
        // Destroy(gameObject,1f);
    }

    private IEnumerator PlayDamageAnimation()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("damage"), 1f); // Activate the layer
        yield return new WaitForSeconds(0.3f); // Wait for the specified duration
        anim.SetLayerWeight(anim.GetLayerIndex("damage"), 0f); // Activate the layer
    }

    // private IEnumerator PlayDieAnimation()
    // {
    //     anim.SetLayerWeight(anim.GetLayerIndex("damage"), 1f); // Activate the layer
    //     anim.SetTrigger("death");
    // }

    private void LoadNextLevel()
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadNextLevel();
        // Load the next level
    }

    private void deactivate() 
    {
        rb.isKinematic = false;
    }

    private IEnumerator RechargeHeartsAtInterval(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if(health < MAX_HEALTH)
            {
                Heal(1);
                healthDisplay.updateHealthDisplay();
            }
        }
    }
}
