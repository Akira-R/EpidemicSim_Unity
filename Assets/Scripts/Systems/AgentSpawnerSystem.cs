using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial class AgentSpawnerSystem : SystemBase
{
    private Entity _agentPrefab;
    private Entity _agentSpawner;
    private Random _random;
    private BeginSimulationEntityCommandBufferSystem _ecbSystem;

    public List<Entity> _entities;

    // Playable limit at 200 (40000 entities total)
    private int perRowColumn = 100;

    protected override void OnStartRunning()
    {
        _random = new Random(1234567890);

        // Get an instance of prefab and create Entity Buffer
        _agentPrefab = GetSingleton<AgentSpawnerData>().Prefab;
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        // Add buffer to the spawner
        _agentSpawner = GetSingletonEntity<LastSpawnAgent>();
        EntityManager.AddBuffer<SpawnedAgentBufferElement>(_agentSpawner);

        _entities = new List<Entity>();

        SpawnAgents();
    }
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            foreach(var entity in _entities) 
            {
                EntityManager.DestroyEntity(entity);
            }
        }
        
    }

    private void SpawnAgents() 
    {
        var ecb = _ecbSystem.CreateCommandBuffer();
        for (int i = 0; i < perRowColumn; i++)
        {
            for (int j = 0; j < perRowColumn; j++)
            {
                var newAgent = ecb.Instantiate(_agentPrefab);
                var newPos = new Translation { Value = new float3(i, 1, j) };
                _entities.Add(newAgent);
                ecb.SetComponent(newAgent, newPos);

                ecb.AppendToBuffer(_agentSpawner, new SpawnedAgentBufferElement { Agent = newAgent });
            }
        }
    }
}
