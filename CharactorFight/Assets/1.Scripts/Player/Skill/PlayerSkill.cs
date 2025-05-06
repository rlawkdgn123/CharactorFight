using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ۼ��� : ������
public class PlayerSkill : MonoBehaviour
{
    public SkillData skillData; // ��ų ������(��ũ���ͺ� ������Ʈ) �޾ƿ���
    SkillData_Sub skillDataSub; // ���� �� ���� ���� ������ ������(SkillData_sub) �޾ƿ���
    PlayerSkill_Sub playerSkillSub; //���� �� ���� ������ ������ ��ũ��Ʈ
    public PlayerBase player;

    private int skillDamage;
    private Vector2 skillKnockback;
    private void Awake()
    {
        skillDamage = skillData.damage;
        skillKnockback = skillData.knockback;
    }
    private void Update() {
        //if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == true) // ������ �ٶ󺸰� ������
        if (skillData.skillFire == true && transform.localScale.x < 0) // ������ �ٶ󺸰� ������
        {
            this.transform.Translate(Vector3.left * (skillData.skillSpeed * 0.01f)); // �������� ���ǵ�*0.01f��ŭ �̵�
        }//else if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == false) // �������� �ٶ󺸰� ������
        else if (skillData.skillFire == true && transform.localScale.x >= 0) // �������� �ٶ󺸰� ������
        {
            this.transform.Translate(Vector3.right * (skillData.skillSpeed * 0.01f));// ���������� ���ǵ�*0.01f��ŭ �̵�
        }
    }
    private void OnTriggerEnter2D(Collider2D col) {   // �ݶ��̴�2D�� �����Ǹ�
        if (col.tag.Equals("Enemy") || col.tag.Equals("Player"))  // �±װ��� ���̰ų� �÷��̾��� ���
        {
            PlayerHealth damageable = col.GetComponent<PlayerHealth>();
            if (damageable != null && player.GetCurChoice() != col.GetComponent<PlayerBase>().GetCurChoice())
            {
                bool gotHit = damageable.Hit(skillDamage, skillKnockback, transform.position);
                if (gotHit)
                {
                    Debug.Log(col.name + " hit for " + skillDamage);

                }
            }
            /*Damageable damageable = collision.GetComponent<Damageable>();*/   // �ش� ���� Damageable��ũ��Ʈ ���� (��, �´� ��󿡰� Damageable ��ũ��Ʈ�� �־�� ��)
                                                                                //Hit hit = collision.GetComponent<Hit>();
                                                                                //damageable.Hit(skillData.damage,skillData.knockback);   
            /*damageable.Hit(skillDamage, skillKnockback)*/
            ; // ������ �� �˹� ����


            print("1�� ����");

            if(skillData.explosionPrefab != null)   //  ������ ǥ���� �������� �ִ� ���
            {
                print("2�� ����");
                Vector2 explosionSpawnPos = this.gameObject.transform.position; 
                GameObject explosionPrefab = Instantiate(skillData.explosionPrefab, explosionSpawnPos , Quaternion.identity);
                playerSkillSub = explosionPrefab.GetComponent<PlayerSkill_Sub>();
                playerSkillSub.target = col.gameObject;

                //Destroy(explosionPrefab, skillData_Sub.duration); // �ڽ��� ��ũ���ͺ� ������Ʈ�� �����Ͽ� �̸� ���� ����*/
                if (skillData.skillPenetrate != true) // ������ Ȱ��ȭ���� �ʾ��� ���
                    Destroy(this.gameObject);   // ������Ÿ�� ������ �Ҹ�
            }
            /*if (damageable != null)
            {
                bool gotHit = damageable.Hit(skillData.damage, skillData.knockback);

                if (gotHit)
                {
                    Debug.Log(collision.name + " hit for " + skillData.damage);
                }
            }*/
        }
    }
}