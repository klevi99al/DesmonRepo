using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Navigator : MonoBehaviour
{
    public float panSpeed = 5.0f; // Adjust this value for the desired panning speed.
    public float panDistance = 15.0f; // Adjust this value for the desired pan distance.
    public List<GameObject> Levels = new List<GameObject>();
    private int selectedLevel = 0; 
    public GameObject levelSeletor; 
    public GameObject Journal; 
    void Start()
    {
        UnlockLevel(0);
        int lastLevel = PlayerPrefs.GetInt("CurrentLevel");
        int levelToUnlock = PlayerPrefs.GetInt("UnlockLevel");       
        
        if (lastLevel != -1){
            IncreaseNumberOfAttempts(lastLevel);
        } 
        if (levelToUnlock != -1)
        {
            UnlockLevel(levelToUnlock);
            selectedLevel = levelToUnlock;
            updateSelector();
            PlayerPrefs.SetInt("CurrentLevel", -1);
            PlayerPrefs.SetInt("UnlockLevel", -1); 
            UnlockPreviousLevels(levelToUnlock);
        }
        UpdateJournal(selectedLevel);
    }
    void Update()
    {
        // Wait for the user to press the Return/Enter key.
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            SelectLevelDown();
            updateSelector();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            SelectLevelUp();
            updateSelector();
        }

        if (Input.GetKeyDown(KeyCode.Return) )
        {
            PlayerPrefs.SetInt("CurrentLevel", selectedLevel);
            SceneManager.LoadScene("Level"+selectedLevel.ToString());
        }
    }
    public void updateSelector()
    {
        /// move the selected to the current level, if the selector is alread at the bottom or top the list, move the list
        levelSeletor.transform.position = getLevelObject(selectedLevel).transform.position + new Vector3(0.75f, 0f, 0);
        UpdateJournal(selectedLevel);
    }
    public void SelectLevelUp()
    {
        if (selectedLevel < Levels.Count-1 && getLevelObject(selectedLevel+1).GetComponent<Level>().IsUnlocked)
        {
            selectedLevel++;
            updateSelector();
        }
        // if (Levels != null && Levels.Count > 1 && selectedLevel < Levels.Count-1)
        // {
        //     selectedLevel++;
        //     if (selectedLevel >= Levels.Count) return;
        //     // Store the position of the last level
        //     Vector3 lastLevelPosition = Levels[Levels.Count - 1].transform.position;

        //     // Rearrange the positions of the other levels in reverse order
        //     for (int i = Levels.Count - 1; i >= 1; i--)
        //     {
        //         Levels[i].transform.position = Levels[i - 1].transform.position;
        //     }

        //     // Set the position of the first level to the stored last level's position
        //     Levels[0].transform.position = lastLevelPosition;
      

        //     // Remove the first level from the list
        //     GameObject firstLevel = Levels[0];
        //     Levels.RemoveAt(0);

        //     // Add the first level to the end of the list
        //     Levels.Add(firstLevel);
        //     firstLevel.SetActive(false);
            
        // }
        // else
        // {
        //     Debug.LogWarning("Levels list is empty or has only one level. No reordering is performed.");
        // }

    }
    public void SelectLevelDown()
    {
        if (selectedLevel >= 1 && getLevelObject(selectedLevel-1).GetComponent<Level>().IsUnlocked)
        {
            selectedLevel--;
            updateSelector();
        }
        // if (Levels != null && Levels.Count > 1 && selectedLevel > 0)
        // {
        //     selectedLevel--;
        //     // Store the position of the last level
        //     Vector3 firstLevelPosition = Levels[0].transform.position;

        //     // Rearrange the positions of the other levels in reverse order
        //     for (int i = 0; i < Levels.Count-1; i++)
        //     {
        //         Levels[i].transform.position = Levels[i + 1].transform.position;
        //     }

        //     // Set the position of the first level to the stored last level's position
        //     Levels[Levels.Count - 1].transform.position = firstLevelPosition;


        //     // Remove the first level from the list
        //     GameObject lastLevel = Levels[Levels.Count-1];
        //     Levels.RemoveAt(Levels.Count - 1);

        //     // Add the first level to the end of the list
        //     Levels.Insert(0, lastLevel);
        //     lastLevel.SetActive(true);
        // }
        // else
        // {
        //     Debug.LogWarning("Levels list is empty or has only one level. No reordering is performed.");
        // }
    }
    public int GetNumberOfAttempts(int levelIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Check if the child's name matches "Level1"
            if (child.name == "Level" + levelIndex.ToString())
            {
                // Store a reference to the "Level1" child
                Level Nextlevel = child.gameObject.GetComponent<Level>();
                Nextlevel.IsUnlocked = true;
                return Nextlevel.numAttempts;
            }
        }
        return -1;
    }

    public void UnlockLevel(int levelIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Check if the child's name matches "Level1"
            if (child.name == "Level" + levelIndex.ToString())
            {
                // Store a reference to the "Level1" child
                Level Nextlevel = child.gameObject.GetComponent<Level>();
                if (! Nextlevel.IsUnlocked) Nextlevel.Unlock();
                
                break;
            }
        }
    }
    public bool IsLevelUnlocked(int levelIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "Level" + levelIndex.ToString())
            {
                // Store a reference to the "Level1" child
                Level Nextlevel = child.gameObject.GetComponent<Level>();
                return Nextlevel.IsUnlocked;
            }
        }
        return false;
    }
    public void IncreaseNumberOfAttempts(int levelIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "Level" + levelIndex.ToString())
            {
                // Store a reference to the "Level1" child
                Level Nextlevel = child.gameObject.GetComponent<Level>();
                Nextlevel.numAttempts ++;
                break;
            }
        }
    }
    public GameObject getLevelObject(int levelIndex)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == "Level" + levelIndex.ToString()) return child.gameObject;
            
        }
        return null;
    }
    public void UnlockPreviousLevels(int levelIndex){
        UnlockLevel(0);
        for (int i = 0; i < levelIndex; i++) UnlockLevel(i);
    }
    private void UpdateJournal(int levelIndex){
        if(Journal == null ) return; 
        Debug.Log("Journaling");
        for (int i = 0; i < Journal.transform.childCount; i++)
        {
            Transform child = Journal.transform.GetChild(i);
            // Check if the child's name matches "Level1"
            if (child.name == "Level" + levelIndex.ToString())
            {
                child.gameObject.SetActive(true);
            }
            else 
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}