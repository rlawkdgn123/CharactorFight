using UnityEngine;

public class playerSkill_Zoro : PlayerSkillBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerInitialize();
    }
    override public void  OnSkill2()
    {
        if (player.GetCurDirSetGround() && !isSkill) // ��ų Ȱ��ȭ(��ٿ��� �����ִ���) ���� �� ���� �ִ� �� Ȯ��
        {
            
            selectedSkillIndex = 1;

            if (!skillOn[selectedSkillIndex] && pm.GetCurMana() >= skillList[selectedSkillIndex].manaCost)  // ���� ����(��ٿ�) + ���� �Ҹ��� ���� �̻��̸� ��ų ����
            {
        /*        Debug.Log("��ų��� 1");
                skillOn[selectedSkillIndex] = true;    // �ݺ� ��ų ��� ����
                anim.SetTrigger(AnimationStrings.SkillTrigger2);    // ��ų ��� ĳ���� �ִϸ��̼� ����
                pm.SubCurMana(skillList[selectedSkillIndex].manaCost); // ���� �Ҹ𷮿� ���� ����
                StartCoroutine(JoroSkill2());  // ��ų ������ ����*/
            }
            else
            {
                Debug.Log("��ų��� 1 ����");
            }
        }
    }

}
