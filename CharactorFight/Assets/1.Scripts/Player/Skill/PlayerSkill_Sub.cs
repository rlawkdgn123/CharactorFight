using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill_Sub : MonoBehaviour
{
    public SkillData skillData; // ���� �� ���� ��ų ������(��ũ���ͺ� ������Ʈ) �޾ƿ���
    public SkillData_Sub skillDataSub; // ���� �� ���� ���� ������ ������(SkillData_sub) �޾ƿ���
    public GameObject target;
    Animator anim;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }
    private void Start() 
    {
        Destroy(this.gameObject,skillDataSub.duration);   // ���ӽð� ���� ��ȯ ������ �Ҹ�
    }
    private void Update() 
    {
        if (!skillDataSub.skillAttach)// ���� ���°� �ƴϸ� (�߻�)
        {
            if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == true) // ������ �ٶ󺸰� ������
                this.transform.Translate(Vector3.left * (skillDataSub.skillSpeed * 0.01f)); // �������� ���ǵ�*0.01f��ŭ �̵�
            else if (skillData.skillFire == true && GetComponent<SpriteRenderer>().flipX == false) // �������� �ٶ󺸰� ������
                this.transform.Translate(Vector3.right * (skillDataSub.skillSpeed * 0.01f));// ���������� ���ǵ�*0.01f��ŭ �̵�
        }else if (skillDataSub.skillAttach)
        {
             this.transform.position = target.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   // �ݶ��̴�2D�� �����Ǹ�
        if (collision.tag.Equals("Enemy") || collision.tag.Equals("Boss"))  // �±װ��� ������ ���� ���, ���� �Ҵ� ���� ���� ���� �ݶ��̴��� ���
        {
            /*Damageable damageable = collision.GetComponent<Damageable>();   // �ش� ���� Damageable��ũ��Ʈ ����
            damageable.Hit(skillDataSub.damage, skillDataSub.knockback);    // ������ �� �˹� ����

            if (skillDataSub.Stun == true)
            {
                //StartCoroutine(CCStun(target, skillDataSub.StunTime));  // ������ ���� ����
                StartCoroutine(CCStuntick(target, skillDataSub.StunTime, skillDataSub.stunTick, skillDataSub.stunCycle));  // ������ ���� ����
            }
            if (damageable != null)
            {
                bool gotHit = damageable.Hit(skillData.damage, skillData.knockback);

                if (gotHit)
                {
                    Debug.Log(collision.name + " hit for " + skillData.damage);
                }
            }*/
        }
    }
    private IEnumerator CCStun(GameObject target, float time) { // CC�� - ����
        print("���� ����");
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        target.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.4f);
        targetRb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(time);
        print("���� ����");
        targetRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
    private IEnumerator CCStuntick(GameObject target, float time, float tick, int cycle) {  // CC�� - ���� (���� �� ������)
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        for (int i =0; i < cycle; i++)
        {
            print("���� ����"+i);
            target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
            targetRb.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(time);
            print("���� ����"+i);
            target.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            targetRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return new WaitForSeconds(tick);
        }
    }
}
