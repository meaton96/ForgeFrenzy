using UnityEngine;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite SetType(ItemType type) {
        itemType = type;
        if ((int)type>0 && (int)type < itemSprites.Length)
            spriteRenderer.sprite = itemSprites[(int)type];
        return itemSprites[(int)type];
    }
}
