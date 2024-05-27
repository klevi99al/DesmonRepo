using System.Collections;
using TMPro;
using UnityEngine;

public class Level0Manager : MonoBehaviour
{
    [Header("Dialogue References")]
    [SerializeField] private float delayBetweenLetters = 0.01f;
    [SerializeField] private float delayBetweenWords = 5f;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string[] initialText;

    [Header("Player References")]
    [SerializeField] private Animator playerAnimator;

    [Header("Enemy References")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform[] enemySpawnPositions;


    private int movementHash;

    private void Start()
    {
        movementHash = Animator.StringToHash("IsMoving");
        playerAnimator.SetTrigger("ShouldWake");

        DialogueManage.Instance.PlayDialogue(dialogueText, initialText, delayBetweenLetters, delayBetweenWords);
        StartCoroutine(WaitFordDialogueFinish());
    }

    private IEnumerator WaitFordDialogueFinish()
    {
        yield return new WaitUntil(() => DialogueManage.Instance.currentSentenceNumber == 6);
        playerAnimator.SetBool(movementHash, true);

        EnableEnemy();
    }

    private void EnableEnemy()
    {
        enemy.SetActive(true);

        // Check which one of the transforms has the furthest distance to the player and set it there, so the enemy doesn't spawn right into the player
        float maxDistance = float.MinValue;
        Transform furthestSpawnPosition = null;

        foreach (Transform spawnPosition in enemySpawnPositions)
        {
            float distanceToPlayer = Vector3.Distance(spawnPosition.position, playerAnimator.transform.position);
            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                furthestSpawnPosition = spawnPosition;
            }
        }

        enemy.transform.position = furthestSpawnPosition.position;
    }
}
