using UnityEngine;

public class Collidable : MonoBehaviour
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