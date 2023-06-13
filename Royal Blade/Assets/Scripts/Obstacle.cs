using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Setting")]
    public Rigidbody rb;
    public BoxCollider bc;
    public float health;
    public float knockBackForce;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.Instance.isAttackDelay)
        {
            Debug.Log("Hit Player Trigger");

            health -= PlayerController.Instance.attackDamage;

            if (health <= 0)
            {
                // Æø¹ß ±¸Çö
            }
            else
            {
                Debug.Log("KnockBack");
                rb.AddForce(-Vector3.back * knockBackForce, ForceMode.Impulse);
            }
        }
    }
}
