using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Setting")]
    public ParticleSystem ps;
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

    public void HitFromPlayer(float attackDamage)
    {
        health -= attackDamage;

        if (health <= 0)
        {
            // 气惯 备泅
            ObstacleManager.Instance.ExplosionObstacle(this.gameObject);
        }
        else
        {
            Debug.Log("attackKnockBackForce");
            ObstacleManager.Instance.KnockBackObstacle(0);
        }
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

    private void OnTriggerEnter(Collider other) // KnockBackObstacle(): 0锅 -> Attack, 1锅 -> Shield
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Instance.isShield)
            {
                // 规绢 贸府
                ObstacleManager.Instance.KnockBackObstacle(1);
            }
        }

    }
}
