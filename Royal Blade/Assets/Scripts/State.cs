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

public class Attack0State : State
{
    public Attack0State(PlayerController playerController) : base(playerController) { }

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

public class Attack1State : State
{
    public Attack1State(PlayerController playerController) : base(playerController) { }

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
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class RunForwardState : State
{
    public RunForwardState(PlayerController playerController) : base(playerController) { }

    public override void Enter()
    {
        _playerController.anim.SetBool("IsRun", true);
        _playerController.anim.SetFloat("RunSpeed", PlayerController.Instance.runSpeed);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}