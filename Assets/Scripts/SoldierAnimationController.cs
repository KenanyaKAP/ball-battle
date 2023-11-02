using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Soldier))]
public class SoldierAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void ChangeState(SoldierState state) {
        if (state == SoldierState.Running || state == SoldierState.Chasing) {
            animator.SetBool("isRunning", true);
            animator.SetBool("isDying", false);
        }

        if (state == SoldierState.Spawning || state == SoldierState.Defending) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isDying", false);
        }

        if (state == SoldierState.Inactive) {
            animator.SetBool("isRunning", false);
            animator.SetBool("isDying", true);
        }
    }

    public void SetIsSoldierCarryingBall(bool value) {
        animator.SetBool("isCarrying", value);
    }

    public void StopAnimation() {
        animator.enabled = false;
    }
}
