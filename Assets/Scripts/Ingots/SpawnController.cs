using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
//Class to provide Singleton access to the Spawners
public class SpawnController : MonoBehaviour
{

    public static SpawnController Instance;
    public List<Spawner> spawners = new List<Spawner>();
    [SerializeField] private GameObject startButton;
    public bool spawningEnabled = false;
    private void Awake() {
        Instance = this;

    }

    
    //Starts the game
    public void BeginSpawningIngots() {
        spawningEnabled = true;
        startButton.SetActive(false);
        spawners.ForEach(spawner => spawner.spawnEnabled = true);
    }
    //Pass through functions to the spawners
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
    public Ingot GetIngotByLayerAndId(int layer, int id) {
        return spawners[layer-6].GetIngotById(id);
    }

    public void ReturnToMainMenu() => SceneManager.LoadScene(0);
}
