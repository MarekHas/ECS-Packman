using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref PhysicsVelocity physicsVelocity, in MoveData moveData) =>
        {
            var move = moveData.movingDirection * moveData.movingSpeed;
            physicsVelocity.Linear = move;
        }).Schedule();
    }
}