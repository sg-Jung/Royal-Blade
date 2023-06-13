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
                // ���� ����
            }
            else
            {
                Debug.Log("attackKnockBackForce");
                // ���⼭ addForce�� �ƴ϶� �ڽ� �ݶ��̴� �ȿ� �ִ� ��� ��ֹ��� ������ ����� ��� ������Ʈ�� addForce�� �ֵ��� ����
                rb.AddForce(-Vector3.back * attackKnockBackForce, ForceMode.Impulse);
            }
        }
    }
}
