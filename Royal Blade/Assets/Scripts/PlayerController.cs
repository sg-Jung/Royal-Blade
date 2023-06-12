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
    public float runSpeed;


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

        fsm = new FSM();

        fsm.AddState("Idle", new IdleState(this));
        fsm.AddState("Attack1", new Attack1State(this));
        fsm.AddState("Attack2", new Attack2State(this));
        fsm.AddState("Shield", new ShieldState(this));
        fsm.AddState("RunForward", new RunForwardState(this));

        fsm.SetState("Idle");
    }

    void Start()
    {
        health = 3;
        attackDamage = 10f;
        runSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
