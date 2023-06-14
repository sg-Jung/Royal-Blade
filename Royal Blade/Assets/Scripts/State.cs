using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerController _playerController;

    public State(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}

public class IdleState : State
{
    public IdleState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {

    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class AttackState : State
{
    public AttackState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        _playerController.anim.SetBool("IsAttack", true);
        _playerController.anim.SetFloat("AttackSpeed", _playerController.attackSpeed);
        _playerController.isAttack = true;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _playerController.isAttack = false;
    }
}

public class ShieldState : State
{
    public ShieldState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        _playerController.anim.SetBool("IsShield", true);
        _playerController.isShield = true;
        _playerController.sc.radius = _playerController.shieldRadius;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _playerController.anim.SetBool("IsShield", false);
        _playerController.isShield = false;
        _playerController.sc.radius = _playerController.firstRadius;
    }
}

public class RunForwardState : State
{
    public RunForwardState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        _playerController.isRun = true;
        _playerController.anim.SetBool("IsRun", true);
        _playerController.anim.SetFloat("RunSpeed", PlayerController.Instance.runSpeed);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _playerController.anim.SetBool("IsRun", false);
        _playerController.isRun = false;
    }
}