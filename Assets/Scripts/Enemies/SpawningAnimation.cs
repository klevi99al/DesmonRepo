using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameObjectToSpawn;
    public Transform enemyParent;
    public void SpawnGameObject()
    {
        if (gameObjectToSpawn == null) return;
        Instantiate(gameObjectToSpawn, transform.position, Quaternion.identity, enemyParent);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
