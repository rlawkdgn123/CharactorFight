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
        if (currentChoice == PlayerChoice.P1 && !getKeyIgnore) // Player 1
        {
            if (td.IsGrounded && Input.GetKeyDown(KeyCode.W)) // ����
            {
                rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
            }

            moveInput.x = Input.GetAxisRaw("P1 Horizontal");

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (lastKey == KeyCode.A)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.A;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (lastKey == KeyCode.D)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.D;
                }
            }
        }
        else if (currentChoice == PlayerChoice.P2 && !getKeyIgnore) // Player 2
        {

            if (td.IsGrounded && Input.GetKeyDown(KeyCode.UpArrow)) // ����
            {
                rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
            }

            moveInput.x = Input.GetAxisRaw("P2 Horizontal");
            // ������ ������ ���� Ű �Է� üũ
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (lastKey == KeyCode.LeftArrow)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.LeftArrow;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (lastKey == KeyCode.RightArrow)
                {
                    tapCount++;
                }
                else
                {
                    tapCount = 1;
                    lastKey = KeyCode.RightArrow;
                }
            }
        }
            if (IsAlive)
                IsMove = moveInput != Vector2.zero;
            else
                IsMove = false;

            if (!allWaysRun)
            {
                DoubleTap(IsRun); // ������ ����
            }
            DoubleTap(IsDash); // ������ ����
            SetFacingDirection(); // ���� �ø�
    }
    private void FixedUpdate()
    {
        if (IsMove)
        {
            OnMove();
        }
        if (moveInput.x == 0 && !IsDash && td.IsGrounded) // ����
        {
            rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, 0, deceleration * Time.fixedDeltaTime);
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
    }
}

    
    
