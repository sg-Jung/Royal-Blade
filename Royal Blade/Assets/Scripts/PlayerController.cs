using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }
    private FSM fsm;

    [Header("Player Animation")]
    public Animator anim;

    [Header("Player Setting")]
    public Rigidbody rb;
    public int health;

    [Header("Attack")]
    public float attackDamage;
    public float attackSpeed;
    public float attackGageValue;
    public int attackCombo;
    public bool isAttack;

    [Header("Run")]
    public float runSpeed;
    // public float runCoolTime;
    public float runMinPower;
    public float runMaxPower;
    public float runPower;
    public bool isRun;
    public bool isGround;

    [Header("Shield")]
    public bool isShield;

    [Header("Attack Collider")]
    public SphereCollider sc;
    public float firstRadius;
    public float attackRadius;
    public float shieldRadius;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Physics.gravity = new Vector3(0, 0, -9.81f);

        fsm = new FSM();

        fsm.AddState("Idle", new IdleState(this));
        fsm.AddState("Attack0", new Attack0State(this));
        fsm.AddState("Attack1", new Attack1State(this));
        fsm.AddState("Shield", new ShieldState(this));
        fsm.AddState("RunForward", new RunForwardState(this));

        fsm.SetState("Idle");
    }

    void Start()
    {
        sc.radius = firstRadius;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            Attack();
            StartCoroutine(AttackDelay());
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) && !isRun && isGround)
        {
            RunForward();
        }
        else if(Input.GetKeyDown(KeyCode.A) && !isShield)
        {
            Shield();
        }
    }

    public void OnClickAttackBtn()
    {
        if (!isAttack)
        {
            Attack();
            StartCoroutine(AttackDelay());
        }
    }

    public void OnClickRunForwardkBtn()
    {
        if (!isRun && isGround)
        {
            RunForward();
        }
    }

    public void OnClickShieldkBtn()
    {
        if (!isShield)
        {
            Shield();
        }
    }

    void Attack()
    {
        if(attackCombo == 0)
        {
            fsm.SetState("Attack0");
            attackCombo = 1;
            anim.SetInteger("AttackCombo", attackCombo);
        }
        else
        {
            fsm.SetState("Attack1");
            attackCombo = 0;
            anim.SetInteger("AttackCombo", attackCombo);
        }
    }

    IEnumerator AttackDelay()
    {
        sc.radius = attackRadius;
        yield return new WaitForSeconds(1 / attackSpeed);
        sc.radius = firstRadius;
        isAttack = false;
    }

    void Shield()
    {
        fsm.SetState("Shield");
    }

    void RunForward()
    {
        fsm.SetState("RunForward");
        rb.AddForce(-Vector3.back * runPower, ForceMode.Impulse);
    }

    public void ChangeStateToIdle()
    {
        fsm.SetState("Idle");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collision Ground");
            isRun = false;
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

}
