using UnityEngine;

public class Item : MonoBehaviour
{

    public enum ItemType {
        Axe,
        Necklace,
        Helmet,
        Arrows,
    }
    public ItemType itemType = ItemType.Axe;

    public void SetType(ItemType type) {
        itemType = type;
    }
}
