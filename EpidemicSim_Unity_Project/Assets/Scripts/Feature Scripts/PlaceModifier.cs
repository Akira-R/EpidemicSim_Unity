using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

using NaughtyAttributes;

public class PlaceModifier : MonoBehaviour
{
    [SerializeField]
    private bool _isEnabled = false;
    private bool _isDragging = false;

    [SerializeField, ReadOnly]
    private GameObject _selectedObject;
    private Vector3 _mousePositionOffset;

    [SerializeField]
    private UITransformTween _buildingPanel;

    private void Update()
    {
        if (!_buildingPanel.GetToggleStatus())
        {
            if(_isEnabled) EntityManager.Instance.SetDisplayMarker(false);
            _isEnabled = false;
        }
        else 
        {
            if (!_isEnabled) EntityManager.Instance.SetDisplayMarker(true);
            _isEnabled = true;

            if (Input.GetMouseButtonDown(0) && !_isDragging)
            {
                _isDragging = true;

                RaycastHit hit = CastRay();
                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("PlaceMarker"))
                    {
                        _isDragging = false;
                        return;
                    }
                    _selectedObject = hit.collider.transform.parent.gameObject;
                    _selectedObject.GetComponent<PlaceEntity>()?.SetNavMeshActive(false);
                    Cursor.visible = false;
                    StartCoroutine(WaitTo_EndDrag());
                }
                else
                {
                    _isDragging = false;
                }
            }
        }
    }

    [Button]
    public void AddPlace()
    {
        EntityManager.Instance.AddPlaceRequest();
    }

    [Button]
    public void RemovePlace()
    {
        if (!_selectedObject) return;
        EntityManager.Instance.RemovePlaceRequest(_selectedObject);
        _selectedObject = null;
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

    private IEnumerator WaitTo_EndDrag()
    {
        //Debug.Log("Drag");
        Vector3 worldPosition;
        Vector3 position;

        position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
        worldPosition = Camera.main.ScreenToWorldPoint(position);
        _mousePositionOffset = _selectedObject.transform.position - worldPosition;

        while (Input.GetMouseButton(0))
        {
            position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z);
            worldPosition = Camera.main.ScreenToWorldPoint(position);
            _selectedObject.transform.position = new Vector3(worldPosition.x, _selectedObject.transform.position.y , worldPosition.z) + new Vector3(_mousePositionOffset.x, 0.0f, _mousePositionOffset.z);
            yield return null;
        }
        EndDrag();
    }

    private void EndDrag()
    {
        //Debug.Log("EndDrag");
        _isDragging = false;
        Cursor.visible = true;
        _selectedObject.GetComponent<PlaceEntity>()?.SetNavMeshActive(true);
    }
}
