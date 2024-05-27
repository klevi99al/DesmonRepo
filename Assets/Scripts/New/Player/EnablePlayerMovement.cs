using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerMovement : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetMovementState(animator, false);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetMovementState(animator, true);
    }

    private void SetMovementState(Animator animator, bool state)
    {
        animator.GetComponent<PlayerMovement>().canMove = state;
        animator.GetComponent<PlayerActions>().canAttack = state;
    }
}
