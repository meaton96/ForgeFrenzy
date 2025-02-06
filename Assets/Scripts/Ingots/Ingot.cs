using System.Collections;
using UnityEngine;

//Represents a 3D Ingot for the Ingot Insanity game mode
public class Ingot : MonoBehaviour {
    public enum IngotType { Copper, Iron, Gold, Silver }
    public IngotType ingotType = IngotType.Copper;
    [SerializeField] private Material[] ingotMaterials;
    [SerializeField] private MeshRenderer ingotRenderer;
    public Collider ingotCollider;
    public Rigidbody ingotRigidbody;
    
    [SerializeField] private float collisionPushForce = 10f;
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetType(IngotType type) {
        ingotType = type;
        ingotRenderer.material = ingotMaterials[(int)ingotType];
    }
    public void HandleSameCollision(Collision collision) {
        ingotRigidbody.AddForce(collision.contacts[0].normal * collisionPushForce, ForceMode.Impulse);
        collision.rigidbody.AddForce(-collision.contacts[0].normal * collisionPushForce, ForceMode.Impulse);
        SpawnController.Instance.SpawnIngotAt(ingotType, collision.contacts[0].point);
    }

    public void OnCollisionEnter(Collision collision) {
        int otherLayer = collision.gameObject.layer;
        if (otherLayer == gameObject.layer) {
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID()) {
                HandleSameCollision(collision);
            }
        }
        else if (otherLayer > 5 && otherLayer < 10) {
            //handle different type collision
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID()) {
                SpawnController.Instance.RecycleIngot((IngotType)otherLayer - 6, collision.gameObject.GetInstanceID());
                SpawnController.Instance.RecycleIngot(ingotType, gameObject.GetInstanceID());
            }
        }
    }
}
