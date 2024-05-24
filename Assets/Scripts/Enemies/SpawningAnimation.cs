using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameObjectToSpawn;
    public void SpawnGameObject()
    {
        if (gameObjectToSpawn == null) return;
        Instantiate(gameObjectToSpawn, this.transform.position, Quaternion.identity);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
