using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorData", menuName = "Game/DoorData")]

public class DoorData : ScriptableObject
{
    public string lastDoorUsed;
}
