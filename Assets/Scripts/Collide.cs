using UnityEngine;

public class Collide : MonoBehaviour
{
    public enum CollisionType
    {
        Start,
        Finish,
        Obstacle,
        Bonus
    }

    public CollisionType type;
}