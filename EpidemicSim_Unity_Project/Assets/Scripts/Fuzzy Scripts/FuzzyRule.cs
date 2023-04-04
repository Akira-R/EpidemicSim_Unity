using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fuzzy Logic/Fuzzy Rule", fileName = "FuzzyRule")]
public class FuzzyRule : ScriptableObject
{
    public enum FuzzyOperation {
        AND,
        OR
    }

    public List<FuzzyRuleSet> fuzzyRuleSets;
}

[System.Serializable]
public class FuzzyRuleSet
{
    public FuzzyRule.FuzzyOperation _operator;

    
    public FuzzyTagManager.ProtectionLevel _protectionCondition;
    public FuzzyTagManager.ExposureLevel _exposureCondition;
    public FuzzyTagManager.PossibilityLevel _possibilityResult;
}
