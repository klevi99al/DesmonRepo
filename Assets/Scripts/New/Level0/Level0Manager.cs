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
    public GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private PlayerActions playerAction;

    [Header("Enemy References")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform[] enemySpawnPositions;

    [Header("Other references")]
    public LevelLoader levelLoader;

    private int movementHash;

    private void Start()
    {
        movementHash = Animator.StringToHash("IsMoving");
        playerAnimator.SetTrigger("ShouldWake");
        playerAction.canAttack = false;
        DialogueManage.Instance.PlayDialogue(dialogueText, initialText, delayBetweenLetters, delayBetweenWords);
        StartCoroutine(PlayCurrentMission());
    }

    private IEnumerator PlayCurrentMission()
    {
        yield return new WaitUntil(() => DialogueManage.Instance.currentSentenceNumber == 6);
        playerAnimator.SetBool(movementHash, true);

        EnableEnemy();
        yield return new WaitUntil(() => DialogueManage.Instance.currentSentenceNumber == 9);
        playerAction.canAttack = true;

        yield return new WaitUntil( () => PlayerStats.Instance.currentLevelKills > 0);
        initialText = new string[1];
        initialText[0] = "That felt nice :)";
        DialogueManage.Instance.PlayDialogue(dialogueText, initialText, delayBetweenLetters, delayBetweenWords);

        // enemy is dead
        yield return new WaitForSeconds(3f);

        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerPrefs.SetInt("UnlockLevel", 1);
        levelLoader.LoadNextLevel();
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
