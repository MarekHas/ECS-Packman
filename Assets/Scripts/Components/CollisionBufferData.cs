using UnityEngine;
using System.Collections;
using Unity.Entities;

public struct CollisionBufferData : IBufferElementData
{
    public Entity entity;
}