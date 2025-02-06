using UnityEngine;

//Handles converging of conveyer belts
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

    //Toggles between the two output directions for the 
    public void ToggleOutputMode() {
        if (locked) return;
        SetOutputDirection(outputDirection == outputDir1 ? outputDir2 : outputDir1);

        //check if the linked merger would be facing in the same direction as this one and if so rotate it
        if (linkedMerger != null) {
            if (outputDirection == outputDir2) {
                if (linkedMerger.outputDirection == linkedMerger.outputDir2) {
                    linkedMerger.SetOutputDirection(linkedMerger.outputDir1);

                }
            }
        }
        //check if the linked belt needs to be reversed
        //changes the belt in the middle between up or down
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

    //Sets the output direction of the merger
    public void SetOutputDirection(Vector2 direction) {
        if (locked) return;
        outputDirection = direction;
        arrowSprite.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, outputDirection));
       
    }
    //Handle when an ingot enters the merger
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Ingot ingot)) {
            var rb = ingot.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            ingot.transform.position = transform.position + (Vector3)outputDirection * outputDistance;
            rb.AddForce(outputDirection * 100f);
        }
    }
}
