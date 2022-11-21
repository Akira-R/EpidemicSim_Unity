using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceEntity : MonoBehaviour
{
    [SerializeField]
    private List<UnitEntity> _placeContainer = new List<UnitEntity>();
    public List<UnitEntity> PlaceContainer => _placeContainer;

    [SerializeField]
    private int _trafficCount = 0;
    [SerializeField]
    private int _infectCount = 0;

    private void CheckInfect()
    { 
        // loop check

        // if true infect
    }
}
