using UnityEngine;
using Unity.Entities;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject EntityToSpawn;
    public int AmountToSpawn = 1;
    [Tooltip("Override this with a nonzero to get a repeatable seed if desired.")]
    public uint RandomSeedOverride = 0;

    public class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            // If user did not specify a seed, pick one now
            uint finalSeed = authoring.RandomSeedOverride != 0
                ? authoring.RandomSeedOverride
                : (uint)UnityEngine.Random.Range(1, int.MaxValue);

            AddComponent(entity, new SpawnerData
            {
                EntityToSpawn = GetEntity(authoring.EntityToSpawn, TransformUsageFlags.Dynamic),
                NextSpawnTime = 0.0,   // Start spawning immediately
                RandomSeed = finalSeed
            });
        }
    }
}