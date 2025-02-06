using UnityEngine;
using System.Collections.Generic;

//Represents a "forge" that spawns ingots
public class Forge : MonoBehaviour
{
    public Ingot.IngotType ingotType = Ingot.IngotType.Copper;
    public int allowedIngotCount = 30; //maximum total ingots allowed to be spawned
    public List<Ingot> availableIngots = new List<Ingot>();
    public List<Ingot> activeIngots = new List<Ingot>();
    [SerializeField] GameObject ingotPrefab;
    
    //Creates all ingots on load
    //Id imagine this would be done during a load screen if it was a heavy process to not need to create ingots mid gameplay
    private void Start() {
        for (int i = 0; i < allowedIngotCount; i++) {
            availableIngots.Add(Instantiate(ingotPrefab, transform.position, Quaternion.identity).GetComponent<Ingot>());
            availableIngots[i].GetComponent<Ingot>().SetType(ingotType);
            availableIngots[i].gameObject.SetActive(false);
        }
    }
    //Spawn a net ingot at the forge
    //uses the tap count to determine the reactivity of the ingot
    public Ingot Spawn(int tapCount) {
        //Debug.Log($"Forge spawn with tap count: {tapCount}");
        if (activeIngots.Count >= allowedIngotCount) return null;
        var ingot = availableIngots[0];
        ingot.SetReactivity(tapCount);
        activeIngots.Add(ingot);
        availableIngots.RemoveAt(0);
        ingot.gameObject.SetActive(true);

        return ingot;
    }
    //Spawns an ingot at a specific location with a specific reactivity
    //called when an ingot collides with another of its type on a conveyor belt
    public Ingot SpawnAt(Vector2 location, int reactivity) {
        if (activeIngots.Count >= allowedIngotCount) return null;
        var ingot = availableIngots[0];
        activeIngots.Add(ingot);
        availableIngots.RemoveAt(0);
        ingot.gameObject.SetActive(true);
        ingot.transform.position = location;
        ingot.SetReactivity(reactivity);
        return ingot;
    }
    //Recylces the ingot back into the forge for later use
    public void DestroyIngot(Ingot ingot) {
        if (!activeIngots.Contains(ingot)) return;
        if (ingot.isHeld) {
            ClickController.Instance.heldIngots.Clear();
            ingot.isHeld = false;
        }
        if (ingot.insideSmithy) {
            ingot.occupiedSmithy.ClearCurrentHeldIngot();
            ingot.occupiedSmithy = null;
            ingot.insideSmithy = false;
        }


        activeIngots.Remove(ingot);
        availableIngots.Add(ingot);
        ingot.gameObject.SetActive(false);
    }
}
