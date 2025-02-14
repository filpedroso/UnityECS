using Unity.Entities;

public struct SpawnerData : IComponentData
{
    public Entity EntityToSpawn;
    public double NextSpawnTime;
    public uint RandomSeed;

}