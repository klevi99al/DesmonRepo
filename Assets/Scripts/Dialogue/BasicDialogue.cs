using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BasicDialogue : MonoBehaviour
{
    //Text component
    public TMP_Text dialogueText;
    //Dialogues list
    public List<string> dialogues;

    public float writingSpeed;
    //Index on dialogue
    public int index;
    //Character index
    protected int charIndex;
    //Started boolean
    protected bool started;
    //Wait for next boolean
    protected bool waitForNext;

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue();
    }


    public void StartDialogue()
    {
        if (started)
            return;

        //Boolean to indicate that we have started
        started = true;
        //Show the window

        //Start with first dialogue
        GetDialogue(0);
    }
    public void GetDialogue(int i)
    {
        //start index at zero
        index = i;
        //Reset the character index
        charIndex = 0;
        //clear the dialogue component text
        dialogueText.text = string.Empty;
        //Start writing
        StartCoroutine(Writing());
    }

    //End Dialogue
    public void EndDialogue()
    {
        //Stared is disabled
        started = false;
        //Disable wait for next as well
        waitForNext = false;
        //Stop all Ienumerators
        StopAllCoroutines();
        //Hide the window
    }
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index];
        //Write the character
        dialogueText.text += currentDialogue[charIndex];
        //increase the character index 
        charIndex++;
        //Make sure you have reached the end of the sentence
        if (charIndex < currentDialogue.Length)
        {
            //Wait x seconds 
            yield return new WaitForSeconds(writingSpeed);
            //Restart the same process
            StartCoroutine(Writing());
        }
        else
        {
            //End this sentence and wait for the next one
            waitForNext = true;
        }
    }
    public void IncreaseDialogue(){
        waitForNext = false;
        index++;

        //Check if we are in the scope fo dialogues List
        if (index < dialogues.Count)
        {
            //If so fetch the next dialogue
            GetDialogue(index);
        }
        else
        {
            //If not end the dialogue process
            // ToggleIndicator(true);
            EndDialogue();
        }
    }
}