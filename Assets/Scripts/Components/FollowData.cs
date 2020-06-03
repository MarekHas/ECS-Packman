using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct FollowData : IComponentData
{
    public Entity TargetToFollow;
    public float Distance, MoveSpeed, RotationSpeed;
    public float3 Offset;
    public bool XposFreeze, YposFreeze, ZposFreeze, RotationFreeze;
}
