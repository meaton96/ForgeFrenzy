using UnityEngine;
using System.Collections.Generic;
public class SpawnController : MonoBehaviour
{
   // public GameObject ingotPrefab; //The prefab for the ingot
    //public BoxCollider spawnArea;

    public static SpawnController Instance;
    public List<Spawner> spawners = new List<Spawner>();
    int currentSpawnerIndex = 0;
    private void Awake() {
        Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            BeginSpawningIngots();
        }
    }

    void BeginSpawningIngots() {
        spawners.ForEach(spawner => spawner.spawnEnabled = true);
    }
}
