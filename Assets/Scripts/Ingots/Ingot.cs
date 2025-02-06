using UnityEngine;

//Represents a 3D Ingot for the Ingot Insanity game mode
public class Ingot : MonoBehaviour
{
    public enum IngotType { Copper, Iron, Gold, Silver } 
    public IngotType ingotType = IngotType.Copper;
    [SerializeField] private Material[] ingotMaterials; 
    [SerializeField] private MeshRenderer ingotRenderer; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(IngotType type) {
        ingotType = type;
        ingotRenderer.material = ingotMaterials[(int)ingotType];
    }
}
