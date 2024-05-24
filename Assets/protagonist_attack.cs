using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protagonist_attack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attackArea;
    bool attacking = false; 
    float timeToAttack = 0.5f; 
    float timer = 0f; 

    void Start()
    {
        // update to get right attack area 
        attackArea = transform.Find("AttackAreas/ForwardAttack").gameObject;
        attackArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) )
        {
            Attack();
        }
        if (attacking)
        {
            timer += Time.deltaTime;
        }
        
        if(timer >= timeToAttack)
        {
            attackArea.SetActive(false);
            attacking = false ;
            timer = 0f;
        } 

        UpdateActiveAttackArea();
    } 

    private void Attack(){
        attacking = true; 
        attackArea.SetActive(true);
    }

    //TODO: make it so some attack areas don't get locked on active
    private void UpdateActiveAttackArea(){
        

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // attackArea = transform.Find("AttackAreas/UpAttackArea").gameObject;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            attackArea = transform.Find("AttackAreas/DownAttackArea").gameObject;
        } else 
        {
            attackArea = transform.Find("AttackAreas/ForwardAttack").gameObject;
        }
    }
}
