using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillystem : MonoBehaviour
{
    public Image[] SkillIcon = new Image[2];
    public Image[] SkillIconShadow = new Image[2];
    public SkillData[] skillList;// ��� ������ ��ų ���
    public PlayerBase player;
    public PlayerMana pm;
    //public bool skillOn;
    [SerializeField] private Vector3 scale;
    public bool[] skillOn = new bool[2];//��ų Ȱ��ȭ ����
    public bool skillCoolDown = false; // ��ų ��Ÿ�� ������ ����;
    //[SerializeField] private bool skillManaCheck = false; //��ų ��� ���� Ȯ�� ����
    Rigidbody2D rb;
    Animator anim;
    TouchingDirection touchingDirection;
    [SerializeField] public int selectedSkillIndex;
    [SerializeField] private bool isCoolDown = false;
    [SerializeField] private bool isSkill = false;
    //[SerializeField] private PlayerSoundSystem soundSystem;
    private void Awake() {
        PlayerInitialize();

        /*for (int i = 0; i < skillList.Length; i++)
        {
            skillList[i] = skillList[i];
            SkillIcon[i].sprite = skillList[i].skillIcon;
            SkillIconShadow[i].sprite = skillList[i].skillIcon;
        }*/
    }

    public void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        pm = gameObject.GetComponent<PlayerMana>();
        //td = gameObject.GetComponent<TouchingDirection>();
        
    }


    void Update()
    {
        //print(player.skillOn + " " + isCoolDown);
/*        if (skillOn[player.selectedSkillIndex] && !isCoolDown) // ��ų ��Ȱ��ȭ(��ٿ�)���̸�
        {
            print("����");
            isCoolDown = true;
            
        }*/
    }
    public void OnSkill1()   // AŰ, ��� ��ȯ ����
    {
        if (player.GetCurDirState(PlayerBase.DirState.Ground) && !player.GetKeyIgnore() && !isSkill) // ��ų Ȱ��ȭ(��ٿ��� �����ִ���) ���� �� ���� �ִ� �� Ȯ��
        {
            selectedSkillIndex = 0;
            Debug.Log("��ų��� 1");
            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // ���� ����(��ٿ�) + ���� �Ҹ��� ���� �̻��̸� ��ų ����
            {
                skillOn[selectedSkillIndex] = true;    // �ݺ� ��ų ��� ����
                anim.SetTrigger(AnimationStrings.SkillTrigger1);    // ��ų ��� ĳ���� �ִϸ��̼� ����
                 pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // ���� �Ҹ𷮿� ���� ����
                StartCoroutine(SkillSpawn(selectedSkillIndex));  // ��ų ������ ����
            }
            else
            {
                Debug.Log("��ų��� 1 ����");
            }
        }
    }
   

    IEnumerator SkillSpawn(int selectedSkillIndex) { // ��ų ���� �ý���
        // ��ų ��� ����
        //PlayerSkill playerSkill;
        if(!isSkill) // ��ų�� ��� ���� ����
        {
            isSkill = true;
            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y);    // ��ų ���� ���� ���ϱ�
            yield return new WaitForSeconds(skillList[selectedSkillIndex].spawnDelay);
            if (transform.localScale.x >= 0) // �÷��̾ �������� �ٶ󺸰� ���� ��� == true
            {
                skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
                spawnPosition = new Vector2(transform.position.x + skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
                //soundSystem.SkillSound(selectedSkillIndex);
                GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
                skill.name = skillList[selectedSkillIndex].name; // ��ų �����Ϳ� ���� �ν����� �̸� ����
                if (skillList[selectedSkillIndex].freezePlayerPos == true)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
                }

                Destroy(skill, skillList[selectedSkillIndex].duration);
                print(skill + "��ų ���. ��");
            }
            else
            {
                skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
                spawnPosition = new Vector2(transform.position.x - skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
                GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
                Vector3 localScale = skill.transform.localScale;
                localScale.x = localScale.x * (-1); // ��ų ������
                skill.transform.localScale = localScale;

                skill.name = skillList[selectedSkillIndex].name; // ��ų �����Ϳ� ���� �ν����� �̸� ����

                if (skillList[selectedSkillIndex].freezePlayerPos == true)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
                }

                Destroy(skill, skillList[selectedSkillIndex].duration);
                print(skill + "��ų ���. ��");
            }
            
        }
        //skillAudio.clip = skillList[selectedSkillIndex].skillSpawnAudioSource;  // skillAudio �ڽ� ������Ʈ�� ����� ���� �Ҵ� �� ����
        //skillAudio.Play();
        //cameraObj.GetComponent<CameraMange>().VibrateForTime(0.5f);
        yield return new WaitForSeconds(skillList[selectedSkillIndex].motionTime);
        isSkill = false;
        StartCoroutine(SkillCoolDown(selectedSkillIndex));
        //yield return new WaitForSeconds(skillList[selectedSkillIndex].cooldown); // ��Ÿ�� ī��Ʈ
    }
    IEnumerator SkillCoolDown(int selectedSkillIndex) {
        float nowtime = 0;
        int SkillIndex = selectedSkillIndex;
        SkillIcon[SkillIndex].fillAmount = 0f;
        while (nowtime <= skillList[SkillIndex].cooldown)
        {
            nowtime += Time.deltaTime;
            SkillIcon[SkillIndex].fillAmount = nowtime / skillList[SkillIndex].cooldown;
            yield return null;
        }
        skillOn[SkillIndex] = false;     // ��ų ���� ���� ��ȯ
    }
    void PlayerFreezeStop() {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}