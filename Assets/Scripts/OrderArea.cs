using TMPro;
using UnityEngine;
using UnityEngine.UI;

//The area where the conveyor delivers the items to
public class OrderArea : MonoBehaviour {
    [SerializeField] private Image orderImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Sprite[] itemSprites;

    public SmithingOrder order;
    public bool orderActive = false;
    //Timer turned off for now
    private void Update() {
        //if (order != null) {
        //    order.UpdateOrderTimer();
        //    if (order.orderTimer <= 0) {
        //        orderImage.gameObject.SetActive(false);
        //        amountText.gameObject.SetActive(false);
        //        order = null;
        //    }
        //}
    }
    //Sets the order for the order area and turns it on
    public void SetOrder(SmithingOrder order) {
        this.order = order;
        orderImage.sprite = itemSprites[(int)order.itemType];
        amountText.text = $"{order.amountDelivered}/{order.amountRequested}";
        orderActive = true;
        gameObject.SetActive(true);
    }
    //Handles objects entering the order area
    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.layer == 13) {
            Debug.Log("Item Collision");
            if (collision.TryGetComponent<Item>(out var item)) {
                if (order != null && order.itemType == item.itemType) {
                    order.DeliverItem();
                    GameController.Instance.RecycleItem(item);
                    if (order.OrderComplete()) {
                        
                       
                        order = null;
                        gameObject.SetActive(false);
                    }
                    else {
                        orderImage.sprite = itemSprites[(int)item.itemType];
                        amountText.text = $"{order.amountDelivered}/{order.amountRequested}";
                    }

                }
            }
        }
    }
}

//Represents a smithing order that is the player needs to fulfill
public class SmithingOrder {
    public Item.ItemType itemType;
    public int amountRequested;
    public int amountDelivered;
    public float orderTimer;
    public float orderTime;
    public SmithingOrder(Item.ItemType itemType, int amountRequested, float orderTime) {
        this.itemType = itemType;
        this.amountRequested = amountRequested;
        this.orderTime = orderTime;
        orderTimer = orderTime;
    }

    public void UpdateOrderTimer() {
        orderTimer -= Time.deltaTime;
    }
    public bool OrderComplete() {
        return amountDelivered >= amountRequested;
    }
    public void DeliverItem() {
        amountDelivered++;
    }

}
