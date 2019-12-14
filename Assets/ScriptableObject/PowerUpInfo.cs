using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new power up", menuName = "Power Up")]
public class PowerUpInfo : ScriptableObject
{
    public new string name;
    public float duration;

    public Sprite icon;
}
