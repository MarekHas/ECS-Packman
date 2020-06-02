using UnityEngine;
using System.Collections;
using Unity.Entities;
[GenerateAuthoringComponent]
public struct CollisionBufferData : IBufferElementData
{
    public Entity entity;
}