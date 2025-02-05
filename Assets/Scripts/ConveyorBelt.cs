using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ConveyorBelt : MonoBehaviour {
    [SerializeField] private float pushForce = 100f;
    [SerializeField] private float speed = .1f;
    [SerializeField] private Vector3 direction = Vector3.right;
    [SerializeField] private int length = 10;

    [SerializeField] private Queue<Transform> pieces = new Queue<Transform>();
    private List<SpriteRenderer> directionArrows = new List<SpriteRenderer>();
    private float pieceSize = 1f;
    private float maxDistance;
    private bool isReversed = false;
    private List<(Rigidbody2D rb, Ingot ingot)> objectsOnBelt = new List<(Rigidbody2D, Ingot)>();
    [SerializeField] private float maxConveyerSpeed = 1f;
    public int ObjectsOnBeltCount;
    void Start() {
        for (int i = transform.childCount - 1; i >= 1; i--) {
            pieces.Enqueue(transform.GetChild(i));
            directionArrows.Add(transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>());
        }

        if (length > pieces.Count) {
            var diff = length - pieces.Count;
            var pieceList = pieces.ToList();
            var newPieces = new List<Transform>();

            for (int i = 0; i < diff; i++) {
                var piece = Instantiate(pieces.Peek(),
                    pieces.Peek().position + (i + 1) * pieceSize * transform.localScale.x * Vector3.right,
                    Quaternion.identity,
                    transform);

                newPieces.Add(piece);
                directionArrows.Add(piece.GetChild(0).GetComponent<SpriteRenderer>());
            }

            newPieces.Reverse();
            foreach (var piece in newPieces) {
                pieceList.Insert(0, piece);
            }

            pieces = new Queue<Transform>(pieceList);
        }

        maxDistance = pieceSize * length - 1;
    }


    #region Update and Belt Movement
    void Update() {
        MovePieces();
        WrapAround();
        MoveObjectsOnBelt();
        if (Input.GetKeyDown(KeyCode.Space)) {
            ToggleDirection();
        }
    }

    private void MoveObjectsOnBelt() {
        if (!objectsOnBelt.Any()) return;

        foreach (var (rb, ingot) in objectsOnBelt) {
            if (ingot.isHeld || ingot.insideSmithy) continue;

            Vector2 conveyorDirection = (isReversed ? -1 : 1) * direction.normalized;

            if (rb.linearVelocity.magnitude < maxConveyerSpeed) {
                rb.AddForce(pushForce * Time.deltaTime * conveyorDirection, ForceMode2D.Force);
            }

            if (rb.linearVelocity.magnitude > maxConveyerSpeed) {
                rb.linearVelocity = conveyorDirection * maxConveyerSpeed;
            }
        }
    }


    private void MovePieces() {
        foreach (var piece in pieces) {
            piece.localPosition += (isReversed ? -1 : 1) * speed * Time.deltaTime * direction;
        }
    }
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
    private void ToggleDirection() {
        isReversed = !isReversed;
        List<Transform> piecesList = pieces.ToList();
        piecesList.Reverse();
        pieces = new Queue<Transform>(piecesList);
        foreach (var arrow in directionArrows) {
            arrow.flipY = !isReversed;
        }

    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ingot")) {
            objectsOnBelt.Add((collision.GetComponent<Rigidbody2D>(), collision.GetComponent<Ingot>()));
            ObjectsOnBeltCount = objectsOnBelt.Count;
        }

    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Ingot")) {
            objectsOnBelt.Remove((collision.GetComponent<Rigidbody2D>(), collision.GetComponent<Ingot>()));
            ObjectsOnBeltCount = objectsOnBelt.Count;
        }
    }



}
