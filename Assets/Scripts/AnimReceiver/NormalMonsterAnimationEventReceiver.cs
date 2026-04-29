using UnityEngine;

public class NormalMonsterAnimationEventReceiver : MonoBehaviour
{
    EnemyTurtle turtle;
    EnemySlime slime;

    void Awake()
    {
        turtle = GetComponentInParent<EnemyTurtle>();
        slime = GetComponentInParent<EnemySlime>();
    }

    public void ActivateTurtleAttack()
    {
        if (turtle != null)
            turtle.ActivateTurtleAttack();
    }

    public void DeactivateTurtleAttack()
    {
        if (turtle != null)
            turtle.DeactivateTurtleAttack();
    }

    public void ActivateSlimeAttack()
    {
        if(slime != null)
            slime.ActivateSlimeAttack();
    }
}
