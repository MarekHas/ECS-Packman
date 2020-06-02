using UnityEngine;
using System.Collections;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct KillData : IComponentData
{
    public float timer;
}
