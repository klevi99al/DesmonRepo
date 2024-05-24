using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAttackArea : MonoBehaviour
{
    // a static attack area for enemies to attack the protagonist
    public GameObject protagonist;
    public Rigidbody2D protagonistRB; 
    public Animator protagonistAnimator; 
    public ProtagonistHealth protagonistHealth; 
    public int damageAmount = 1;
    private BoxCollider2D selfCollider;
    private bool previousState;
    void Start()
    {
        //protagonist =  GameObject.Find("Protagonist");
        protagonistRB = protagonist.GetComponent<Rigidbody2D>();
        protagonistHealth = protagonist.GetComponent<ProtagonistHealth>(); 
        protagonistAnimator = protagonist.GetComponent<Animator>();
        selfCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        if (IsContained())
        {
            StartCoroutine(ProtagonistDamageDelay(0.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool IsContained() // fully or partially contained 
    {

        if (selfCollider != null && protagonistRB != null)
        {
            Bounds colliderBounds = selfCollider.bounds;
            Bounds rigidbodyBounds = new Bounds(protagonistRB.position, selfCollider.size);

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
        protagonistHealth.Damage(damageAmount,this.transform.parent.gameObject);
        protagonistAnimator.SetBool("damage",true);
    }
    private void OnTriggerEnter2D(Collider2D protagonistCollider)
    {
        if(protagonistCollider.name=="Protagonist"){
            protagonistHealth.Damage(damageAmount,this.transform.parent.gameObject);
            protagonistAnimator.SetBool("damage",true);
        }
    }

    public void DamageProtagonist()
    {
        protagonistHealth.Damage(damageAmount, this.transform.parent.gameObject);
        protagonistAnimator.SetBool("damage", true);
    }
}
