using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public void OnFinishedAttack()
    {
        PlayerController.Instance.anim.SetBool("IsAttack", false);
    }

    public void OnFinishedShield()
    {
        PlayerController.Instance.anim.SetBool("IsShield", false);
    }

    public void OnFinishedRunForward()
    {
        PlayerController.Instance.anim.SetBool("IsRun", false);
    }
}
