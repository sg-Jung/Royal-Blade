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

public class Attack1State : State
{
    public Attack1State(PlayerController playerController) : base(playerController) { }

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

public class Attack2State : State
{
    public Attack2State(PlayerController playerController) : base(playerController) { }

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

public class ShieldState : State
{
    public ShieldState(PlayerController playerController) : base(playerController) { }

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

public class RunForwardState : State
{
    public RunForwardState(PlayerController playerController) : base(playerController) { }

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