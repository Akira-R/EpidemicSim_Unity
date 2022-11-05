using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    public AnimationCurve _animationCurve;

    [Range(0.0f, 1.0f)]
    public float xVal = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _animationCurve = GetComponent<AnimationCurve>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_animationCurve.Evaluate(xVal));
    }
}
