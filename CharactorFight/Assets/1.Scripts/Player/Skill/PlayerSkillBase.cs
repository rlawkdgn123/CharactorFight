using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillBase : MonoBehaviour
{
    [Header("DefaultSettings")]
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] private GameObject ui;
    public PlayerBase player;
    public PlayerMana pm;

    [Header("SkillSettings")]
    public Image[] SkillIcon;
    public Image[] SkillIconShadow;
    public SkillData[] skillList;// ��� ������ ��ų ���
    //public bool skillOn;
    [SerializeField] private Vector3 scale;
    public bool[] skillOn = new bool[2];//��ų Ȱ��ȭ ����
    public bool skillCoolDown = false; // ��ų ��Ÿ�� ������ ����;
    //[SerializeField] private bool skillManaCheck = false; //��ų ��� ���� Ȯ�� ����
    [SerializeField] public int selectedSkillIndex;
    [SerializeField] private bool isCoolDown = false;
    [SerializeField] private bool isSkill = false;
    //[SerializeField] private PlayerSoundSystem soundSystem;
    private void Start() {
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
        this.tag = "Player";

        player = gameObject.GetComponent<PlayerBase>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        pm = gameObject.GetComponent<PlayerMana>();

        ui = player.GetPlayerUI();
        SkillIcon = new Image[skillList.Length];
        SkillIconShadow = new Image[skillList.Length];
        string barName = "";

        if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_PlayerSkillList"; }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_PlayerSkillList"; }

        Transform tr = ui.transform.Find(barName);
        if(tr != null)
        {
            for (int i = 0; i < skillList.Length; i++)
            {
                if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_SkillSprite" + (i + 1); }
                else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_SkillSprite" + (i + 1); }

                Transform tr2 = tr.Find(barName);

                if (tr2 != null)
                {
                    IconFill icf = tr2.GetComponent<IconFill>();
                    icf.FillIcon(skillList[i].skillIcon);
                    SkillIcon[i] = icf.images[2];
                    SkillIconShadow[i] = icf.images[1];
                }
                else { print(barName + "�� ã�� �� �����ϴ�."); }
            }
        }
        else
        {
            print(barName + "�� ã�� �� �����ϴ�.");
        }
        
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
    public void OnSkill1()  
    {
        if (player.GetCurDirSetGround() && !isSkill) // ��ų Ȱ��ȭ(��ٿ��� �����ִ���) ���� �� ���� �ִ� �� Ȯ��
        {
            Debug.Log("��� 1");
            selectedSkillIndex = 0;
            
            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // ���� ����(��ٿ�) + ���� �Ҹ��� ���� �̻��̸� ��ų ����
            {
                Debug.Log("��ų��� 2");
                skillOn[selectedSkillIndex] = true;    // �ݺ� ��ų ��� ����
                anim.SetTrigger(AnimationStrings.SkillTrigger2);    // ��ų ��� ĳ���� �ִϸ��̼� ����
                 pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // ���� �Ҹ𷮿� ���� ����
                StartCoroutine(SkillSpawn(selectedSkillIndex));  // ��ų ������ ����
            }
            else
            {
                Debug.Log("��ų��� 1 ����");
            }
        }
    }
    public void OnSkill2()  
    {
        if (player.GetCurDirSetGround() && !isSkill) // ��ų Ȱ��ȭ(��ٿ��� �����ִ���) ���� �� ���� �ִ� �� Ȯ��
        {
            Debug.Log("��� 1");
            selectedSkillIndex = 1;

            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // ���� ����(��ٿ�) + ���� �Ҹ��� ���� �̻��̸� ��ų ����
            {
                Debug.Log("��ų��� 1");
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
            
            skillList[selectedSkillIndex].projectilePrefab.GetComponent<SpriteRenderer>().flipX = false;
            spawnPosition = new Vector2(transform.position.x + skillList[selectedSkillIndex].spawnPosx, transform.position.y + skillList[selectedSkillIndex].spawnPosy);
            //soundSystem.SkillSound(selectedSkillIndex);
            GameObject skill = Instantiate(skillList[selectedSkillIndex].projectilePrefab, spawnPosition, Quaternion.identity);
            if (skillList[selectedSkillIndex].spawnEffect && skillList[selectedSkillIndex].spawnEffectPrefab != null)
            {
                
                if (transform.localScale.x < 0) // �÷��̾ ������ �ٶ󺸸�
                {
                    GameObject spawnEffect = Instantiate(skillList[selectedSkillIndex].spawnEffectPrefab, new Vector3(spawnPosition.x - 2, spawnPosition.y, 0), Quaternion.identity);
                    Vector3 localScale = spawnEffect.transform.localScale;
                    spawnEffect.transform.localScale *= (-1);
                    Destroy(spawnEffect, 1f);
                    }
                else
                {
                    GameObject spawnEffect = Instantiate(skillList[selectedSkillIndex].spawnEffectPrefab, new Vector3(spawnPosition.x + 2, spawnPosition.y, 0), Quaternion.identity);
                    Destroy(spawnEffect, 1f);
                }
            }
            skill.GetComponent<PlayerSkill>().player = player;

            if (transform.localScale.x < 0) // �÷��̾ ������ �ٶ󺸸�
            {
                Vector3 localScale = skill.transform.localScale;
                localScale.x = localScale.x * (-1); // ��ų ������
                skill.transform.localScale = localScale;
                print(skill + "��ų ���. ��");
            }
            else { print(skill + "��ų ���. ��"); }

            skill.name = skillList[selectedSkillIndex].name; // ��ų �����Ϳ� ���� �ν����� �̸� ����
            if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { skill.layer = LayerMask.NameToLayer("P1Attack"); }
            else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { skill.layer = LayerMask.NameToLayer("P2Attack");  }
                
            if (skillList[selectedSkillIndex].freezePlayerPos == true)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Invoke("PlayerFreezeStop", skillList[selectedSkillIndex].duration);
            }

            Destroy(skill, skillList[selectedSkillIndex].duration);
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