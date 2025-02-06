using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Represents a conveyor belt that moves objects
public class ConveyorBelt : MonoBehaviour {
    [SerializeField] private float pushForce = 100f;
    [SerializeField] private float speed = .1f;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private int length = 10;

    [SerializeField] private Queue<Transform> pieces = new Queue<Transform>();
    private List<SpriteRenderer> directionArrows = new List<SpriteRenderer>();
    private float pieceSize = 1f;
    private float maxDistance;
    [SerializeField] private bool isReversed = false;
    private List<(Rigidbody2D rb, ConveyerMovableObject movableObject)> objectsOnBelt = new List<(Rigidbody2D, ConveyerMovableObject)>();
    [SerializeField] private float maxConveyerSpeed = 1f;
    public int ObjectsOnBeltCount;
    public bool beltEnabled = true;
    void Start() {
        for (int i = transform.childCount - 1; i >= 1; i--) {
            pieces.Enqueue(transform.GetChild(i));
            directionArrows.Add(transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>());
        }


        maxDistance = pieceSize * length - 1;
    }


    #region Update and Belt Movement
    void Update() {
        if (!beltEnabled) return;
        MovePieces();
        WrapAround();
        MoveObjectsOnBelt();

    }
    //Disables the belt
    public void DisableBelt() {
        beltEnabled = false;
        directionArrows.ForEach(arrow => arrow.color = Color.red);
    }
    //Enables the belt
    private void EnableBelt() {
        beltEnabled = true;
        directionArrows.ForEach(arrow => arrow.color = Color.white);
    }
    //Tries to enable the belt if there are no objects on the belt
    public void TryEnableBelt() {
        if (objectsOnBelt.Count == 0) {
            EnableBelt();
        }
    }
    //Move any objects that are on the belt
    private void MoveObjectsOnBelt() {

        if (!objectsOnBelt.Any()) return;

        foreach (var (rb, obj) in objectsOnBelt) {
            if (obj is Ingot ingot && (ingot.isHeld || ingot.insideSmithy)) {
                continue;
            }
            Vector2 conveyorDirection = (isReversed ? -1 : 1) * direction;

            if (rb.linearVelocity.magnitude < maxConveyerSpeed) {
                rb.AddForce(pushForce * Time.deltaTime * conveyorDirection, ForceMode2D.Force);
            }

            if (rb.linearVelocity.magnitude > maxConveyerSpeed) {
                rb.linearVelocity = conveyorDirection * maxConveyerSpeed;
            }
        }
    }

    //Moves the pieces of the conveyor belt
    private void MovePieces() {
        foreach (var piece in pieces) {
            piece.localPosition += (isReversed ? -1 : 1) * speed * Time.deltaTime * Vector3.right;
        }
    }
    //handles the wrapping around of the conveyor belt pieces
    private void WrapAround() {

        if (isReversed) {
            if (pieces.Peek().localPosition.x < 0) {
                var piece = pieces.Dequeue();
                piece.localPosition = Vector3.right * maxDistance;
                pieces.Enqueue(piece);
            }
        }
        else {
            if (pieces.Peek().localPosition.x > maxDistance) {
                var piece = pieces.Dequeue();
                piece.localPosition = Vector3.zero;
                pieces.Enqueue(piece);
            }
        }

    }
    //Reverses the direction of the conveyor belt
    public void ReverseDirection() {
        isReversed = !isReversed;
        List<Transform> piecesList = pieces.ToList();
        piecesList.Reverse();
        pieces = new Queue<Transform>(piecesList);
        foreach (var arrow in directionArrows) {
            arrow.flipY = !isReversed;
        }

    }
    #endregion

    //add or remove the objects from the tracking lists
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("ConveyorMovable")) {
            objectsOnBelt.Add((collision.GetComponent<Rigidbody2D>(), collision.GetComponent<Ingot>()));
            ObjectsOnBeltCount = objectsOnBelt.Count;
        }

    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("ConveyorMovable")) {
            objectsOnBelt.Remove((collision.GetComponent<Rigidbody2D>(), collision.GetComponent<Ingot>()));
            ObjectsOnBeltCount = objectsOnBelt.Count;
        }
    }



}
