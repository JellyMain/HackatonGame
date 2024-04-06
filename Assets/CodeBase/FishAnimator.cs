using System;
using UnityEngine;


public class FishAnimator : MonoBehaviour, IAnimationStateReader
{
    private static readonly int Bite = Animator.StringToHash("Bite");
    private static readonly int EvoHealthy = Animator.StringToHash("EvoHealthy");
    private static readonly int EvoMutated = Animator.StringToHash("EvoMutated");

    private static readonly int damagedStateHash = Animator.StringToHash("Damaged");

    private Animator animator;

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void PlayBite()
    {
        animator.SetTrigger(Bite);
    }


    public void EvolveHealthy()
    {
        animator.SetTrigger(EvoHealthy);
    }


    public void EvolveMutated()
    {
        animator.SetTrigger(EvoMutated);
    }


    public void EnteredState(int stateHash)
    {
        State = StateFor(stateHash);
        StateEntered?.Invoke(State);
    }


    public void ExitedState(int stateHash)
    {
        State = StateFor(stateHash);
        StateExited?.Invoke(State);
    }


    private AnimatorState StateFor(int stateHash)
    {
        AnimatorState state;

        if (stateHash == damagedStateHash)
        {
            state = AnimatorState.Damaged;
        }
        else
        {
            state = AnimatorState.Unknown;
        }

        return state;
    }
}