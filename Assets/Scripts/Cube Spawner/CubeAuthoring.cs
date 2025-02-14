using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

public class CubeAuthoring : MonoBehaviour
{
    public float Speed = 5f;

    private class Baker : Baker<CubeAuthoring>
    {
        public override void Bake(CubeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            
            AddComponent(entity, new CubeData
            {
                Speed = authoring.Speed,
                Direction = new float3(0, 0, 0),
            });

			AddComponent(entity, new URPMaterialPropertyBaseColor
            {
                Value = new float4(1,1,1,1)
            });
        }
    }
}