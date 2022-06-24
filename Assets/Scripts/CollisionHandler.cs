using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Action<Collide.CollisionType> OnCollide;

    private void OnCollisionEnter(Collision collision)
    {
        var cl = collision.gameObject.GetComponent<Collide>();
        OnCollide?.Invoke(cl.type);
    }
}