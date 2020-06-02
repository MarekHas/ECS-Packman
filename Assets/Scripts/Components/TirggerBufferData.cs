using UnityEngine;
using System.Collections;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct TriggerBufferData : IBufferElementData
{
    public Entity entity;
}
