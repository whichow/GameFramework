using UnityEngine;

public class Player : StateMachine
{
    public IState idleState;
    public IState walkState;
    public IState dieState;

    public Animator animator;
    public int step;
    public int health = 100;

    void Start()
    {
        animator = GetComponent<Animator>();

        idleState = new IdleState(this);
        walkState = new WalkState(this);
        dieState = new DieState(this);

        ChangeState(idleState);
    }

    public void ThrowDice()
    {
        step = Random.Range(1, 7);
        Debug.Log(step);
    }

    public void Walk()
    {
        InvokeRepeating("_Walk", 0f, 1f);
    }

    private void _Walk()
    {
        step--;
        health--;
        if(step == 0 || health == 0)
        {
            CancelInvoke("_Walk");
        }
    }
}