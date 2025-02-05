using UnityEngine;

public class OutsideWall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer > 5 && collision.gameObject.layer < 10) {
            if (collision.TryGetComponent<Ingot>(out var ingot)) {
                GameController.Instance.DestroyIngot(ingot);
            }
        }
    }
}
