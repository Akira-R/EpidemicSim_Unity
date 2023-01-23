using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEntity : MonoBehaviour
{
    [SerializeField]
    private List<UnitEntity> _placeContainer = new List<UnitEntity>();

    [SerializeField]
    private int _trafficCount = 0;
    [SerializeField]
    private int _infectCount = 0;

    private void UnitArrive(UnitEntity unit)
    {
        _placeContainer.Add(unit);
    }

    private void UnitDepart(UnitEntity unit)
    {
        _placeContainer.Remove(unit);
    }
    
    private void Infect()
    {
        // dispatch event
    }
}
