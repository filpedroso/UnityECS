using UnityEngine;
using Unity.Entities;

public struct PlayerData : IComponentData
{
    public float MoveSpeed;
	public float CurrentYAngle;
}