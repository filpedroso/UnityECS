using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;


[BurstCompile]
public partial struct CubeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubeData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        double time = SystemAPI.Time.ElapsedTime;

        foreach (var (cube, transform) in
                 SystemAPI.Query<RefRW<CubeData>, RefRW<LocalTransform>>())
        {
            transform.ValueRW = transform.ValueRW.Translate(cube.ValueRW.Direction * cube.ValueRW.Speed * deltaTime);
        }
    }
}