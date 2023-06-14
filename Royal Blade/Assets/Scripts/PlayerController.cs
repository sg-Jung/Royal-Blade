using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance { get { return instance; } }
    private FSM fsm;

    [Header("Player Animation & Audio")]
    public Animator anim;
    public AudioSource audioSrc;
    public AudioClip[] ac;

    [Header("Player Setting")]
    public BoxCollider bc;
    public Rigidbody rb;
    public int health;

    [Header("Attack")]
    public float attackDamage;
    public float attackSpeed;
    public float originAttackRange;
    public float skillAttackRange;
    public float attackRange;
    public float attackGageValue;
    public float attackSkillDuration;
    public float wholeAttackSkillDuration;
    public int attackCombo;
    public bool isAttack;
    public bool isAttackSkill;
    public bool attackSkill_ing;

    [Header("Run")]
    public float runSpeed;
    public float runMinPower;
    public float runMaxPower;
    public float runPower;
    public float runSkillPower;
    public float runSkillDuration;
    public bool isRun;
    public bool isRunSkill;
    public bool isGround;

    [Header("Shield")]
    public float shieldCoolTime;
    public bool isShield;

    [Header("Attack Collider")]
    public SphereCollider sc;
    public float firstRadius;
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
        fsm.AddState("Attack", new AttackState(this));
        fsm.AddState("Shield", new ShieldState(this));
        fsm.AddState("RunForward", new RunForwardState(this));

        fsm.SetState("Idle");
    }

    void Start()
    {
        firstRadius = sc.radius;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            Attack();
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
            if (!isAttackSkill)
                Attack();
            else
                AttackSkill();

        }
    }

    public void OnClickAttackSkillBtn()
    {
        StartCoroutine(IsAttackSkillDuration());
    }

    public void OnClickRunForwardkBtn()
    {
        if (!isRun && isGround)
        {
            RunForward();
        }
    }

    public void OnClickRunSkillBtn()
    {
        rb.AddForce(-Vector3.back * runSkillPower, ForceMode.Impulse);
        StartCoroutine(RunSkillDuration());
    }

    public void OnClickShieldkBtn()
    {
        if (!isShield)
        {
            Shield();
        }
    }
   
    void AttackSound()
    {
        if (audioSrc.isPlaying) audioSrc.Stop();
        
        audioSrc.clip = ac[0];
        audioSrc.Play();
    }

    void AttackSkill()
    {
        StartCoroutine(AttackSkillDuration());
    }

    void Attack()
    {
        fsm.SetState("Attack");
        AttackSound();
        AttackRayCast();

        if (attackCombo == 0)
        {
            attackCombo = 1;
            anim.SetInteger("AttackCombo", attackCombo);
        }
        else
        {
            attackCombo = 0;
            anim.SetInteger("AttackCombo", attackCombo);
        }
    }

    void AttackRayCast()
    {
        RaycastHit hit;

        int layerMask = 1 << LayerMask.NameToLayer("Obstacle"); // Obstacle 레이어만 충돌 체크

        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, layerMask))
        {
            // 충돌한 오브젝트를 공격하는 코드 작성
            GameObject targetObject = hit.collider.gameObject;
            Debug.Log("공격 대상: " + targetObject.name);

            UIManager.Instance.AttackSkillImageFill(attackGageValue);

            targetObject.GetComponent<Obstacle>().HitFromPlayer(attackDamage);
        }

        Debug.DrawRay(transform.position, transform.forward * attackRange, Color.red, 0.5f);
    }

    IEnumerator IsAttackSkillDuration()
    {
        isAttackSkill = true;

        yield return new WaitForSeconds(wholeAttackSkillDuration);

        isAttackSkill = false;
    }

    IEnumerator AttackSkillDuration()
    {
        attackSkill_ing = true;
        isAttack = true;
        UIManager.Instance.attackBtn.interactable = false;

        float originAttackDamage = attackDamage;

        attackDamage = ObstacleManager.Instance.maxHealth;
        attackRange = skillAttackRange;

        StartCoroutine(AttackSkillEnd());

        while (attackSkill_ing)
        {
            AttackRayCast();
            yield return null;
        }

        attackDamage = originAttackDamage;
        attackRange = originAttackRange;
    }

    IEnumerator AttackSkillEnd()
    {
        yield return new WaitForSeconds(attackSkillDuration);
        attackSkill_ing = false;
        isAttack = false;
        UIManager.Instance.attackBtn.interactable = true;
    }

    IEnumerator RunSkillDuration()
    {
        isRunSkill = true;

        float originAttackDamage = attackDamage;
        attackDamage = ObstacleManager.Instance.maxHealth;

        StartCoroutine(RunSkillEnd());

        while (isRunSkill)
        {
            AttackRayCast();
            yield return null;
        }

        attackDamage = originAttackDamage;
    }

    IEnumerator RunSkillEnd()
    {
        yield return new WaitForSeconds(runSkillDuration);
        isRunSkill = false;
    }

    void ShieldSound()
    {
        if (audioSrc.isPlaying) audioSrc.Stop();

        audioSrc.clip = ac[1];
        audioSrc.Play();
    }

    void Shield()
    {
        ShieldSound();
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.velocity = Vector3.zero;

            if (isGround && !isShield)
            {
                health--;
                ObstacleManager.Instance.ReturnToPool(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
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
