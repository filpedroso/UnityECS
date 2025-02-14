using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;
using Random = Unity.Mathematics.Random;
using Unity.Rendering;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        double currentTime = SystemAPI.Time.ElapsedTime;

        foreach (var (spawnerData, spawnerTransform) in
                 SystemAPI.Query<RefRW<SpawnerData>, RefRO<LocalTransform>>())
        {
            if (currentTime >= spawnerData.ValueRW.NextSpawnTime)
            {
                // instantiate logic
                Random rand = new Random(spawnerData.ValueRW.RandomSeed);
                float nextWait = rand.NextFloat(2f, 12f);
                spawnerData.ValueRW.NextSpawnTime = currentTime + nextWait;
                spawnerData.ValueRW.RandomSeed = rand.NextUInt();

                // Instantiate the cube
                Entity newCube = state.EntityManager.Instantiate(spawnerData.ValueRW.EntityToSpawn);

                // Random speed
                float randomSpeed = rand.NextFloat(10f, 25f);

                // Assign direction (spawner's forward)
                float3 cubeDirection = spawnerTransform.ValueRO.Forward();

                // Update CubeData
                var cubeData = new CubeData
                {
                    Speed = randomSpeed,
                    Direction = cubeDirection
                };
                state.EntityManager.SetComponentData(newCube, cubeData);

                float4 randomColor = new float4(
                    rand.NextFloat(),
                    rand.NextFloat(),
                    rand.NextFloat(),
                    1f
                );

                if (state.EntityManager.HasComponent<URPMaterialPropertyBaseColor>(newCube))
                {
                    var colorComp = state.EntityManager.GetComponentData<URPMaterialPropertyBaseColor>(newCube);
                    colorComp.Value = randomColor;
                    state.EntityManager.SetComponentData(newCube, colorComp);
                }

                LocalTransform cubeTransform = spawnerTransform.ValueRO;
                state.EntityManager.SetComponentData(newCube, cubeTransform);
            }
        }
    }
}