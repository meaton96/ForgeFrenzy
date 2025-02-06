using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using PlasticGui.WorkspaceWindow;
public class Spawner : MonoBehaviour {
    [Header("Spawner Settings")]
    [SerializeField] Ingot.IngotType spawnType;
    public GameObject ingotPrefab;
    public BoxCollider spawnArea;
    public int maxIngots = 100;
    public int createdCount = 0;

    public int maxSpawnCount = 20;
    public int spawnCounter = 0;
    public int createPerFrameOnStart = 2;
    //public List<(Ingot ingot, int id)> availableIngots = new List<(Ingot, int)>();
    public Dictionary<int, Ingot> availableIngots = new Dictionary<int, Ingot>();
    public Dictionary<int, Ingot> allIngots = new Dictionary<int, Ingot>();
    public float spawnRate = 1;
    public bool spawnEnabled = false;
    public float spawnTimer;

    [Header("Cannon Settings")]
    public float cannonForce = 200f;
    public Vector3 cannonPosition = new Vector3(-15, 2, 0);
    [Header("Collision Settings")]

    [SerializeField] private float collisionDisableTimer = .2f;
    //public List<Ingot> spawnedIngots = new List<Ingot>();
    private List<(float timer, Collider collider)> spawnedColliders = new List<(float, Collider)>();
    private List<Collider> cannonFiredColliders = new List<Collider>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (spawnRate == 0) spawnRate = 1;
    }

    //Instantiate ingots until the maxIngots is reached
    void Update() {
        HandleIngotCreation();
        HandleIngotSpawning();
        HandleEnablingCollisionSpawnColliders();
        HandleCannonColliders();
    }
    //handle turning back colliders after they enter the play area when fired from the cannon
    public void HandleCannonColliders() {
        if (cannonFiredColliders.Count > 0) {
            for (int i = 0; i < cannonFiredColliders.Count; i++) {
                var collider = cannonFiredColliders[i];
                if (Mathf.Abs(collider.bounds.center.x) < 10.5) {
                    collider.enabled = true;
                    
                    cannonFiredColliders.RemoveAt(i);
                    break;
                }
            }
        }
    }
    //handle turning back on colliders after a short delay when spawning at a location from a collision
    void HandleEnablingCollisionSpawnColliders() {
        
        if (spawnedColliders.Count > 0) {
            for (int i = 0; i < spawnedColliders.Count; i++) {
                float timeLeft = spawnedColliders[i].timer - Time.deltaTime;
                if (timeLeft <= 0) {
                    spawnedColliders[i].collider.enabled = true;
                    spawnedColliders.RemoveAt(i);
                    break;
                }
                else {
                    spawnedColliders[i] = (timeLeft, spawnedColliders[i].collider);
                }
            }
        }
    }

    public void HandleIngotSpawning() {
        //handle ingot spawning
        if (spawnEnabled) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= 1 / spawnRate) {
                if (spawnCounter >= maxSpawnCount)
                    return;
                SpawnIngot();
                spawnTimer = 0;
            }
        }
    }

    public void HandleIngotCreation() {
        //handle ingot creation
        if (createdCount < maxIngots) {
            for (int i = 0; i < createPerFrameOnStart; i++) {
                CreateIngot();
                createdCount++;
            }
        }
    }
    //Create an ingot at a random spawn point
    public void CreateIngot() {
        if (createdCount > maxIngots) return;
        Ingot ingot = Instantiate(ingotPrefab, new Vector3(-50, -50, -50), Quaternion.identity).GetComponent<Ingot>();
        ingot.SetType(spawnType);
        ingot.gameObject.layer = (int)spawnType + 6;
        ingot.gameObject.SetActive(false);

        int id = ingot.gameObject.GetInstanceID();


        allIngots.Add(id, ingot);
        availableIngots.Add(id, ingot);
    }
    //spawns an ingot by moving it to a random spawn point and enabling it
    public void SpawnIngot() {
        if (availableIngots.Count == 0) return;

        SpawnIngotAt(GetRandomSpawnPoint());


        
    }

    public Ingot SpawnIngotAt(Vector3 location, bool reenableCollider = true) {
        if (availableIngots.Count == 0) return null;
        Ingot ingot = availableIngots.First().Value;
        availableIngots.Remove(ingot.gameObject.GetInstanceID());

        ingot.transform.SetPositionAndRotation(
          location,
           Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))
           );

        //temp disable the collider on spawn
        ingot.ingotCollider.enabled = false;
        if (reenableCollider)
            spawnedColliders.Add((collisionDisableTimer, ingot.ingotCollider));
        ingot.gameObject.SetActive(true);
        spawnCounter++;
        return ingot;
    }
    public Ingot SpawnIngotFromCannon(Vector3 targetPosition, bool left = true) {
        if (availableIngots.Count == 0) return null;
        var spawnPos = left ? cannonPosition : new Vector3(-cannonPosition.x, cannonPosition.y, cannonPosition.z);
        Ingot ingot = SpawnIngotAt(spawnPos, false);
        Vector3 direction = targetPosition - spawnPos;
        ingot.ingotRigidbody.AddForce(direction.normalized * cannonForce, ForceMode.Impulse);
        cannonFiredColliders.Add(ingot.ingotCollider);
        return ingot;
    }


    //Recycles an ingot by disabling it and adding it to the available ingots list
    public void RecycleIngot(int id) {
        if (allIngots.TryGetValue(id, out Ingot ingot)) {
            if (ingot.ingotType != spawnType) throw new System.Exception("Recycling ingot of wrong type");
            if (availableIngots.ContainsKey(id)) return;
            availableIngots.Add(id, ingot);
            ingot.gameObject.SetActive(false);
            spawnCounter--;
        }

    }
    public void ResetIngotPosition(int id) {
        if (allIngots.TryGetValue(id, out Ingot ingot)) {
            ingot.transform.position = GetRandomSpawnPoint();
        }
    }
    //Returns a random spawn point within the spawn area
    private Vector3 GetRandomSpawnPoint() {
        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0f);
    }

}
