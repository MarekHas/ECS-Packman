using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class EnemySystem : SystemBase
{
    private Unity.Mathematics.Random randomNumber = new Unity.Mathematics.Random(1234);

    protected override void OnUpdate()
    {
        var raycastMovement = new RaycastMovement() { pw = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld };
        randomNumber.NextInt();
        var tempRandomNumber = randomNumber;

        Entities.ForEach((ref MoveData moveData, ref EnemyData enemyData, in Translation translation) =>
        {
            if (math.distance(translation.Value, enemyData.previousPosition) > 0.9f)
            {
                enemyData.previousPosition = math.round(translation.Value);

                var directionValidation = new NativeList<float3>(Allocator.Temp);

                if (!raycastMovement.RaycastCheck(translation.Value, new float3(0, 0, -1), moveData.movingDirection))
                    directionValidation.Add(new float3(0, 0, -1));
                if (!raycastMovement.RaycastCheck(translation.Value, new float3(0, 0, 1), moveData.movingDirection))
                    directionValidation.Add(new float3(0, 0, 1));
                if (!raycastMovement.RaycastCheck(translation.Value, new float3(-1, 0, 0), moveData.movingDirection))
                    directionValidation.Add(new float3(-1, 0, 0));
                if (!raycastMovement.RaycastCheck(translation.Value, new float3(1, 0, 0), moveData.movingDirection))
                    directionValidation.Add(new float3(1, 0, 0));

                moveData.movingDirection = directionValidation[tempRandomNumber.NextInt(directionValidation.Length)];

                directionValidation.Dispose();
            }
        }).Schedule();
    }

    private struct RaycastMovement
    {
        [ReadOnly] public PhysicsWorld pw;

        public bool RaycastCheck(float3 position, float3 direction, float3 currentDirection)
        {

            if (direction.Equals(-currentDirection))
                return true;

            var ray = new RaycastInput()
            {
                Start = position,
                End = position + (direction * 0.9f),
                Filter = new CollisionFilter()
                {
                    GroupIndex = 0,
                    BelongsTo = 1u << 1,
                    CollidesWith = 1u << 2
                }
            };

            return pw.CastRay(ray);
        }
    }
}
