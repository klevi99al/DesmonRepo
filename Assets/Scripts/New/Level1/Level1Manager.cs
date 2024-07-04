using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

public class Level1Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float delayBetweenLetters = 0.01f;
    [SerializeField] private float delayBetweenWords = 5f;
    [SerializeField] private string[] texts;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private Door door;
    string[] currentString;
    private void Start()
    {
        door.canCheckForPlayer = false;
        StartCoroutine(WaitForDialogueManager());
    }

    private IEnumerator WaitForDialogueManager()
    {
        while (DialogueManage.Instance == null)
        {
            yield return null;
        }

        currentString = new string[4];
        System.Array.Copy(texts, currentString, currentString.Length);


        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);
        playerActions.canPlant = false;
        StartCoroutine(PlayCurrentMission());
    }

    private IEnumerator PlayCurrentMission()
    {
        // player can plant now
        yield return new WaitUntil(() => DialogueManage.Instance.dialogueIsPlaying == false);
        playerActions.canPlant = true;
        yield return new WaitUntil(() => PlayerStats.Instance.numberOfGhosts == 1);
        
        currentString = new string[1];
        currentString[0] = texts[4];
        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);
        yield return new WaitUntil(() => PlayerStats.Instance.nummberOfPlants == 1);

        currentString = new string[1];
        currentString[0] = texts[5];
        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);

        yield return new WaitForSeconds(10);

        currentString[0] = texts[6];
        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);

        currentString = new string[2];
        currentString[0] = texts[7];
        currentString[1] = texts[8];
        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);

        yield return new WaitUntil(() => PlayerStats.Instance.numberOfHarvests == 2);

        currentString = new string[1];
        currentString[0] = texts[10];
        door.canCheckForPlayer = true;
        DialogueManage.Instance.PlayDialogue(dialogueText, currentString, delayBetweenLetters, delayBetweenWords);
    }
}
