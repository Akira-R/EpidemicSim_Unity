using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public partial class AgentBehaviorSystem : SystemBase
{
    protected override void OnUpdate()
    {
        double time = Time.ElapsedTime;

        Entities.ForEach((ref Translation translation, ref Rotation rotation, ref AgentBehaviorData agentData) =>
        {
            agentData._moveRate = UnityEngine.Random.Range(0.1f, 1.0f);
            translation.Value.y = Mathf.Sin((float)(agentData._moveRate * time));
        }).Run();

    }
}
