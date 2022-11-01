using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct AgentBehaviorData : IComponentData
{
    public float _moveRate;
}
