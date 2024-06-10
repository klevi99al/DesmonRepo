using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;

    // Properties
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
                if (instance == null)
                {
                    GameObject singletonObject = new(typeof(PlayerStats).Name);
                    instance = singletonObject.AddComponent<PlayerStats>();
                }
            }
            return instance;
        }
    }

    public int playerKills;         // count all the kills
    public int currentLevelKills;   // count kills for the specific level, simply set this to zero each start of a new scene

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
