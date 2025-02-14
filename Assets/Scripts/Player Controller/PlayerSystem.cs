using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerSystem : ISystem
{
    private Entity playerEntity;
    private Entity inputEntity;
    private PlayerData playerData;
    private InputData inputData;

    public void OnCreate(ref SystemState state)
    {
        // Require that at least one PlayerData component exists in the world
        state.RequireForUpdate<PlayerData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        // Retrieve the entities that hold PlayerData and InputData
        playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();
        inputEntity  = SystemAPI.GetSingletonEntity<InputData>();

        // Read the data from those entities
        playerData = state.EntityManager.GetComponentData<PlayerData>(playerEntity);
        inputData  = state.EntityManager.GetComponentData<InputData>(inputEntity);

        // Handle movement & rotation
        Movement(ref state);

        // Write back any changes to the ECS world
        state.EntityManager.SetComponentData(playerEntity, playerData);
    }

    void Movement(ref SystemState state)
    {
        // Get the player's LocalTransform
        LocalTransform transform = state.EntityManager.GetComponentData<LocalTransform>(playerEntity);

        // Camera vectors for movement
        float3 camRight   = Camera.main.transform.right;
        float3 camForward = Camera.main.transform.forward;

        // Ignore any camera tilt in Y
        camForward.y = 0;
        camForward   = math.normalize(camForward);

        // Calculate the move direction based on input
        float3 moveDir = inputData.MoveInput.x * camRight + inputData.MoveInput.y * camForward;

        // Translate the player
        transform = transform.Translate(moveDir * playerData.MoveSpeed * SystemAPI.Time.DeltaTime);

        //────────────────────────────────────────────────────────────
        // Y-AXIS PIVOT (clamp ±45°) based on horizontal input
        //────────────────────────────────────────────────────────────
        float turnDir = 0f;
        if (inputData.MoveInput.x < 0) turnDir = -1f;
        else if (inputData.MoveInput.x > 0) turnDir = 1f;

        float turnSpeed = 200f;  // degrees per second
        float maxAngle  = 45f;   // clamp range

        // Accumulate rotation angle
        playerData.CurrentYAngle += turnDir * turnSpeed * SystemAPI.Time.DeltaTime;
        playerData.CurrentYAngle  = math.clamp(playerData.CurrentYAngle, -maxAngle, +maxAngle);

        // Convert angle to quaternion
        float angleRadians     = math.radians(playerData.CurrentYAngle);
        quaternion targetRot   = quaternion.Euler(0, angleRadians, 0);

        // Apply rotation
        transform.Rotation = targetRot;

        // Write the updated transform
        state.EntityManager.SetComponentData(playerEntity, transform);
    }
}