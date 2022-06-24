using UnityEngine;

public class Collide : MonoBehaviour
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