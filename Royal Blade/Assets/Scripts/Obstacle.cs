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
        if (other.CompareTag("Player") && PlayerController.Instance.isAttack)
        {
            Debug.Log("Hit Player Trigger");

            health -= PlayerController.Instance.attackDamage;

            if (health <= 0)
            {
                // 폭발 구현
            }
            else
            {
                Debug.Log("attackKnockBackForce");
                // 여기서 addForce가 아니라 박스 콜라이더 안에 있는 모든 장애물을 검출해 검출된 모든 오브젝트에 addForce를 주도록 구현
                rb.AddForce(-Vector3.back * attackKnockBackForce, ForceMode.Impulse);
            }
        }
    }
}
