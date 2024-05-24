using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite locked; // Assign your first sprite in the Unity Inspector
    public Sprite unlocked; // Assign your second sprite in the Unity Inspector
    public int numAttempts = 0;
    public bool IsUnlocked = false;
    public Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        // Check the value of IsUnlocked and set the sprite accordingly
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsUnlocked){
            anim.SetBool("IsUnlocked", true);
            anim.Play("unlockLevel");
        };
    }
    public void Unlock()
    {
        anim.SetTrigger("Unlock");
    }

    public void SetUnlocked()
    {
        IsUnlocked = true;
    }

    
}
