using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 1차 작성자 : 김재성  2차 작성자 : 김장후
public class PlayerHealth : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; // 데미지 받았을 때 발생하는 유니티 이벤트
    public UnityEvent<int, int> healthChanged; // 체력 변화 시 발생하는 유니티 이벤트
    //public UIManager uiManager;
    public Image hpBar;
    private bool death = false;
    public LayerMask playerMask;
    public LayerMask EnemyMask;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 1000; //    최대 체력
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 1000; //현재 체력

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

        }
    }
    [SerializeField]
    private bool _isAlive = true; // 살아있는 지
    [SerializeField]
    private bool isInvincible = false; // 무적 상태인지




    private float timeSinceHit = 0; //무적 시간 누적 시간
    public float invincibilityTime = 0.25f; //무적 시간

    public bool IsAlive {
        get 
        {
            return _isAlive;       
        }
        set 
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }


    public bool LockVelocity //이동 제한 상태 여부
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        string tag = this.gameObject.tag;
    }

    private void Update() //무적 시간 상태면 누적 시간 갱신하고, 무적이 끝났을 경우 무적 상태 해제
    {
        if (hpBar != null) // 만약 hpBar 이미지가 할당되었다면
            hpBar.fillAmount = ((float)_health / (float)_maxHealth); // hpBar fillAmount값 변경

        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
        if (_health <= 0)
        {
            IsAlive = false;
        }
    }

    public bool Hit(int damage, Vector2 knockback) //데미지 처리 
    {
        if(IsAlive && !isInvincible)
        {   
            Health -= damage;
            isInvincible = true;

            LockVelocity = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            damageableHit?.Invoke(damage, knockback);
            //CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        //Unable to be hit
        return false;
    }

/*    public void GameOver() //  새로 추가된 함수
    {
        this.gameObject.GetComponent<PlayerController>().getKeyIgnore = true; // 죽으면 플레이어 못 움직임
        Stop(); 
        uiManager.Show("오버",true); // 게임오버 팝업
    }*/
    public void Stop()
    {
        StopAllCoroutines();
    }
    public bool damageTest = false;
    public void hitTest(int damageTest)
    {
        Health -= damageTest;
    }

}
