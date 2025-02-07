using UnityEngine;
using System.Collections.Generic;

//Controls some basic functions for the conveyor belt game
public class GameController : MonoBehaviour {
    public static GameController Instance;
    public List<Forge> forges = new List<Forge>();
    public List<Smithy> smithies = new List<Smithy>();
    [SerializeField] private float defaultOrderTimer = 60f;
    private Queue<SmithingOrder> queuedOrders = new Queue<SmithingOrder>();


    [SerializeField] private OrderArea topOrderArea;
    [SerializeField] private OrderArea bottomOrderArea;


    [SerializeField] private int minOrderAmount = 5;
    [SerializeField] private int maxOrderAmount = 20;



    //Press space to create a random crafting order 
    public void CreateSmithingOrder() {
        bool top = Random.Range(0, 2) > 0;
        OrderArea orderArea;
        if (top) {
            if (topOrderArea.orderActive && !bottomOrderArea.orderActive) {
                orderArea = bottomOrderArea;
            }
            else {
                orderArea = topOrderArea;
            }
        }
        else {
            if (bottomOrderArea.orderActive && !topOrderArea.orderActive) {
                orderArea = topOrderArea;
            }
            else {
                orderArea = bottomOrderArea;
            }
        }
        var order = new SmithingOrder(
            (Item.ItemType)Random.Range(0, 4), 
            Random.Range(minOrderAmount, maxOrderAmount), 
            defaultOrderTimer);

        orderArea.SetOrder(order);
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            CreateSmithingOrder();
        }
    }
    //Recycle the ingot game object
    public void DestroyIngot(Ingot ingot) {
        forges[(int)ingot.ingotType].DestroyIngot(ingot);
    }
    //Spawns an ingot at a spectific location, provides singleton access to the forges
    public void SpawnIngotAt(Vector2 location, Ingot.IngotType type, int reactivity) {
        forges[(int)type].SpawnAt(location, reactivity);
    }
    //Recycle the item game object
    public void RecycleItem(Item item) {
        smithies[item.createdBySmithy].RecycleItem(item);
    }

    public void ReturnToMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
}
