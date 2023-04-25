using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class VariableManager : MonoSingleton<VariableManager>
{
    public VariableObject variables;

    public TMP_InputField latitudeInput;
    public TMP_InputField longitudeInput;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Initial Variables:");
        //Debug.Log("Latlng: " + variables.LatlngCoord);
        //Debug.Log("Population Number: " + variables.PopulationNumber);
        //Debug.Log("Population Protection: " + variables.PopulationProtection);
        //Debug.Log("Transmission Rate: " + variables.TransmissionRate);
        //Debug.Log("Fatalities Rate: " + variables.FatalitiesRate);
        //Debug.Log("Recovery Rate: " + variables.RecoveryRate);
        //Debug.Log("Total Sim Time: " + variables.TotalSimulationTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void UpdateLatlngCoords() 
    {
        bool isNumber;
        float lat;
        isNumber = float.TryParse(latitudeInput.text.ToString(), out lat);
        if (!isNumber) 
        {
            Debug.Log("Lat is not a number: " + latitudeInput.text.ToString());
            return;
        }

        float lng;
        isNumber = float.TryParse(longitudeInput.text.ToString(), out lng);
        if (!isNumber) 
        {
            Debug.Log("Lng is not a number:" + longitudeInput.text.ToString());
            return;
        }

        variables.SetLatlngCoord(lat, lng);

        Debug.Log("Latlng: " + variables.LatlngCoord);
    }
}
