using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    public List<string> dialogues;
    [SerializeField] private string[] strings;
    public float writingSpeed;
    public int index;
    protected int charIndex;
    protected bool started;
    protected bool waitForNext;

    void Start()
    {
        //DialogueManage.Instance.PlayDialogue(dialogueText, strings, writingSpeed, 5);
        //StartDialogue();
    }


    public void StartDialogue()
    {
        if (started)
            return;

        started = true;

        GetDialogue(0);
    }
    public void GetDialogue(int i)
    {

        index = i;

        charIndex = 0;

        dialogueText.text = string.Empty;

        StartCoroutine(Writing());
    }

    public void EndDialogue()
    {

        started = false;

        waitForNext = false;

        StopAllCoroutines();

    }
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index];

        dialogueText.text += currentDialogue[charIndex];

        charIndex++;

        if (charIndex < currentDialogue.Length)
        {
            yield return new WaitForSeconds(writingSpeed);
            StartCoroutine(Writing());
        }
        else
        {
            waitForNext = true;
        }
    }
    public void IncreaseDialogue(){
        waitForNext = false;
        index++;

        if (index < dialogues.Count)
        {
            GetDialogue(index);
        }
        else
        {
            EndDialogue();
        }
    }
}
