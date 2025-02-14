using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Transforms;

[BurstCompile]
public partial struct DestroyerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DestroyerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        DestroyerData d = SystemAPI.GetSingleton<DestroyerData>();
        float3 boxMin = d.Center - d.Size * 0.5f;
        float3 boxMax = d.Center + d.Size * 0.5f;

        var query = SystemAPI.QueryBuilder()
            .WithAll<CubeData, LocalTransform>()
            .Build();

        var entities = query.ToEntityArray(Allocator.Temp);
        var transforms = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            float3 position = transforms[i].Position;

            bool inside =
                position.x >= boxMin.x && position.x <= boxMax.x &&
                position.y >= boxMin.y && position.y <= boxMax.y &&
                position.z >= boxMin.z && position.z <= boxMax.z;

            if (inside)
            {
                ecb.DestroyEntity(entities[i]);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        entities.Dispose();
        transforms.Dispose();
    }
}