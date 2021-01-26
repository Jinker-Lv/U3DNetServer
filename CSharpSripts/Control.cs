using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Control : MonoBehaviour
{
    //描述
    public string desc = "";
    protected enum move { Idle, forward, back }
    protected enum turn { right, Idle, left }
    //获取控制对象的属性
    protected GameObject ControlObject;
    protected Animator animator;
    private move _move=move.Idle;
    private turn _turn=turn.Idle;
    // Start is called before the first frame update
    protected void Start()
    {
        ControlObject = gameObject;
        animator = ControlObject.GetComponent<Animator>();
    }

    protected void moveUpdate(move _move, turn _turn)
    {
        switch (_move)
        {
            case move.back:
                animator.SetTrigger("Back");
                break;
            case move.forward:
                animator.SetBool("isMoving", true);
                break;
            default:
                animator.SetBool("isMoving", false);
                break;
        }
        switch (_turn)
        {
            case turn.right:
                animator.SetBool("TurnRight90", true);
                break;
            case turn.left:
                animator.SetBool("TurnLeft90", true);
                break;
            default:
                animator.SetBool("TurnLeft90", false);
                animator.SetBool("TurnRight90", false);
                break;
        }
        NetManager.send("Enter|127.1.1.1,0,0,0,45");
    }
    protected move Move
    {
        get => _move;
        set
        {
            _move = value;
            moveUpdate(value, Turn);
        }
    }
    protected turn Turn
    {
        get => _turn;
        set
        {
            _turn = value;
            moveUpdate(Move, value);
        }
    }
}
