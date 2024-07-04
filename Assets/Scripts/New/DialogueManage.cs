using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManage : MonoBehaviour
{
    public bool dialogueIsPlaying = false;
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

    public void PlayDialogue(TMP_Text text, string[] dialogues, float delayBetweenEachLetter = 0.01f, float delayBetweenLines = 5f)
    {
        StopAllCoroutines();
        dialogueIsPlaying = false;
        StartCoroutine(PlayDialogueCoroutine(text, dialogues, delayBetweenEachLetter, delayBetweenLines));
    }

    private IEnumerator PlayDialogueCoroutine(TMP_Text text, string[] dialogues, float delayBetweenEachLetter, float delayBetweenLines)
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
            yield return new WaitForSeconds(delayBetweenLines);
        }
        currentSentenceNumber = 0;
        dialogueIsPlaying = false;
    }
}
