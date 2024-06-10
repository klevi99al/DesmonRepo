using System.Collections;
using UnityEngine;

public class StaticAttackArea : MonoBehaviour
{
    private GameObject protagonist;
    private Rigidbody2D protagonistRB;
    private Animator protagonistAnimator;
    private ProtagonistHealth protagonistHealth;
    public int damageAmount = 1;
    private BoxCollider2D selfCollider;
    private int damageHashID;

    void Start()
    {
        protagonist = FindObjectOfType<PlayerMovement>().gameObject;
        protagonistRB = protagonist.GetComponent<Rigidbody2D>();
        protagonistHealth = protagonist.GetComponent<ProtagonistHealth>();
        protagonistAnimator = protagonist.GetComponent<Animator>();
        selfCollider = GetComponent<BoxCollider2D>();
        damageHashID = Animator.StringToHash("DamagePlayer");
    }
    private void OnEnable()
    {
        if (IsContained())
        {
            StartCoroutine(ProtagonistDamageDelay(0.1f));
        }
    }

    private bool IsContained() // fully or partially contained 
    {

        if (selfCollider != null && protagonistRB != null)
        {
            Bounds colliderBounds = selfCollider.bounds;
            Bounds rigidbodyBounds = new(protagonistRB.position, selfCollider.size);

            // Check for intersection between the bounds
            return colliderBounds.Intersects(rigidbodyBounds);
        }
        else
        {
            return false;
        }
    }

    private IEnumerator ProtagonistDamageDelay(float amount)
    {
        yield return new WaitForSeconds(amount);
        DamageProtagonist();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // i am not sure why this script is contained in the enemies and in the player attack areas too, and it checks for the player only
        // but if what we want to do is damage the player, im gonna remove this script from the player and keep it only in the enemies too
        if (collider.gameObject.layer == 9)
        {
            DamageProtagonist();
        }
    }

    public void DamageProtagonist()
    {
        protagonistHealth.Damage(damageAmount, transform.parent.gameObject);
        protagonistAnimator.SetTrigger(damageHashID);
    }
}
