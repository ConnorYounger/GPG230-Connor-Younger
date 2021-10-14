using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public EnemyHealth enemyHealth;

    public void DealHeadShotDamage(float damage)
    {
        if (enemyHealth)
        {
            enemyHealth.TakeDamage(damage * enemyHealth.headShotMultiplier);
        }
    }
}