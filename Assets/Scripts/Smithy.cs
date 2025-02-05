using UnityEngine;

public class Smithy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ingotIcon;
    private Ingot currentHeldIngot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClearCurrentHeldIngot() {
        currentHeldIngot = null;
        ingotIcon.color = Color.clear;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent(out Ingot ingot)) {
            if (ingot.isHeld) return;
            if (currentHeldIngot != null) return;
            ingot.insideSmithy = true;
            ingotIcon.color = Ingot.INGOT_COLORS[(int)ingot.ingotType];
            currentHeldIngot = ingot;
            currentHeldIngot.occupiedSmithy = this;
            currentHeldIngot.transform.position = transform.position;
            currentHeldIngot.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
