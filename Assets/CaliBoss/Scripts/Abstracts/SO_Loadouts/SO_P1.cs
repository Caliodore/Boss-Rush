using UnityEngine;

[CreateAssetMenu(fileName = "SO_P1", menuName = "Scriptable Objects/SO_P1")]
public class SO_P1 : EnemyFramework_SO
{
    [Header("Phase 1 Specific Vars")]
    [SerializeField] public bool IsAware;
    [SerializeField] public bool IsBuffing;
    [SerializeField] public bool IsChosen;
    [SerializeField] public bool IsDying;
    
    [SerializeField] public float BuffMod;
    [SerializeField] public float BodyScale;
    [SerializeField] public float HealthBarPercent;
}
