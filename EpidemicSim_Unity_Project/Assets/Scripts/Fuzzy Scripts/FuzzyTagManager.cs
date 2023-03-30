using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyTagManager 
{
    public enum FuzzySet {
        FZ_SET_PROTECTION,
        FZ_SET_EXPOSURETIME,
        FZ_SET_POSSIBILITY
    }

    public enum ProtectionLevel { 
        LOW,
        NORMAL,
        HIGH
    }

    public enum ExposureLevel
    {
        SHORT,
        MEDIUM,
        LONG
    }

    public enum PossibilityLevel
    {
        IMPOSSIBLE,
        LIKELY,
        GUARANTEE
    }
}
