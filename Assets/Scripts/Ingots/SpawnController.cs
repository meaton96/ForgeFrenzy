using UnityEngine;
using System.Collections.Generic;
public class SpawnController : MonoBehaviour
{
   // public GameObject ingotPrefab; //The prefab for the ingot
    //public BoxCollider spawnArea;

    public static SpawnController Instance;
    public List<Spawner> spawners = new List<Spawner>();
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
    public Ingot SpawnIngotAt(Ingot.IngotType ingotType, Vector3 location) {
        return spawners[(int)ingotType].SpawnIngotAt(location);
    }
    public void RecycleIngot(Ingot.IngotType ingotType, int ingotId) {
        spawners[(int)ingotType].RecycleIngot(ingotId);
    }
    public void ResetIngotPosition(Ingot.IngotType ingotType, int ingotId) {
        spawners[(int)ingotType].ResetIngotPosition(ingotId);
    }
    public void FireIngotCannon(Ingot.IngotType ingotType, Vector3 position, bool left = true) {
        spawners[(int)ingotType].SpawnIngotFromCannon(position, left);
    }
}
