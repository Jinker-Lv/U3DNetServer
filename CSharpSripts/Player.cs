using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Control
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move = move.forward;
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            Move = move.Idle;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Turn = turn.left;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Turn = turn.Idle;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Turn = turn.right;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Turn = turn.Idle;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move = move.back;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Move = move.Idle;
        }
    }
}
