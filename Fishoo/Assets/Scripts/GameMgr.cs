using System.Collections;
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
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private bool canSkipTutorial=false;
    [SerializeField] private TextMeshProUGUI seasonText;
    [SerializeField] private TextMeshProUGUI weekText;
    List<IInputUpdatable> m_inputObjects = new List<IInputUpdatable>();

    [SerializeField] public Environment.PlaceData currentPlace;
    [SerializeField] AudioSource pauseUIActivate;
    [SerializeField] AudioSource BGM;
    UnityAction OnGameStarted;
    bool m_IsPauseActive = false;
    bool m_CanMove = true;


    public enum State
    {
        Nove,
        Ready,
        Tutorial,
        Playing,
        Finished
    }
    State m_state = State.Nove;

    // Start is called before the first frame update
    void Start()
    {
       BGM.clip= SaveManager.Instance.GetSeasonBGM();
            BGM.Play();
        pauseUI.SetActive(false);
        if (tutorialUI != null)
            tutorialUI.gameObject.SetActive(false);
        m_IsPauseActive = false;
        m_state = State.Ready;
        seasonText.text = SaveManager.Instance.GetSeasonKanji();
        seasonText.color = SaveManager.Instance.GetSeasonColor();
        weekText.text = $"{ SaveManager.Instance.Year}年目";

        m_CanMove = false;
        CountDownGenerator.Instance.CountStart(2f,1f, () =>
        {
            m_CanMove = true;
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
        if (m_CanMove&&!m_IsPauseActive)
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
        {
            Debug.Log("null!!!");
            return;

        }
        //ランキングや背景を渡す
        var obj = Instantiate(currentPlace.backGroundPrefab, resultManager.transform);
        resultManager.SelectRankingData(currentPlace.rankingSaveData);

        //魚を渡す
        resultManager.SetList(coolerBox.GetFishList);

        SceneManager.MoveGameObjectToScene(coolerBox.gameObject, SceneManager.GetActiveScene());
        SceneManager.sceneLoaded -= ResultSceneLoaded;
    }


    public void OpenPauseUI()
    {
        pauseUI.SetActive(true);
        m_IsPauseActive = true;
        pauseUIActivate.Play();
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
