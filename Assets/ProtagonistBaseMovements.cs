using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtagonistBaseMovements : MonoBehaviour
{
    private float v0y = 14f; 
    private float v0x = 3f;
    public Rigidbody2D rb;
    private Animator anim; 
    private GameObject attackArea;
    private SpriteRenderer spriteRenderer;

    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb =  GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackArea =  GameObject.Find("Protagonist/AttackAreas/ForwardAttack");
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {      
        if (anim.GetInteger("state") == -1 ) return;
        if (!canMove) {
            anim.SetInteger("state",0);
            return;
        }
    
        if(anim.GetLayerWeight(anim.GetLayerIndex("damage")) < 1f && anim.GetLayerWeight(anim.GetLayerIndex("Driven Events")) < 1f){
            Debug.Log("ProtagonistBaseMovements: " + anim.GetInteger("state"));
            executeAnimationState();
            updateAnimationState();
        }
    }

    public int speedCounter = 0;
    private void updateAnimationState(){
        if (Input.GetKey(KeyCode.A))
        {
           StartCoroutine(PlayAttackingAnimation());

        } 
        else if (Input.GetKeyDown(KeyCode.Space) && anim.GetInteger("state") <= 1 )
        {
            // taking off
            rb.velocity = new Vector2(rb.velocity.x*2,v0y);
            anim.SetInteger("state",2);
        } 
        else if (rb.velocity.y > 0.1f)
        {
            // jumping 
            anim.SetInteger("state",2);
        }
        else if (rb.velocity.y < -0.1f)   
        {
            // falling
            anim.SetInteger("state",3); 
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            // walking
            anim.SetInteger("state",1);
        }
        else
        {
            anim.SetInteger("state",0);
        }
    }

    private int state; 

    private void executeAnimationState(){
        state = anim.GetInteger("state");

        if (state == 4)
        {
            if(Mathf.Abs(rb.velocity.y)<0.1f)
            {
                rb.velocity =  new Vector2(0, rb.velocity.y);
            }
        
        }
        else if (state == 2 || state == 3){
            // jumping or falling: Gravity handels this one
        }
        else if (state == 1){
            //walking 
        } 
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) )
        {
            rb.velocity = new Vector2(v0x, rb.velocity.y);
            
            spriteRenderer.flipX = false;
            Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
            FlipAttackArea(transform, centralAxis, spriteRenderer.flipX);
        } 
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(-1*v0x, rb.velocity.y);
            spriteRenderer.flipX = true; 
            Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
            FlipAttackArea(transform, centralAxis, spriteRenderer.flipX);
        }
    }

    public void AttackObject(Collider2D obj)
    {
        // attacking up, stop rise 
        if (Input.GetKey(KeyCode.UpArrow)) rb.velocity = new Vector2(Mathf.Min(0,rb.velocity.x),rb.velocity.y);
        
        // attacking down, bounce up 
        else if (Input.GetKey(KeyCode.DownArrow) && Mathf.Abs(rb.velocity.y)>0.1f) rb.velocity = new Vector2(rb.velocity.x,20);
        
        // positive: attacker is to right (bounce left )
        else if((obj.transform.position-this.transform.position).x>0) rb.velocity = new Vector2(-3,rb.velocity.y);
        // negative attack is to left (bounce right )
        else rb.velocity = new Vector2(3,rb.velocity.y);

        // if they're still bounce up
        if(Mathf.Abs(rb.velocity.y) > 0.1f) rb.velocity = new Vector2(rb.velocity.x,10); 
    }
    
    private void Walk(){
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && anim.GetInteger("state")<3 && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.A) ))
        {
            if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) )
            {
                anim.SetInteger("state",1);
                rb.velocity = new Vector2(v0x, rb.velocity.y);

                spriteRenderer.flipX = false;
                Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                FlipAttackArea(transform, centralAxis, spriteRenderer.flipX);
            } 
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetInteger("state",1);
                rb.velocity = new Vector2(-1*v0x, rb.velocity.y);
                spriteRenderer.flipX = true; 
                Vector3 centralAxis = transform.position + spriteRenderer.bounds.center;
                FlipAttackArea(transform, centralAxis, spriteRenderer.flipX);
            }
        } 
        else if (Mathf.Abs(rb.velocity.x) < v0x && Mathf.Abs(rb.velocity.y) < 0.01)
        {
            anim.SetInteger("state",0);
        }
    }

    private IEnumerator PlayAttackingAnimation()
    {
        StopAllCoroutines();
        if (anim != null)
        {
           
        int layerIndex = anim.GetLayerIndex("Driven Events");

        anim.SetLayerWeight(layerIndex, 1f); // Activate the layer
        anim.Play("strike");

        while (anim.GetLayerWeight(layerIndex) == 1f) yield return null; 
        
        }
    }

    public void FinishDrivenEvent(){
        int layerIndex = anim.GetLayerIndex("Driven Events");
        anim.Play("New State");
        
        anim.SetLayerWeight(layerIndex, 0f);// Deactivate the layer
    }

    public void IncreaseState(){
        anim.SetInteger("state",anim.GetInteger("state")+1);
    }

    private void FlipAttackArea(Transform currentTransform, Vector3 centralAxis, bool flipX)
    {
       if (currentTransform != null)
        {
            Vector3 currentScale = currentTransform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x) * (flipX ? -1f : 1f);
            attackArea.transform.localScale = currentScale;
        }
    }
 
}
