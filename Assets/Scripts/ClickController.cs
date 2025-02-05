using System.Collections.Generic;
using UnityEngine;

//temp controller for player input
public class ClickController : MonoBehaviour {
    public Stack<Transform> heldIngots = new Stack<Transform>();
    public static ClickController Instance;

    private float tapTimer = 0f;
    private int tapCount = 0;
    [SerializeField] private float tapDelay = 0.3f; 
    private Forge lastTappedForge = null;

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

        if (tapTimer > 0f) {
            tapTimer -= Time.deltaTime;
            if (tapTimer <= 0f) {
                if (lastTappedForge != null) {
                    lastTappedForge.Spawn(tapCount);
                }
                tapCount = 0;
                lastTappedForge = null;
            }
        }
    }

    private void MoveHeldIngots() {
        if (heldIngots.Count == 0) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heldIngots.Peek().position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            if (hits.Length > 0) {
                foreach (var h in hits) {
                    if (h.collider.TryGetComponent<Forge>(out var forge)) {
                        if (heldIngots.Count > 0)
                            return;

                        if (lastTappedForge == forge) {
                            tapCount++;
                            tapTimer = tapDelay; 
                        }
                        else {
                            if (lastTappedForge != null) {
                                lastTappedForge.Spawn(tapCount);
                            }
                            // Start new sequence.
                            lastTappedForge = forge;
                            tapCount = 1;
                            tapTimer = tapDelay;
                        }
                        break;
                    }

                    if (h.collider.TryGetComponent<ConveyorMerger>(out var merger)) {
                        merger.ToggleOutputMode();
                        break;
                    }
                }
            }
        }
    }
}
