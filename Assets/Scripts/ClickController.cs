using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClickController : MonoBehaviour {
    public Stack<Transform> heldIngots = new Stack<Transform>();
    public static ClickController Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    void Update() {

        HandleInput();
        MoveHeldIngots();
    }
    private void MoveHeldIngots() {
        if (heldIngots.Count == 0) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heldIngots.Peek().position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {

            if (heldIngots.Count > 0) {
               heldIngots.Pop().GetComponent<Ingot>().isHeld = false;
                return;
            }


            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            var hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            if (hits.Length > 0) {
                foreach (var h in hits) {
                    if (h.collider.TryGetComponent<Forge>(out var forge)) {
                        if (heldIngots.Count > 0) return;
                        Ingot i = forge.Spawn();
                        if (i != null) {
                            i.isHeld = true;
                            heldIngots.Push(i.transform);
                        }
                        break;
                    }

                    if (h.collider.TryGetComponent<Ingot>(out var ingot)) {
                        ingot.isHeld = !ingot.isHeld;
                        if (ingot.isHeld) {
                            heldIngots.Push(ingot.transform);
                        }
                        else {
                            heldIngots.Pop();
                        }
                    }
                }
            }

        }
    }
}
