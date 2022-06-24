using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Action<Collidable.CollisionType> OnCollide;

    private void OnCollisionEnter(Collision collision)
    {
        var cl = collision.gameObject.GetComponent<Collidable>();
        OnCollide?.Invoke(cl.type);
    }
}