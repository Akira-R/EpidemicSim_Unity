using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class FuzzyCalculator : MonoBehaviour
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

    private List<FLV_Graph> _FLVs;
    private List<FuzzyRule> _rules;

    private void Start()
    {
        _FLVs.Clear();
        _FLVs.Add(_flvProtection);
        _FLVs.Add(_flvExposure);
        _FLVs.Add(_flvPossibility);

        _rules.Clear();
        _rules.Add(_nonInfectRule);
        _rules.Add(_mildInfectRule);
        _rules.Add(_severeInfectRule);
    }

    [Button]
    private void TestCalculate()
    {
        for (int ruleIndex = 0; ruleIndex < _rules.Count; ruleIndex++)
        {
            List<float> confidences = new List<float>(_rules.Count);

            foreach (FuzzyRuleSet ruleSet in _rules[ruleIndex].fuzzyRuleSets)
            {
                float value_P = _flvProtection.GetConfidenceValue((int)ruleSet._protectionCondition, testProctectionValue);
                float value_E = _flvProtection.GetConfidenceValue((int)ruleSet._exposureCondition, testExposureValue);
                float value_C = (ruleSet._operator != 0) ? Mathf.Max(value_P, value_E) : Mathf.Min(value_P, value_E);

                int index_C = (int)ruleSet._possibilityResult;
                confidences[index_C] = Mathf.Max(confidences[index_C], value_C);
            }

            Debug.Log(confidences);

        }
    }
}
