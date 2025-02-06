using System.Collections.Generic;
using UnityEngine;

//temp controller for player input
public class ClickController : MonoBehaviour {
    public Stack<Transform> heldIngots = new Stack<Transform>();
    public static ClickController Instance;

    private Texture2D defaultCursor;
    public Texture2D cursorTexture;
    private float tapTimer = 0f;
    private int tapCount = 0;

    
    
    [SerializeField] private float tapDelay = 0.3f;
    private Forge lastTappedForge = null;

    private Vector3 lastMousePosition;
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
        HandleSweeping();
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

    //Handles the "Broom" sweeping mechanic
    private void HandleSweeping() {
        if (Input.GetMouseButtonDown(1)) 
        {
            Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width/2, cursorTexture.height/2), CursorMode.Auto);
            Broom.Instance.gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1)) {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            Broom.Instance.Clear();
            Broom.Instance.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(1)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Broom.Instance.transform.position = mousePos;

        }
    }

    
    //Handle mouse input
    private void HandleInput() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

            if (hits.Length > 0) {
                foreach (var h in hits) {
                    //left click forge
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
                    //left click conveyor merger
                    if (h.collider.TryGetComponent<ConveyorMerger>(out var merger)) {
                        merger.ToggleOutputMode();
                        break;
                    }
                    //left click conveyor belt
                    if (h.collider.TryGetComponent<ConveyorBelt>(out var belt)) {
                        belt.TryEnableBelt();
                    }
                }
            }
        }
    }
}
