using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicStats", menuName = "Week5/MusicStats")]
public class MusicLevelStats : ScriptableObject
{
    public AudioClip musicTrack;
    public float timeSigniture;
}
