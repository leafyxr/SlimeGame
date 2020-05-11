using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SaveData 
{
    public int Checkpoint;//Current Checkpoint
    public float[] Position;
    public int Progression;//Currently Carrying?

    public bool[] Milestones;//List of milestones
}
