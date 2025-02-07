using UnityEngine;

//Reset area under the floor for recycling items and ingots
public class ResetArea : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other) {
        //Item Layer
        if (other.gameObject.layer == 13) {
            Item.ItemType type = other.GetComponent<Item>().itemType;
            OrderManager.Instance.DeliverItem(type);
            Smithy.Instance.RecycleItem(type, other.gameObject.GetInstanceID());
        }
        //Ingot layers
        else if (other.gameObject.layer > 5 && other.gameObject.layer < 10)
            SpawnController.Instance.ResetIngotPosition((Ingot.IngotType)other.gameObject.layer - 6, other.gameObject.GetInstanceID());
        
    }
}
