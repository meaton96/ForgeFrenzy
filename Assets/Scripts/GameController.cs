using UnityEngine;
using System.Collections.Generic;
public class GameController : MonoBehaviour {
    public static GameController Instance;
    public List<Forge> forges = new List<Forge>();
    public Transform recentlyCollidedIngot;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyIngot(Ingot ingot) {
        forges[(int)ingot.ingotType].DestroyIngot(ingot);
    }
    public void SpawnIngotAt(Vector2 location, Ingot.IngotType type, int reactivity) {
        forges[(int)type].SpawnAt(location, reactivity);
    }
}
