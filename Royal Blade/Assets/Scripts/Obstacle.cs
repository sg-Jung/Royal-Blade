using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Setting")]
    public Rigidbody rb;
    public BoxCollider bc;
    public float bounciness;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Player"))
        {
            rb.velocity *= (1f - bounciness);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            ObstacleManager.Instance.ReturnToPool(this.gameObject);
        }
    }


    // ��� �� ���� �� ��� �ÿ� �̹� OnTriggerEnter�Լ��� ������ ������ ������ �� �����ؾ� ��

    private void OnTriggerEnter(Collider other) // KnockBackObstacle(): 0�� -> Attack, 1�� -> Shield
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Instance.isAttack)
            {
                // ���� ó��
                if (!PlayerController.Instance.isRunSkill)
                    UIManager.Instance.AttackImageFill(PlayerController.Instance.attackGageValue);

                health -= PlayerController.Instance.attackDamage;

                if (health <= 0)
                {
                    // ���� ����
                    ObstacleManager.Instance.ReturnToPool(this.gameObject);
                }
                else
                {
                    Debug.Log("attackKnockBackForce");
                    ObstacleManager.Instance.KnockBackObstacle(0);
                }
            }
            else if (PlayerController.Instance.isShield)
            {
                // ��� ó��
                Debug.Log("Shield Player Trigger");
                ObstacleManager.Instance.KnockBackObstacle(1);
            }
        }

    }
}
