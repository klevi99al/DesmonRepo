using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManage : MonoBehaviour
{
    private bool dialogueIsPlaying = false;

    public static DialogueManage Instance;
    [HideInInspector] public int currentSentenceNumber = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayDialogue(TMP_Text text, string[] dialogues, float delayBetweenEachLetter = 0.01f, float delayBetweenWords = 5f)
    {
        if (!dialogueIsPlaying)
        {
            StartCoroutine(PlayDialogueCoroutine(text, dialogues, delayBetweenEachLetter, delayBetweenWords));
        }
    }

    private IEnumerator PlayDialogueCoroutine(TMP_Text text, string[] dialogues, float delayBetweenEachLetter, float delayBetweenWords)
    {
        dialogueIsPlaying = true;

        foreach (string dialogue in dialogues)
        {
            text.text = string.Empty;

            foreach (char letter in dialogue)
            {
                text.text += letter;
                yield return new WaitForSeconds(delayBetweenEachLetter);
            }
            currentSentenceNumber++;
            yield return new WaitForSeconds(delayBetweenWords);
        }
        currentSentenceNumber = 0;
        dialogueIsPlaying = false;
    }
}
