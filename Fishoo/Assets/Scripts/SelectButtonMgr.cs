﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// シーンに入って最初に指定されるボタンやBボタンを押したときに指定されたボタンへ遷移させる
/// </summary>
public class SelectButtonMgr : MonoBehaviour
{

    [Tooltip("Bボタンを押したときに指定されるボタン"), SerializeField]
    Button backButton;
    public Button BackButton { get { return backButton; } }

    public Button temporaryBackButton;

    [Tooltip("シーンで最初に指定されるボタン"), SerializeField]
    Button firstButton;
    public Button FirstButton { get { return firstButton; } }
    // Use this for initialization
    void Start()
    {
        if (firstButton != null)
            firstButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        //ボタンが操作不可能だったり存在しなければ
        if (backButton ==null || !backButton.IsActive())
            return;
        

        if (Player.InputSystem.GetKeyDown(KeyCode.B))
        {
            if (temporaryBackButton != null)
                temporaryBackButton.Select();
            else
                backButton.Select();
        }
    }

    public void LoadStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
        
    }
    public void LoadStage(string Stage)
    {
        SceneManager.LoadScene(Stage);

    }

    public void DebugPrint()
    {
        Debug.Log("aaaaaaaaaaa");
    }
}