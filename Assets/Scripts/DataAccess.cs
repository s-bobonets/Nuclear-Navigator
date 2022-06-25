using UnityEngine;

public class DataAccess : MonoBehaviour
{
    [Header("Particles")] [Space]
    public GameObject goExplosion;
    public GameObject goExhaust;
    public GameObject goWarp;
    [Space]
    public ParticleSystem[] psExhausts;
    public ParticleSystem[] psStreamsL;
    public ParticleSystem[] psStreamsR;
    [Space]
    public MeshRenderer[] shipMRenderers;
}