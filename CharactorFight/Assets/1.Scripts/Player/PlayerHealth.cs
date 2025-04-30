using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 1�� �ۼ��� : ���缺  2�� �ۼ��� : ������
public class PlayerHealth : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; // ������ �޾��� �� �߻��ϴ� ����Ƽ �̺�Ʈ
    public UnityEvent<int, int> healthChanged; // ü�� ��ȭ �� �߻��ϴ� ����Ƽ �̺�Ʈ
    //public UIManager uiManager;
    public Image hpBar;
    private bool death = false;
    public LayerMask playerMask;
    public LayerMask EnemyMask;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 1000; //    �ִ� ü��
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
    private int _health = 1000; //���� ü��

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
    private bool _isAlive = true; // ����ִ� ��
    [SerializeField]
    private bool isInvincible = false; // ���� ��������




    private float timeSinceHit = 0; //���� �ð� ���� �ð�
    public float invincibilityTime = 0.25f; //���� �ð�

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


    public bool LockVelocity //�̵� ���� ���� ����
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

    private void Update() //���� �ð� ���¸� ���� �ð� �����ϰ�, ������ ������ ��� ���� ���� ����
    {
        if (hpBar != null) // ���� hpBar �̹����� �Ҵ�Ǿ��ٸ�
            hpBar.fillAmount = ((float)_health / (float)_maxHealth); // hpBar fillAmount�� ����

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

    public bool Hit(int damage, Vector2 knockback) //������ ó�� 
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

/*    public void GameOver() //  ���� �߰��� �Լ�
    {
        this.gameObject.GetComponent<PlayerController>().getKeyIgnore = true; // ������ �÷��̾� �� ������
        Stop(); 
        uiManager.Show("����",true); // ���ӿ��� �˾�
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
