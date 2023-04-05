using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class FuzzyCalculator : MonoSingleton<FuzzyCalculator>
{
    [SerializeField] private FLV_Graph _flvProtection;
    [SerializeField] private FLV_Graph _flvExposure;
    [SerializeField] private FLV_Graph _flvPossibility;

    [SerializeField] private FuzzyRule _nonInfectRule;
    [SerializeField] private FuzzyRule _mildInfectRule;
    [SerializeField] private FuzzyRule _severeInfectRule;

    [Range(0.0f, 100.0f)]
    [SerializeField] private int testProctectionValue;
    [Range(0.0f, 10.0f)]
    [SerializeField] private int testExposureValue;

    private List<FLV_Graph> _FLVs = new List<FLV_Graph>();
    private List<FuzzyRule> _rules = new List<FuzzyRule>();

    private void Start()
    {
        _rules.Clear();
        _rules.Add(_nonInfectRule);
        _rules.Add(_mildInfectRule);
        _rules.Add(_severeInfectRule);
    }

    public int GetHighestProb(float protectionValue, float exposureValue)
    {
        // List of possibility outcome
        List<float> resultPossibilities = new List<float>();
        List<float> representValues = _flvPossibility.GetRepresentativeValue();

        for (int ruleIndex = 0; ruleIndex < _rules.Count; ruleIndex++)
        {
            List<float> confidences = new List<float>();
            for (int i = 0; i < _rules.Count; i++)
                confidences.Add(0.0f);

            foreach (FuzzyRuleSet ruleSet in _rules[ruleIndex].fuzzyRuleSets)
            {
                float value_P = _flvProtection.GetConfidenceValue((int)ruleSet._protectionCondition, testProctectionValue);
                float value_E = _flvExposure.GetConfidenceValue((int)ruleSet._exposureCondition, testExposureValue);
                float value_C = (ruleSet._operator != 0) ? Mathf.Max(value_P, value_E) : Mathf.Min(value_P, value_E);

                //Debug.Log("protection:" + ruleSet._protectionCondition + " & exposure:" + ruleSet._exposureCondition);
                //Debug.Log("value_P: " + value_P + ", value_E: " + value_E + ", value_C: " + value_C + " to " + ruleSet._possibilityResult);

                int index_C = (int)ruleSet._possibilityResult;
                confidences[index_C] = Mathf.Max(confidences[index_C], value_C);
            }

            float totalConfidence = 0;
            float resultProb = 0;

            for (int i = 0; i < confidences.Count; i++)
            {
                resultProb += (float)(confidences[i] * representValues[i]);
                totalConfidence += (float)confidences[i];
            }
            resultPossibilities.Add((float)resultProb / totalConfidence);

            //Debug.Log("rule: " + ruleIndex);
            //for (int i = 0; i < _rules.Count; i++)
            //    Debug.Log("confidences " + i + " : " + confidences[i]);

        }
        //for (int i = 0; i < representValues.Count; i++)
        //    Debug.Log("representValues " + i + " : " + representValues[i]);

        int highestProb = 0;
        for (int i = 0; i < resultPossibilities.Count; i++)
            if (resultPossibilities[i] > resultPossibilities[highestProb])
                highestProb = i;

        return highestProb;
    }

    [Button]
    private void TestCalculate()
    {
        List<float> resultPossibilities = new List<float>();
        List<float> representValues = _flvPossibility.GetRepresentativeValue();

        for (int ruleIndex = 0; ruleIndex < _rules.Count; ruleIndex++)
        {
            List<float> confidences = new List<float>();
            for (int i = 0; i < _rules.Count; i++)
                confidences.Add(0.0f);

            foreach (FuzzyRuleSet ruleSet in _rules[ruleIndex].fuzzyRuleSets)
            {
                float value_P = _flvProtection.GetConfidenceValue((int)ruleSet._protectionCondition, testProctectionValue);
                float value_E = _flvExposure.GetConfidenceValue((int)ruleSet._exposureCondition, testExposureValue);
                float value_C = (ruleSet._operator != 0) ? Mathf.Max(value_P, value_E) : Mathf.Min(value_P, value_E);

                //Debug.Log("protection:" + ruleSet._protectionCondition + " & exposure:" + ruleSet._exposureCondition);
                //Debug.Log("value_P: " + value_P + ", value_E: " + value_E + ", value_C: " + value_C + " to " + ruleSet._possibilityResult);

                int index_C = (int)ruleSet._possibilityResult;
                confidences[index_C] = Mathf.Max(confidences[index_C], value_C);
            }

            float totalConfidence = 0;
            float resultProb = 0;

            for(int i = 0; i < confidences.Count; i++)
            {
                resultProb += (float)(confidences[i] * representValues[i]);
                totalConfidence += (float)confidences[i];
            }
            resultPossibilities.Add((float)resultProb /totalConfidence);

            //Debug.Log("rule: " + ruleIndex);
            //for (int i = 0; i < _rules.Count; i++)
            //    Debug.Log("confidences " + i + " : " + confidences[i]);

        }
        //for (int i = 0; i < representValues.Count; i++)
        //    Debug.Log("representValues " + i + " : " + representValues[i]);

        //for (int i = 0; i < resultPossibilities.Count; i++)
        //    Debug.Log("resultPossibility " + i + " : " + resultPossibilities[i]);
    }
}
