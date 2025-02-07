using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
//basic mouse and keyboard input for game demo
public class InputController : MonoBehaviour {
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float forceStrength = 10f;

 

    private int activeCannon = 0;

    [SerializeField] private List<Image> ingotSelectionHighlights = new List<Image>();

    void Update() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            activeCannon = 0;
            ingotSelectionHighlights.ForEach(x => x.enabled = false);
            ingotSelectionHighlights[0].enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            activeCannon = 1;
            ingotSelectionHighlights.ForEach(x => x.enabled = false);
            ingotSelectionHighlights[1].enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            activeCannon = 2;
            ingotSelectionHighlights.ForEach(x => x.enabled = false);
            ingotSelectionHighlights[2].enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            activeCannon = 3;
            ingotSelectionHighlights.ForEach(x => x.enabled = false);
            ingotSelectionHighlights[3].enabled = true;
        }


        if (Input.GetMouseButtonDown(0)) {
            if (!SpawnController.Instance.spawningEnabled)
                return;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z); 

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            SpawnController.Instance.FireIngotCannon((Ingot.IngotType)activeCannon, new Vector3(worldPos.x, worldPos.y, 0f), worldPos.x < 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            OrderManager.Instance.AddRandomOrder();
        }

    }

    

}
