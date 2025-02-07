using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Handles some basic smithing orders for the player to fulfill
public class OrderManager : MonoBehaviour
{
    [SerializeField] private Image orderImage;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private GameObject orderParent;
    [SerializeField] private Sprite[] orderSprites;

    [SerializeField] private int minOrderAmount = 20;
    
    [SerializeField] private int maxOrderAmount = 50;
    [SerializeField] private int orderTimer = 20;
    private SmithingOrder currentOrder;
    public static OrderManager Instance;
    private void Awake() {
        Instance = this;
    }
    //Create and add random order
    public void AddRandomOrder() {
        var order = new SmithingOrder(
            (Item.ItemType)Random.Range(0, 4), 
            Random.Range(minOrderAmount, maxOrderAmount), 
            orderTimer);
        AddOrder(order);
    }
    //set the current order update the UI
    public void AddOrder(SmithingOrder order) {
        currentOrder = order;
        orderImage.sprite = orderSprites[(int)order.itemType];
        orderText.text = $"{order.amountDelivered}/{order.amountRequested}";
        orderParent.SetActive(true);
        Debug.Log(order);
    }
    private void Update() {
      //  UpdateOrders();
    }
    //remove the active order
    public void ClearOrder() {
        currentOrder = null;
        orderParent.SetActive(false);
    }
    //deliver the item to the smithy
    //called from collision in ResetArea
    public void DeliverItem(Item.ItemType itemType) {
        if (currentOrder == null) return;

        if (itemType == currentOrder.itemType) {
            currentOrder.DeliverItem();
            orderText.text = $"{currentOrder.amountDelivered}/{currentOrder.amountRequested}";
            if (currentOrder.OrderComplete()) {
                ClearOrder();
            }
        }
    }
    //update order timer, disabled for demo
    public void UpdateOrders() {

        currentOrder.UpdateOrderTimer();
        if (currentOrder.orderTimer <= 0) {
            ClearOrder();
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
    public override string ToString() {
        return $"Order: {itemType} {amountDelivered}/{amountRequested} {orderTimer}";
    }

}
