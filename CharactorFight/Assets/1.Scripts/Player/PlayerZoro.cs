using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerZoro : PlayerBase
{
    void Awake()
    {
        PlayerInitialize();
    }
    void Update()
    {
        DoubleTap(); // 더블탭 감지
    }
    private void FixedUpdate()
    {
        if (currentChoice == PlayerChoice.P1)
        {
            moveInput.x = Input.GetAxisRaw("P1 Horizontal");
            OnMove();
            //print("p1");
            print(CurrentMoveSpeed);
        }
        else if (currentChoice == PlayerChoice.P2)
        {
            moveInput.x = Input.GetAxisRaw("P2 Horizontal");
            OnMove();
            print("p2");
        } else { }
        
    }
    private void OnMove()
    {
        rb.AddForce(new Vector2(moveInput.x * CurrentMoveSpeed, 0), ForceMode2D.Impulse); //Addforce로 인한 가속 형태의 이동

        if (!IsRun)
            ClampHorizontalSpeed(walkMaxAcceleration);
        else
            ClampHorizontalSpeed(runMaxAcceleration);

        /*if (moveInput.x == 0 && !IsDash && touchingDirection.IsGrounded) // 감속
        {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, deceleration * Time.fixedDeltaTime); 
        }*/
    }
    private void ClampHorizontalSpeed(float runMaxAcceleration)
    {
            if (rb.linearVelocityX > runMaxAcceleration && moveInput.x == 1 && !IsDash)
                rb.linearVelocityX = runMaxAcceleration;
            if (rb.linearVelocityX < -runMaxAcceleration && moveInput.x == -1 && !IsDash)
                rb.linearVelocityX = -runMaxAcceleration;
    }
    void DoubleTap()
    {
        if (tapCount > 0 && doubleTapCurTime.start <= doubleTapDetectTime.start)
        {
            //print("시작");
            doubleTapCurTime.start += Time.deltaTime;
            //print(startDoubleTapCurTime);
            if (doubleTapCurTime.start >= doubleTapDetectTime.start)
            {
                tapCount = 0;
                doubleTapCurTime.start = 0;
            }
            else if (doubleTapCurTime.start < doubleTapDetectTime.start && tapCount == 2)
            {
                IsRun = true;
            }
        }
        if (IsRun && moveInput.x == 0)
        {
            doubleTapCurTime.end += Time.deltaTime;
            if (doubleTapCurTime.end >= doubleTapDetectTime.end)
            {
                doubleTapCurTime.start = 0;
                doubleTapCurTime.end = 0;
                IsRun = false;
                tapCount = 0;
            }
        }
    }
}

    
    
