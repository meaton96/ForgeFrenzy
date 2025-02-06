using UnityEngine;

public class ResetArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        //Debug.Log(other);
        SpawnController.Instance.ResetIngotPosition((Ingot.IngotType)other.gameObject.layer - 6, other.gameObject.GetInstanceID());
    }
}
