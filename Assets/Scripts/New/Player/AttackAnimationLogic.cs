using UnityEngine;

public class AttackAnimationLogic : StateMachineBehaviour
{

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerActions playerActions = animator.GetComponent<PlayerActions>();
        playerActions.isAttacking = false;
        playerActions.canAttack = true;
        animator.SetLayerWeight(1, 0);
        animator.SetBool("IsAttacking", false);
    }
}
