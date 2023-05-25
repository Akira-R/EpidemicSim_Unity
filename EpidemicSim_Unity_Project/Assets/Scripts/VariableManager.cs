using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

using UnityEngine.UI;

public class VariableManager : MonoSingleton<VariableManager>
{
    public VariableObject variables;

    public TMP_InputField latitudeInput;
    public TMP_InputField longitudeInput;

    [BoxGroup("Slider Group")]
    public Slider popNumSlider;
    [BoxGroup("Slider Group")]
    public Slider popProSlider;
    [BoxGroup("Slider Group")]
    public Slider transRateSlider;
    [BoxGroup("Slider Group")]
    public Slider recRateSlider;

    [BoxGroup("Text Group")]
    public TMP_InputField popNumText;
    [BoxGroup("Text Group")]
    public TMP_InputField popProcText;
    [BoxGroup("Text Group")]
    public TMP_InputField transRateText;
    [BoxGroup("Text Group")]
    public TMP_InputField recRateText;

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

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        popNumSlider.onValueChanged.RemoveListener((value) => { variables.PopulationNumber = (int)value; popNumText.text = ((int)value).ToString(); });
        popProSlider.onValueChanged.RemoveListener((value) => { variables.PopulationProtection = value; popProcText.text = value.ToString(); });
        transRateSlider.onValueChanged.RemoveListener((value) => { variables.TransmissionRate = value; transRateText.text = value.ToString(); });
        recRateSlider.onValueChanged.RemoveListener((value) => { variables.RecoveryRate = value; recRateText.text = value.ToString(); });
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

    public void UpdateUI()
    {
        popNumSlider.value = variables.PopulationNumber;
        popProSlider.value = variables.PopulationProtection;
        transRateSlider.value = variables.TransmissionRate;
        recRateSlider.value = variables.RecoveryRate;

        popNumText.text = popNumSlider.value.ToString();
        popProcText.text = popProSlider.value.ToString();
        transRateText.text = transRateSlider.value.ToString();
        recRateText.text = recRateSlider.value.ToString();

        AddlistenerGroup();
    }

    public void AddlistenerGroup() {
        popNumSlider.onValueChanged.AddListener((value) => { variables.PopulationNumber = (int)value; popNumText.text = ((int)value).ToString(); });
        popProSlider.onValueChanged.AddListener((value) => { variables.PopulationProtection = value; popProcText.text = value.ToString(); });
        transRateSlider.onValueChanged.AddListener((value) => { variables.TransmissionRate = value; transRateText.text = value.ToString(); });
        recRateSlider.onValueChanged.AddListener((value) => { variables.RecoveryRate = value; recRateText.text = value.ToString(); });

        popNumText.onValueChanged.AddListener((value) => { variables.PopulationNumber = System.Convert.ToInt32(value); popNumSlider.value = System.Convert.ToInt32(value); });
        popProcText.onValueChanged.AddListener((value) => { variables.PopulationProtection = System.Convert.ToInt32(value); popProSlider.value = System.Convert.ToInt32(value); });
        transRateText.onValueChanged.AddListener((value) => { variables.TransmissionRate = System.Convert.ToInt32(value); transRateSlider.value = System.Convert.ToInt32(value); });
        recRateText.onValueChanged.AddListener((value) => { variables.RecoveryRate = System.Convert.ToInt32(value); recRateSlider.value = System.Convert.ToInt32(value); });
    }
    

}
