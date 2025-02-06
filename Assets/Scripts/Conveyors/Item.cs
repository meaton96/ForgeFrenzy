using UnityEngine;

//represents an item that was crafted in the smithy
public class Item : ConveyerMovableObject
{
    [SerializeField] private SpriteRenderer spriteRenderer; 
    public enum ItemType {
        Axe,
        Necklace,
        Helmet,
        Arrows,
    }
    public ItemType itemType = ItemType.Axe;
    [SerializeField] private Sprite[] itemSprites;
    public int createdBySmithy = -1;

    public Sprite SetType(ItemType type) {
        itemType = type;
        if ((int)type>0 && (int)type < itemSprites.Length)
            spriteRenderer.sprite = itemSprites[(int)type];
        return itemSprites[(int)type];
    }
}
