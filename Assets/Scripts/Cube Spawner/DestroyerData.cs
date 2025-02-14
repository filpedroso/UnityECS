using Unity.Entities;
using Unity.Mathematics;
public struct DestroyerData : IComponentData
{
    public float3 Center;
    public float3 Size;
}
