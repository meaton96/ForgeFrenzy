
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Linq;
//Handles the "Smithy" which handles item creation, and smithing ingots into items after collisions

public class Smithy : MonoBehaviour {
    public static Smithy Instance;
    [Header("Smithy Settings")]
    [SerializeField] private int maxItemCount = 60;
    public float smithingTime = 1f;
    public GameObject[] itemPrefabs = new GameObject[4];


    //item tracking
    public static readonly Dictionary<Ingot.IngotType, Item.ItemType> smithingRecipes = new Dictionary<Ingot.IngotType, Item.ItemType> {
        {Ingot.IngotType.Copper, Item.ItemType.Axe},
        {Ingot.IngotType.Iron, Item.ItemType.Helmet},
        {Ingot.IngotType.Gold, Item.ItemType.Necklace},
        {Ingot.IngotType.Silver, Item.ItemType.Arrows}
    };
    public Dictionary<Item.ItemType, List<Item>> availableItems = new Dictionary<Item.ItemType, List<Item>>();
    public Dictionary<int, Item> smithedItems = new Dictionary<int, Item>();
    public List<(float timer, Ingot ingot)> ingotsToSmith = new List<(float timer, Ingot ingot)>();
    public Dictionary<Item.ItemType, Dictionary<int, Item>> allItems = new Dictionary<Item.ItemType, Dictionary<int, Item>>();

    private int itemsCreatedPerFrame = 1;
    private int itemsCreated = 0;
   
    
    private void Awake() {
        Instance = this;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        for (int i = 0; i < 4; i++) {
            allItems.Add((Item.ItemType)i, new Dictionary<int, Item>());
            availableItems.Add((Item.ItemType)i, new List<Item>());
        }
    }
    //Spawns an item at a location
    void SpawnItemAt(Item.ItemType type, Vector3 location) {
        if (availableItems[type].Count == 0)
            return;
        Item item = availableItems[type].First();
        availableItems[type].RemoveAt(0);
        //item.SetType(type);
        smithedItems.Add(item.gameObject.GetInstanceID(), item);
        item.transform.position = location;
        item.gameObject.SetActive(true);

    }
    //Handles the creation of items on game start
    void HandleItemCreation() {
        if (itemsCreated >= maxItemCount) {
            return;
        }
        for (int i = 0; i < itemsCreatedPerFrame; i++) {
            int itemTypeIndex = (itemsCreated / (maxItemCount / 4));

            var item = Instantiate(itemPrefabs[itemTypeIndex], transform.position, Quaternion.identity).GetComponent<Item>();
            item.gameObject.SetActive(false);
            availableItems[item.itemType].Add(item);
            allItems[(Item.ItemType)itemTypeIndex].Add(item.gameObject.GetInstanceID(), item);
            itemsCreated++;
        }
    }
    //Recycles an item, disabling its game object and adding it back tothe list to be reused
    public void RecycleItem(Item.ItemType type, int id) {

        Item item = allItems[type][id];
        smithedItems.Remove(id);
        item.gameObject.SetActive(false);
        availableItems[type].Add(item);
    }
    void Update() {
        HandleItemCreation();
        HandleItemSpawning();
    }
    //Check the ingotsToSmith list and spawn the item if the timer has expired
    void HandleItemSpawning() {
        for (int i = 0; i < ingotsToSmith.Count; i++) {

            float timeLeft = ingotsToSmith[i].timer - Time.deltaTime;
            if (timeLeft <= 0) {
                Ingot ingot = ingotsToSmith[i].ingot;
                SpawnController.Instance.RecycleIngot(ingot.ingotType, ingot.gameObject.GetInstanceID());
                SpawnItemAt(smithingRecipes[ingot.ingotType], ingot.transform.position);
                ingotsToSmith.RemoveAt(i);
                break;
            }
            else {
                ingotsToSmith[i] = (timeLeft, ingotsToSmith[i].ingot);
            }
        }
    }
    //begin tracking an ingot to be smithed
    public void AddIngotToSmith(Ingot ingot) {
        ingotsToSmith.Add((smithingTime, ingot));
    }
}
