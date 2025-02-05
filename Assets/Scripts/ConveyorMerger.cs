using UnityEngine;

public class ConveyorMerger : MonoBehaviour
{
    [SerializeField] private Vector2 outputDirection = Vector2.right;
    [SerializeField] private Transform arrowSprite;
    [SerializeField] private float outputDistance = .4f;
    [SerializeField] private bool locked = false;

    [SerializeField] private Vector2 outputDir1;
    [SerializeField] private Vector2 outputDir2;

    [SerializeField] ConveyorMerger linkedMerger;
    [SerializeField] ConveyorBelt linkedBelt;
    [SerializeField] bool isLinkedBeltCorrectOrientation = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RotateOutputClockwise() {
        if (locked) return;
        if (outputDirection == Vector2.right)
            SetOutputDirection(Vector2.down);
        else if (outputDirection == Vector2.down)
            SetOutputDirection(Vector2.left);
        else if (outputDirection == Vector2.left)
            SetOutputDirection(Vector2.up);
        else if (outputDirection == Vector2.up)
            SetOutputDirection(Vector2.right);

        
    }
    public void ToggleOutputMode() {
        if (locked) return;
        SetOutputDirection(outputDirection == outputDir1 ? outputDir2 : outputDir1);

        if (linkedMerger != null) {
            if (outputDirection == outputDir2) {
                if (linkedMerger.outputDirection == linkedMerger.outputDir2) {
                    linkedMerger.SetOutputDirection(linkedMerger.outputDir1);

                }
            }
        }
        if (linkedBelt != null) {
            if (outputDirection == outputDir2) {
                if (!isLinkedBeltCorrectOrientation) {
                    linkedBelt.ReverseDirection();
                    isLinkedBeltCorrectOrientation = !isLinkedBeltCorrectOrientation;
                    if (linkedMerger != null) {
                        linkedMerger.isLinkedBeltCorrectOrientation = !linkedMerger.isLinkedBeltCorrectOrientation;
                    }

                }
            }
        }
    }

    public void SetOutputDirection(Vector2 direction) {
        if (locked) return;
        outputDirection = direction;
        arrowSprite.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, outputDirection));
       
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Ingot ingot)) {
            var rb = ingot.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            ingot.transform.position = transform.position + (Vector3)outputDirection * outputDistance;
            rb.AddForce(outputDirection * 100f);
        }
    }
}
