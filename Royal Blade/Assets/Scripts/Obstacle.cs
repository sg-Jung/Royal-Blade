using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Setting")]
    public Rigidbody rb;
    public BoxCollider bc;
    public float health;
    public float shieldKnockBackForce;
    public float attackKnockBackForce;

    public void AttackKnockBack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-Vector3.back * attackKnockBackForce, ForceMode.Impulse);
    }

    public void ShieldKnockBack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-Vector3.back * shieldKnockBackForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) // KnockBackObstacle(): 0번 -> Attack, 1번 -> Shield
    {
        if (other.CompareTag("Player") && PlayerController.Instance.isAttack)
        {
            UIManager.Instance.AttackImageFill(PlayerController.Instance.attackGageValue);

            health -= PlayerController.Instance.attackDamage;

            if (health <= 0)
            {
                // 폭발 구현
            }
            else
            {
                Debug.Log("attackKnockBackForce");
                ObstacleManager.Instance.KnockBackObstacle(0);
            }
        }
        else if(other.CompareTag("Player") && PlayerController.Instance.isShield)
        {
            Debug.Log("Shield Player Trigger");
            ObstacleManager.Instance.KnockBackObstacle(1);
        }

    }
}
