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
    public int health;
    public float attackDamage;
    public float attackSpeed;
    public bool isAttackDelay;
    public int attackCombo;
    public float runSpeed;

    [Header("Attack Collider")]
    public SphereCollider sc;
    public float firstRadius;
    public float attackRadius;


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
        if (Input.GetKeyDown(KeyCode.Space) && !isAttackDelay)
        {
            isAttackDelay = true;
            Attack();
            StartCoroutine(AttackDelay());
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            RunForward();
        }
        else if(Input.GetKeyDown(KeyCode.A))
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
        isAttackDelay = false;
    }

    void Shield()
    {
        fsm.SetState("Shield");
    }

    void RunForward()
    {
        fsm.SetState("RunForward");
    }

}
