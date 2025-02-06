using UnityEngine;
using System.Collections.Generic;
public class Broom : MonoBehaviour
{
    public static Broom Instance;
    public List<(Vector2 offset, Transform ingot, Collider2D collider)> sweptIngots = 
        new List<(Vector2, Transform, Collider2D)>();
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (var (offset, ingotTransform, _)in sweptIngots) {
            ingotTransform.position = transform.position + (Vector3)offset;
        }
    }
    public void Clear() {
        sweptIngots.ForEach(x => x.collider.enabled = true);
        sweptIngots.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer > 5 && collision.gameObject.layer < 10) {
            sweptIngots.Add((collision.transform.position - transform.position, collision.transform, collision));
            collision.enabled = false;
        }
    }
}
