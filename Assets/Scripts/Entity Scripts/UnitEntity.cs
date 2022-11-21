using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    // change to enum
    [SerializeField]
    private int _infectionState = 0;

    [SerializeField]
    private List<PlaceEntity> _unitPath = new List<PlaceEntity>();
    [SerializeField]
    private int _pathCounter = 0;

    private void MoveUnit() 
    { 
        // move unit position
    }

    private void Arrive()
    {
        _unitPath[_pathCounter].PlaceContainer.Add(this);
    }

    private void Depart()
    {
        _unitPath[_pathCounter].PlaceContainer.Remove(this);
    }
}
