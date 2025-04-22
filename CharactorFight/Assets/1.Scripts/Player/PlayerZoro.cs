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
        if (currentChoice == PlayerChoice.P1 && !getKeyIgnore)
        {
            moveInput.x = Input.GetAxisRaw("P1 Horizontal");
            // ������ ������ ���� Ű �Է� üũ
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) { tapCount++; }
            print(moveInput.x * CurrentMoveSpeed);
            
        }
        else if (currentChoice == PlayerChoice.P2 && !getKeyIgnore)
        {
            moveInput.x = Input.GetAxisRaw("P2 Horizontal");
            // ������ ������ ���� Ű �Է� üũ
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) { tapCount++; }
            print(moveInput.x * CurrentMoveSpeed);
        }

        if (IsAlive)
            IsMove = moveInput != Vector2.zero;
        else
            IsMove = false;

        DoubleTap(); // ������ ����
        SetFacingDirection(); // ���� �ø�
    }
    private void FixedUpdate()
    {
        if (IsMove)
        {
            OnMove();
        }
    }
    private void OnMove()
    {
        if (moveInput.x != 0)
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocityY);

        if (!IsRun)
            ClampHorizontalSpeed(walkMaxAcceleration);
        else
            ClampHorizontalSpeed(runMaxAcceleration);

        if (moveInput.x == 0 && !IsDash && td.IsGrounded) // ����
        {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, deceleration * Time.fixedDeltaTime);
        }
    }
    
}

    
    
