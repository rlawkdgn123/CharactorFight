using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static PlayerBase;
// 작성자 : 김장후
public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int curMana;
    [SerializeField] private int maxMana;
    [SerializeField] private Slider mpBar;                // 마나 게이지 UI
    [SerializeField] private PlayerBase player;        // 플레이어 오브젝트
    [SerializeField] private GameObject ui;
    PlayerBase playerBase;  // 플레이어 컨트롤러 스크립트
    private void Start()
    {
        PlayerInitialize(); // 컴포넌트 연결
    }

    private void PlayerInitialize()
    {
        player = gameObject.GetComponent<PlayerBase>();
        ui = player.GetPlayerUI();

        string barName = "";
        if (player.GetCurChoice(PlayerBase.PlayerChoice.P1)) { barName = "P1_MPBar"; }
        else if (player.GetCurChoice(PlayerBase.PlayerChoice.P2)) { barName = "P2_MPBar"; }

        Transform tr = ui.transform.Find(barName);
        if (tr != null)
        {
            mpBar = tr.gameObject.GetComponentInChildren<Slider>();
            if (mpBar == null) { print("슬라이더가 존재하지 않습니다."); }
        }
        else { print(barName + "을 찾을 수 없습니다."); }

    }

    private void Update()  // 플레이어의 마나량에 접근해 마나량에 따라 마나UI의 채우기값 조정
    {
        if (mpBar != null) // 만약 hpBar 이미지가 할당되었다면
            mpBar.value = ((float)curMana / (float)maxMana); // hpBar fillAmount값 변경

        if (curMana < 0) { curMana = 0; }
        else if (curMana > maxMana) { curMana = maxMana; }

    }
    public void MpCharge(int chargeMana)
    {
        Debug.Log("MP 충전, 현재 마나: " + curMana);
        curMana += chargeMana; // 플레이어 스크립트에 마나량 추가
    }
    public void SetCurMana(int mana) { curMana = mana; }
    public void AddCurMana(int mana) { curMana += mana; }
    public void SubCurMana(int mana) { curMana -= mana; }
    public int GetCurMana() { return curMana; }
    public int GetMaxMana() { return maxMana; }
}
