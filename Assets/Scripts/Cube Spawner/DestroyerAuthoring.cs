using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[DisallowMultipleComponent]
public class DestroyerAuthoring : MonoBehaviour
{
    public Vector3 Size = new Vector3(5, 5, 5);

    class Baker : Baker<DestroyerAuthoring>
    {
        public override void Bake(DestroyerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new DestroyerData
            {
                Center = authoring.transform.position,
                Size = authoring.Size
            });
        }
    }
}
