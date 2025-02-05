using UnityEngine;

public class Ingot : ConveyerMovableObject {
    public bool isHeld = false;
    public bool insideSmithy = false;
    public enum IngotType { Copper, Iron, Gold, Silver }
    public IngotType ingotType = IngotType.Copper;
    public Smithy occupiedSmithy = null;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] ingotSprites;

    [SerializeField] private float collisionDisableBeltRadius = 0.5f;

    private const int REACT_LEVEL_2 = 3;
    private const int REACT_LEVEL_3 = 10;

    public int reactivity = 1;

    public static readonly Color[] INGOT_COLORS = {
        new Color(184f / 255f, 115f / 255f, 51f / 255f),
        new Color(121f / 255f, 121f / 255f, 121f / 255f),
        new Color(255f / 255f, 215f / 255f, 0f / 255f),
        new Color(192f / 255f, 192f / 255f, 192f / 255f)
    };

    private void Update() {

    }


    public void SetType(IngotType type) {
        ingotType = type;
        spriteRenderer.color = INGOT_COLORS[(int)ingotType];
        gameObject.layer = (int)ingotType + 6;

    }
    public void SetReactivity(int reactivity) {
        this.reactivity = reactivity;
        if (reactivity < REACT_LEVEL_2) {
            spriteRenderer.sprite = ingotSprites[0];
        }
        else if (reactivity < REACT_LEVEL_3) {
            spriteRenderer.sprite = ingotSprites[1];
        }
    }
    private void CheckForBeltDisable() {
        var hits = Physics2D.OverlapCircleAll(transform.position, collisionDisableBeltRadius);
        if (hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                var hit = hits[i];
                if (hit.gameObject.layer == 11) {
                    hit.gameObject.GetComponent<ConveyorBelt>().DisableBelt();
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == gameObject.layer) {
            Debug.Log("Same type collision");
            if (reactivity <= 0) return;
            SetReactivity(reactivity - 1);

            if (gameObject.GetInstanceID() > collision.gameObject.GetInstanceID()) return; //only handle collision once
            //handle same type collision
            if (reactivity >= 0) {

                if (insideSmithy) {
                    Debug.Log("Inside smithy");
                }
                else {
                    //spawn the duplicate ingot
                    GameController.Instance.SpawnIngotAt(
                        new Vector2(transform.position.x - 1,
                        transform.position.y),
                        ingotType,
                        reactivity);

                    CheckForBeltDisable();
                }

            }
        }
        else if (collision.gameObject.layer > 5 && collision.gameObject.layer < 10) {
            //handle different type collision
            GameController.Instance.DestroyIngot(collision.gameObject.GetComponent<Ingot>());
            GameController.Instance.DestroyIngot(this);
        }
    }

}
