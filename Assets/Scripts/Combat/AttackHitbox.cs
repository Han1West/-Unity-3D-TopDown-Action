using UnityEngine;

public enum CCType
{ 
    None,
    Falldown,
}



public class AttackHitbox : MonoBehaviour
{
    [SerializeField] public int damage = 10;
    [SerializeField] public float reapplyTime = 0.2f;
    [SerializeField] public CCType ccType = CCType.None;
}
