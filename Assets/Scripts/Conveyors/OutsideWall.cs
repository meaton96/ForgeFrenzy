using UnityEngine;

public class OutsideWall : MonoBehaviour
{
    //Handle objects going off screen
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer > 5 && collision.gameObject.layer < 10) {
            if (collision.TryGetComponent<Ingot>(out var ingot)) {
                GameController.Instance.DestroyIngot(ingot);
            }
        }
        else if (collision.gameObject.layer == 13) {
            if (collision.TryGetComponent<Item>(out var item)) {
                GameController.Instance.RecycleItem(item);
            }
        }
    }
}
