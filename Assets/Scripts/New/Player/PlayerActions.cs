using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [Header("Player Aattack Properties")]
    [SerializeField] private float attackTime = 0.5f;
    [SerializeField] private GameObject forwardAttackArea;
    [SerializeField] private GameObject downAttackArea;
    [SerializeField] private Animator playerAnimator;

    [Header("Player Harvest Properties")]
    [SerializeField] private Planting plantingScript;

    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool isAttacking = false;
    private float attackTimer;
    private InputActionMap playerActionMap;
    private int attackHash;
    private void Awake()
    {
        attackHash = Animator.StringToHash("IsAttacking");
        attackTimer = attackTime;
        playerActionMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        playerActionMap["Shoot"].performed += Shoot;
        playerActionMap["Plant"].performed += Plant;
        playerActionMap["Harvest"].performed += Harvest;
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime; 
            if (attackTimer <= 0)
            {
                attackTimer = attackTime;
                forwardAttackArea.SetActive(false);
            }
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
         Attack();
    }

    public void Attack()
    {
        if (canAttack)
        {
            playerAnimator.SetBool(attackHash, true);
            canAttack = false;
            isAttacking = true;
            forwardAttackArea.SetActive(true);
            playerAnimator.SetLayerWeight(1, 1);
        }
    }

    public void Plant(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed P");
        plantingScript.ReplaceGhostPlantWithPlant();
    }

    public void Harvest(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed H");
        plantingScript.Harvest();
    }
}
