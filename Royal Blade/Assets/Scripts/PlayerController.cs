using System;
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
    public Renderer render;
    public Color blinkColor;
    public float blinkDuration;

    private Color originPlayerColor;

    [Header("Attack")]
    public ParticleSystem attackParticle;
    public ParticleSystem attackSkillParticle;
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
        fsm.AddState("Attack", new AttackState(this));
        fsm.AddState("AttackSkill", new AttackSkillState(this));
        fsm.AddState("Shield", new ShieldState(this));
        fsm.AddState("RunForward", new RunForwardState(this));

        fsm.SetState("Idle");
    }

    void Start()
    {
        firstRadius = sc.radius;
        originPlayerColor = render.material.color;
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            BaseAttack();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) && !isRun && isGround)
        {
            RunForward();
        }
        else if(Input.GetKeyDown(KeyCode.A) && !isShield)
        {
            Shield();
        }
    }*/

    public void OnClickAttackBtn()
    {
        if (!isAttack)
        {
            if (!isAttackSkill)
                BaseAttack();
            else
                AttackSkill();

        }
    }

    public void OnClickAttackSkillBtn()
    {
        StopGameForSkillStart();
        ChangeBackGroundForSkill(wholeAttackSkillDuration);
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

        StopGameForSkillStart();
        ChangeBackGroundForSkill(runSkillDuration);
        StartCoroutine(RunSkillDuration());
    }

    public void OnClickShieldkBtn()
    {
        if (!isShield)
        {
            Shield();
        }
    }
    void StopGameForSkillStart()
    {
        GameManager.Instance.StopGameForSkill();
    }

    void ChangeBackGroundForSkill(float skillDuration)
    {
        CameraController.Instance.ChangeCameraBackGroundForSkill(skillDuration);
    }
   
    void AttackSound()
    {
        if (audioSrc.isPlaying) audioSrc.Stop();
        
        audioSrc.clip = ac[0];
        audioSrc.Play();
    }

    void AttackSkill()
    {
        fsm.SetState("AttackSkill");
        StartCoroutine(AttackSkillDuration());
    }

    void BaseAttack()
    {
        fsm.SetState("Attack");
        AttackSound();
        AttackOverlap();

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

    void AttackOverlap()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Obstacle");
        Collider[] colliders = Physics.OverlapSphere(sc.bounds.center, attackRadius, layerMask);

        if(colliders != null)
            UIManager.Instance.AttackSkillImageFill(attackGageValue);

        foreach (Collider collider in colliders)
        {
            Obstacle obs = collider.GetComponent<Obstacle>();
            obs.HitFromPlayer(attackDamage);
        }
    }

    void AttackRayCast()
    {
        RaycastHit hit;

        int layerMask = 1 << LayerMask.NameToLayer("Obstacle"); // Obstacle 레이어만 충돌 체크
        Vector3 pos = transform.position + new Vector3(0f, 0.3f, 0f);

        if (Physics.Raycast(pos, transform.forward, out hit, attackRange, layerMask))
        {
            // 충돌한 오브젝트를 공격하는 코드 작성
            GameObject targetObject = hit.collider.gameObject;
            UIManager.Instance.AttackSkillImageFill(attackGageValue);

            targetObject.GetComponent<Obstacle>().HitFromPlayer(attackDamage);
        }

        Debug.DrawRay(pos, transform.forward * attackRange, Color.red, 0.5f);
    }

    IEnumerator IsAttackSkillDuration()
    {
        attackSkillParticle.gameObject.SetActive(true);
        
        float time = 0f;

        while (time < wholeAttackSkillDuration)
        {
            isAttackSkill = true;
            time += Time.deltaTime;
            yield return null;
        }

        isAttackSkill = false;
        attackSkillParticle.gameObject.SetActive(false);
    }

    IEnumerator AttackSkillDuration()
    {
        attackSkill_ing = true;
        isAttack = true;
        UIManager.Instance.attackBtn.interactable = false;

        float originAttackDamage = attackDamage;

        attackDamage = ObstacleManager.Instance.maxHealth;
        attackRange = skillAttackRange;

        CameraShake(attackSkillDuration);
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
        anim.SetBool("IsAttackSkill", false);
    }

    IEnumerator RunSkillDuration()
    {
        isRunSkill = true;

        float originAttackDamage = attackDamage;
        attackDamage = ObstacleManager.Instance.maxHealth;

        CameraShake(runSkillDuration);
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

    void CameraShake(float duration)
    {
        CameraController.Instance.CameraShake(duration);
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

    void TakeDamage()
    {
        GameManager.Instance.LossHeart();
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        // 깜빡이는 효과를 주기 위해 일정 시간 동안 반복
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            // 깜빡이는 색상으로 변경
            render.material.color = blinkColor;

            // 대기
            yield return new WaitForSeconds(blinkDuration / 2f);

            // 원래 색상으로 변경
            render.material.color = originPlayerColor;

            // 대기
            yield return new WaitForSeconds(blinkDuration / 2f);

            elapsedTime += blinkDuration;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.velocity = Vector3.zero;

            if (isGround && !isShield)
            {
                TakeDamage();
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
