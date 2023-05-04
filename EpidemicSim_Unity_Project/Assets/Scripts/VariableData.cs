using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VariableData
{
    public float latlngCoordX;
    public float latlngCoordY;

    public int populationNumber;
    public float populationProtection;
    public float transmissionRate;
    public float fatalitiesRate;
    public float recoveryRate;
    public int totalSimulationTime;

    public VariableData(VariableObject varObj) 
    {
        latlngCoordX = varObj.LatlngCoord.x;
        latlngCoordY = varObj.LatlngCoord.y;
        populationNumber = varObj.PopulationNumber;
        populationProtection = varObj.PopulationProtection;
        transmissionRate = varObj.TransmissionRate;
        fatalitiesRate = varObj.FatalitiesRate;
        recoveryRate = varObj.RecoveryRate;
        //totalSimulationTime = varObj.TotalSimulationTime;
    }
}
