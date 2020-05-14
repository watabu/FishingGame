using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームのマネージャー
/// </summary>
public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    [Header("References")]
    [SerializeField] private Player.InputSystem inputSystem;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameStartEffectPrefab;
    [SerializeField] private Transform gameStartEffectParent;
    [SerializeField] AudioSource gameStartBell;

    List<IInputUpdatable> m_inputObjects=new List<IInputUpdatable>();

    UnityAction OnGameStarted;
    GameStartEffect gameStartEffect;
    bool m_IsPauseActive = false;
    public enum GameState
    {
        Nove,
        Ready,
        Playing,
        Finished
    }
    GameState m_state=GameState.Nove;

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
        m_IsPauseActive = false;
        m_state = GameState.Ready;
        gameStartEffect = Instantiate(gameStartEffectPrefab, gameStartEffectParent).GetComponent<GameStartEffect>();
        this.Delay(3f, () =>
        {
            gameStartBell.Play();
            gameStartEffect.FadeOut();
            SwitchState(GameState.Playing);
        });
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
        //ポーズ中はプレイヤーを動かしたりしない
        if (!m_IsPauseActive)
        {
            foreach (var i in m_inputObjects)
                i.InputUpdate();
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
    public void AddInputUpdatable(IInputUpdatable obj) { m_inputObjects.Add(obj); }
    public void RemoveInputUpdatable(IInputUpdatable obj) { m_inputObjects.Remove(obj); }

    public void AddOnGameStarted(UnityAction func) { OnGameStarted+=func; }

    public void SwitchState(GameState state)
    {
        m_state = state;
        switch (m_state)
        {
            case GameState.Nove:
                break;
            case GameState.Ready:
                break;
            case GameState.Playing:
                OnGameStarted?.Invoke();
                break;
            case GameState.Finished:
                break;
            default:
                break;
        }
    }
}
