using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grabber : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    private GameObject selectedObject;
    private Vector3 worldPosition;

    private void Update()
    {
        if (selectedObject != null) 
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Dragable") && EventSystem.current.IsPointerOverGameObject())
                        return;

                    selectedObject = hit.collider.gameObject;
                    Cursor.visible = false;
                }
            }
            else 
            {
                selectedObject.transform.position = new Vector3(Mathf.Round(worldPosition.x), 0f, Mathf.Round(worldPosition.z));
                selectedObject = null;
                Cursor.visible = true;
            }
        }

        if (selectedObject != null) 
        {
            selectedObject.transform.position = new Vector3(worldPosition.x, 0.25f, worldPosition.z);
        }
    }

    public void SpawnObjectAtCursor()
    {
        var instancedObject = Instantiate(objectToSpawn, new Vector3(worldPosition.x, 0.25f, worldPosition.z), Quaternion.identity);
        selectedObject = instancedObject;
    }

    private RaycastHit CastRay() 
    {
        Vector3 screenMousePosFar = new Vector3
        (
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 screenMousePosNear = new Vector3
        (
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

   
}
