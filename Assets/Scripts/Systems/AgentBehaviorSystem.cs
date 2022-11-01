using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public partial class AgentBehaviorSystem : SystemBase
{
    protected override void OnStartRunning()
    {
        RandomMoveRate();
    }
    protected override void OnUpdate()
    {
        double time = Time.ElapsedTime;

        Entities.ForEach((ref Translation translation, ref AgentBehaviorData agentData) =>
        {
            translation.Value.y = Mathf.Abs(Mathf.Sin((float)time)) * agentData._moveRate;
        }).Run();
    }

    void RandomMoveRate() 
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random(123456789);

        Entities.WithAll<AgentBehaviorData>().ForEach((ref AgentBehaviorData agentData) =>
        {
            agentData._moveRate = random.NextFloat(0.1f, 1.0f);
        }).ScheduleParallel();
    }
}
