using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class SyncData : NetworkTransform 
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
