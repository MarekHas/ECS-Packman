using UnityEngine;
using System.Collections;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct DamageData : IComponentData
{
    public float damgeValue;
}
