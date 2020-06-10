﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField]private TutorialUI tutorialUI;
    [SerializeField] private bool canSkipTutorial=false;
    [SerializeField] private SaveData data;
    [SerializeField] private TextMeshProUGUI seasonText;
    [SerializeField] private TextMeshProUGUI weekText;
    List<IInputUpdatable> m_inputObjects = new List<IInputUpdatable>();

    [SerializeField] public Environment.PlaceData currentPlace;

    UnityAction OnGameStarted;
    GameStartEffect gameStartEffect;
    bool m_IsPauseActive = false;
    public enum State
    {
        Nove,
        Ready,
        Tutorial,
        Playing,
        Finished
    }
    State m_state = State.Nove;
    public SaveData saveData { get { return data; } }

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
        if (tutorialUI != null)
            tutorialUI.gameObject.SetActive(false);
        m_IsPauseActive = false;
        m_state = State.Ready;
        seasonText.text = data.GetSeasonKanji();
        seasonText.color = data.GetSeasonColor();
        weekText.text = $"{data.Year}年目";
        gameStartEffect = Instantiate(gameStartEffectPrefab, gameStartEffectParent).GetComponent<GameStartEffect>();
        this.Delay(3f, () =>
        {
            gameStartBell.Play();
            gameStartEffect.FadeOut();
            if (canSkipTutorial)
                SwitchState(State.Playing);
            else
                SwitchState(State.Tutorial);
        });
        Environment.TimeHolder.Instance.AddOnTimeFinished((time)=> { SwitchState(State.Finished); });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(KeyCode.Escape.ToString()))

//            if (Player.InputSystem.GetKeyDown(KeyCode.Escape))
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


    //リザルト画面に切り替えたとき呼ばれる
    private void ResultSceneLoaded(Scene next, LoadSceneMode mode)
    {
        var coolerBox= FindObjectOfType<Player.CoolerBox>();
        var resultManager = FindObjectOfType<ResultManager>();
        if (resultManager == null)
            return;
        //ランキングや背景を渡す
        Instantiate(currentPlace.backGroundPrefab, resultManager.transform);
        resultManager.SelectRankingData(currentPlace.rankingSaveData);

        //魚を渡す
        resultManager.SetList(coolerBox.GetFishList);

        //ダーティとしてマークする(変更があった事を記録する)
        EditorUtility.SetDirty(data);
        EditorUtility.SetDirty(FindObjectOfType<RankingPanel>().RankingData);
        //保存する
        AssetDatabase.SaveAssets();
        SceneManager.MoveGameObjectToScene(coolerBox.gameObject, SceneManager.GetActiveScene());
        SceneManager.sceneLoaded -= ResultSceneLoaded;
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

    public void SwitchState(State state)
    {
        m_state = state;
        switch (m_state)
        {
            case State.Nove:
                break;
            case State.Ready:
                break;
            case State.Tutorial:
                if (tutorialUI == null)
                {
                    SwitchState(State.Playing);
                    break;
                }
                tutorialUI.gameObject.SetActive(true);
                tutorialUI.Initialize();
                break;
            case State.Playing:
                if (tutorialUI == null)
                {
                    OnGameStarted?.Invoke();
                    break;
                }
                tutorialUI.gameObject.SetActive(false);
                OnGameStarted?.Invoke();
                break;
            case State.Finished:
                SceneManager.sceneLoaded += ResultSceneLoaded;
                Debug.Log("aa");
                SceneManager.LoadScene("Result");
                break;
            default:
                break;
        }
    }

    public void BackToTitle()
    {
        SceneManager.sceneLoaded += ResultSceneLoaded;
        SceneManager.LoadScene("StageSelect");
    }
}
