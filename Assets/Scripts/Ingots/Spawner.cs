using UnityEngine;
using System.Collections.Generic;
public class Spawner : MonoBehaviour {
    [Header("Spawner Settings")]
    [SerializeField] Ingot.IngotType spawnType;
    public GameObject ingotPrefab;
    public BoxCollider spawnArea;
    public int maxIngots = 100;
    public int createdCount = 0;
    public int createPerFrameOnStart = 2;
    public List<Ingot> availableIngots = new List<Ingot>();
    public float spawnRate = 1;
    public bool spawnEnabled = false;
    public float spawnTimer;
    //public List<Ingot> spawnedIngots = new List<Ingot>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (spawnRate == 0) spawnRate = 1;
    }

    //Instantiate ingots until the maxIngots is reached
    void Update() {
        if (createdCount < maxIngots) {
            for (int i = 0; i < createPerFrameOnStart; i++) {
                CreateIngot();
                createdCount++;
            }
        }

        if (spawnEnabled) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1 / spawnRate) {
                SpawnIngot();
                spawnTimer = 0;
            }
        }

    }
    //Create an ingot at a random spawn point
    public void CreateIngot() {
        if (createdCount > maxIngots) return;
        Ingot ingot = Instantiate(ingotPrefab, new Vector3(-50, -50, -50), Quaternion.identity).GetComponent<Ingot>();
        ingot.SetType(spawnType);
        ingot.gameObject.SetActive(false);
        availableIngots.Add(ingot);
    }
    //spawns an ingot by moving it to a random spawn point and enabling it
    public void SpawnIngot() {
        if (availableIngots.Count == 0) return;
        Ingot ingot = availableIngots[0];
        availableIngots.RemoveAt(0);

        ingot.transform.SetPositionAndRotation(
            GetRandomSpawnPoint(),
            Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))
            );

        ingot.gameObject.SetActive(true);
    }
    //Recycles an ingot by disabling it and adding it to the available ingots list
    public void RecycleIngot(Ingot ingot) {
        if (ingot.ingotType != spawnType) throw new System.Exception("Recycling ingot of wrong type");
        availableIngots.Add(ingot);
        ingot.gameObject.SetActive(false);

    }
    //Returns a random spawn point within the spawn area
    private Vector3 GetRandomSpawnPoint() {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, -1f);
    }

}
