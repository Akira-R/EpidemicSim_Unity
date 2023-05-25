using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable Manager/VariableObject", fileName = "VariableObj")]
public class VariableObject : ScriptableObject
{
    [SerializeField] private Vector2 latlngCoord;

    [Range(10, 10000)]
    [SerializeField] private int populationNumber = 10;
    [Range(0f, 100f)]
    [SerializeField] private float populationProtection;

    [Range(0f, 100f)]
    [SerializeField] private float transmissionRate;
    [Range(0f, 100f)]
    [SerializeField] private float recoveryRate;

    //[SerializeField] private int totalSimulationTime;


    public Vector2 LatlngCoord => latlngCoord;
    public int PopulationNumber { get { return populationNumber; } set { populationNumber = value; } }
    public float PopulationProtection { get { return populationProtection; } set { populationProtection = value; } }
    public float TransmissionRate { get { return transmissionRate; } set { transmissionRate = value; } }
    public float RecoveryRate { get { return recoveryRate; } set { recoveryRate = value; } }
    //public int TotalSimulationTime => totalSimulationTime;

    public void SetDatafromSave(VariableData data) 
    {
        latlngCoord.x = data.latlngCoordX;
        latlngCoord.y = data.latlngCoordY;
        populationNumber = data.populationNumber;
        populationProtection = data.populationProtection;
        transmissionRate = data.transmissionRate;
        recoveryRate = data.recoveryRate;
        //totalSimulationTime = data.totalSimulationTime;

        VariableManager.Instance?.UpdateUI();
    }

    public void SetLatlngCoord(Vector2 latlng) 
    {
        latlngCoord = latlng;
    }
    public void SetLatlngCoord(float lat, float lng)
    {
        latlngCoord = new Vector2(lat, lng);
    }
}
