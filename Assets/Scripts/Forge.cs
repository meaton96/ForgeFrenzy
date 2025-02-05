using UnityEngine;
using System.Collections.Generic;

public class Forge : MonoBehaviour
{
    public Ingot.IngotType ingotType = Ingot.IngotType.Copper;
    public int allowedIngotCount = 30;
    public List<Ingot> availableIngots = new List<Ingot>();
    public List<Ingot> activeIngots = new List<Ingot>();
    [SerializeField] GameObject ingotPrefab;
    
    private void Start() {
        for (int i = 0; i < allowedIngotCount; i++) {
            availableIngots.Add(Instantiate(ingotPrefab, transform.position, Quaternion.identity).GetComponent<Ingot>());
            availableIngots[i].GetComponent<Ingot>().SetType(ingotType);
            availableIngots[i].gameObject.SetActive(false);
        }
    }
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
