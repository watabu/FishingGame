using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームのマネージャー
/// </summary>
public class GameMgr : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishingGame.FishingGameMgr fishingGameMgr;
    [SerializeField] private Player.InputSystem inputSystem;
    [SerializeField] private GameObject pauseUI;

    bool m_IsPauseActive = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
        m_IsPauseActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_IsPauseActive)
                ClosePauseUI();
            else
                OpenPauseUI();
        }
    }

    public void OpenPauseUI()
    {
        pauseUI.SetActive(true);
        m_IsPauseActive = true;
    }
    public void ClosePauseUI()
    {
        pauseUI.SetActive(false);
        m_IsPauseActive = false;
    }

}
