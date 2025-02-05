
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Smithy : MonoBehaviour {
    [SerializeField] private SpriteRenderer ingotIcon;
    private Ingot currentHeldIngot;

    private const int MAX_ITEMS = 10;
    [SerializeField] private List<Item> availableItems = new List<Item>();
    [SerializeField] private List<Item> producedItems = new List<Item>();
    private Queue<Item> queuedProduction = new Queue<Item>();
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject uiParent;
    [SerializeField] private TextMeshProUGUI queueText;
    [SerializeField] private Image queueImage;
    public static readonly Dictionary<Ingot.IngotType, Item.ItemType> smithingRecipes = new Dictionary<Ingot.IngotType, Item.ItemType> {
        {Ingot.IngotType.Copper, Item.ItemType.Axe},
        {Ingot.IngotType.Iron, Item.ItemType.Helmet},
        {Ingot.IngotType.Gold, Item.ItemType.Necklace},
        {Ingot.IngotType.Silver, Item.ItemType.Arrows}
    };
    public float productionTime = 1f;
    private Coroutine smithingCoroutine;
    [SerializeField] private GameObject smithyCloser;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        for (int i = 0; i < MAX_ITEMS; i++) {
            var item = Instantiate(itemPrefab, transform).GetComponent<Item>();
            item.gameObject.SetActive(false);
            availableItems.Add(item);

        }
    }

    // Update is called once per frame
    void Update() {

    }

    private System.Collections.IEnumerator HandleProductionQueue() {
        while (queuedProduction.Count > 0) {
            yield return new WaitForSeconds(productionTime);
            var item = queuedProduction.Dequeue();
            producedItems.Add(item);
            item.gameObject.SetActive(true);
            item.transform.position = new Vector3(transform.position.x + .4f, transform.position.y, 0);
            
        }
        uiParent.SetActive(false);
        ClearCurrentHeldIngot();
        smithyCloser.SetActive(false);
        smithingCoroutine = null;
    }
    private void ProduceItems(int numItems, Item.ItemType itemType) {
        smithyCloser.SetActive(true);
        Sprite itemSprite = null;
        for (int i = 0; i < numItems; i++) {
            var item = availableItems[0];

            itemSprite = item.SetType(itemType);
            availableItems.RemoveAt(0);
            queuedProduction.Enqueue(item);
        }
        queueImage.sprite = itemSprite;
        queueText.text = $"x{numItems}";
        uiParent.SetActive(true);

        if (smithingCoroutine == null) {
            smithingCoroutine = StartCoroutine(HandleProductionQueue());
        }
    }
    public void ClearCurrentHeldIngot() {
        currentHeldIngot = null;
        ingotIcon.color = Color.clear;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Ingot ingot)) {
            if (ingot.isHeld) return;
            if (currentHeldIngot != null) {
               
                int totalReactivity = currentHeldIngot.reactivity + ingot.reactivity;
                
                var itemType = smithingRecipes[ingot.ingotType];
                
                GameController.Instance.DestroyIngot(ingot);
                GameController.Instance.DestroyIngot(currentHeldIngot);
                ProduceItems(totalReactivity, itemType);
                return;
            }
            ingot.insideSmithy = true;
            ingotIcon.color = Ingot.INGOT_COLORS[(int)ingot.ingotType];
            currentHeldIngot = ingot;
            currentHeldIngot.occupiedSmithy = this;
            currentHeldIngot.transform.position = transform.position;
            currentHeldIngot.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
