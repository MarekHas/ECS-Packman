using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class FollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;

        Entities
            .WithAll<Translation, Rotation>()
            .ForEach((Entity entity, in FollowData followData) =>
            {
                if (HasComponent<Translation>(followData.TargetToFollow) && HasComponent<Rotation>(followData.TargetToFollow))
                {
                    var currentPosition = GetComponent<Translation>(entity).Value;
                    var currentRotation = GetComponent<Rotation>(entity).Value;

                    var targetPosition = GetComponent<Translation>(followData.TargetToFollow).Value;
                    var targetRotation = GetComponent<Rotation>(followData.TargetToFollow).Value;

                    targetPosition += math.mul(targetRotation, targetPosition) * -followData.Distance;
                    targetPosition += followData.Offset;

                    targetPosition.x = followData.XposFreeze ? currentPosition.x : targetPosition.x;
                    targetPosition.y = followData.YposFreeze ? currentPosition.y : targetPosition.y;
                    targetPosition.z = followData.ZposFreeze ? currentPosition.z : targetPosition.z;
                    targetRotation = followData.RotationFreeze ? currentRotation : targetRotation;

                    targetPosition = math.lerp(currentPosition, targetPosition, deltaTime * followData.MoveSpeed);
                    targetRotation = math.lerp(currentRotation.value, targetRotation.value, deltaTime * followData.RotationSpeed);

                    SetComponent(entity, new Translation() { Value = targetPosition });
                    SetComponent(entity, new Rotation() { Value = targetRotation });
                }
            }).Schedule();
    }
}