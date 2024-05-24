using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemy_01_movement : EnemyMovement
{
    private float speed = 1f;
    private bool move = false;
    private float direction = 1;
    private BoxCollider2D BoxCollider2DBody;


    protected override void CustomStart()
    {
        rb =  GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;
        BoxCollider2DBody = GetComponent<BoxCollider2D>();

        GameObject gridGameObject = GameObject.Find("Grid");
        tilemap = gridGameObject.transform.Find("Floor").GetComponent<Tilemap>();  

        StartCoroutine(MoveRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        if (anim.GetLayerWeight(anim.GetLayerIndex("Die")) == 1)
        {
            if (!takingDamage)
            {
                StopAllCoroutines();
                StartCoroutine(DieCoroutine());
            }
        }
        else if (anim.GetLayerWeight(anim.GetLayerIndex("Damage")) == 1 )
        {
            if (!takingDamage)
            {
                StopAllCoroutines();
                StartCoroutine(DamageCoroutine());
            }  
        }
        else 
        {
            rb.velocity = new Vector2(speed * direction, rb.velocity.y);
        }

    }

    private bool CanMoveToDirection( )
    {
        Vector3 directionVec = new Vector3 (direction * speed, 0,0);
        Vector3Int targetCell = tilemap.WorldToCell(transform.position + directionVec * Time.deltaTime + new Vector3 (0, -1,0));
        return tilemap.HasTile(targetCell); // Returns true if the target cell is filled
    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            move = true;
            direction = Random.Range(0, 2) * 2 - 1;
            spriteRenderer.flipX = direction < 0;
            if (IsProtagonistWithinXTiles(1)){
                direction = GetProtagonistDirection();
                FaceProtagonist();
            }
            if (!CanMoveToDirection()) {
                direction *= -1;
                spriteRenderer.flipX = direction < 0;
            }
            
            float waitTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(waitTime);   
        }
    }
    
    private IEnumerator DamageCoroutine()
    {
        if (!takingDamage)  DamageBounce(2f,2f);
        takingDamage = true;

        yield return new WaitForSeconds(0.2f);
        anim.SetLayerWeight(anim.GetLayerIndex("Damage"), 0);
        takingDamage = false;
        // yield return new WaitForSeconds(0.2f);
        StartCoroutine(MoveRoutine());
    }
    private IEnumerator DieCoroutine()
    {
        if (!takingDamage) DamageBounce(2f,2f);
        takingDamage = true;

        yield return new WaitForSeconds(0.5f);
        anim.SetInteger("state",4);
    }
    private IEnumerator EnemyDie(){
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
