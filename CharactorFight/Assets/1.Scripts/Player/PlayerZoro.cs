using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerZoro : PlayerBase
{

    void Awake()
    {
        PlayerInitialize();
        dashEndTime = 0.083f*1.5f;
    }
    private void Update()
    {
        DefaultUpadateSetting();
    }
    private void FixedUpdate()
    {
        DefaultFixedUpadateSetting();
    }

}

    
    
