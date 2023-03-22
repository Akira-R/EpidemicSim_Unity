using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Manager/VariableObject", fileName = "VariableObj")]
public class VariableObject : ScriptableObject
{
    [SerializeField] private Vector2 latlngCoord;

    [Range(10, 1000)]
    [SerializeField] private int populationNumber = 10;
    [Range(0f, 100f)]
    [SerializeField] private float populationProtection;

    [Range(0f, 100f)]
    [SerializeField] private float transmissionRate;
    [Range(0f, 100f)]
    [SerializeField] private float fatalitiesRate;
    [Range(0f, 100f)]
    [SerializeField] private float recoveryRate;

    [SerializeField] private int totalSimulationTime;


    public Vector2 LatlngCoord => latlngCoord;
    public int PopulationNumber => populationNumber;
    public float PopulationProtection => populationProtection;
    public float TransmissionRate => transmissionRate;
    public float FatalitiesRate => fatalitiesRate;
    public float RecoveryRate => recoveryRate;
    public int TotalSimulationTime => totalSimulationTime;

    public void SetLatlngCoord(Vector2 latlng) 
    {
        latlngCoord = latlng;
    }
    public void SetLatlngCoord(float lat, float lng)
    {
        latlngCoord = new Vector2(lat, lng);
    }

}
