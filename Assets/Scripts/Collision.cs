using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public enum CollisionType
    {
        Start,
        End,
        Obstacle,
        Bonus
    }

    public CollisionType type;
}