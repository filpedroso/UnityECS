using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public partial struct InputSystem : ISystem
{
    public void OnCreate (ref SystemState state)
    {
        if(SystemAPI.TryGetSingleton(out InputData inputData))
            return;

        state.EntityManager.CreateEntity(typeof(InputData));
    }

    public void OnUpdate (ref SystemState state)
    {
        float2 moveInput = new float2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput = math.normalizesafe(moveInput);
        
        SystemAPI.SetSingleton(new InputData
        {
            MoveInput = moveInput
        });
    }
}