using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
/// <summary>
/// ボタンを押すと特定のシーンに移動できるようにするクラス
/// </summary>
public class StageSelectMgr : MonoBehaviour
{
    [System.Serializable]
    public class PlaceAndData
    {
        public string sceneName;
        public PlaceData place;
        public StageSelectButton button;
    }

    [SerializeField] private PlaceAndData[] scenesData;
    [Header("Sprites")]
    [SerializeField] private Sprite backSpring;
    [SerializeField] private Sprite backSummer;
    [SerializeField] private Sprite backAutumn;
    [SerializeField] private Sprite backWinter;
    [SerializeField] private SpriteRenderer back;

    [Header("References")]
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private CanvasGroup titleCanvas;
    [SerializeField] private CanvasGroup stageCanvas;
    [SerializeField] private CanvasGroup optionCanvas;
    [SerializeField] private Selectable firstSelectButton;
    [SerializeField] private GameObject popUp2UI;
    [SerializeField] private Selectable popUp2SelectButton;
    [SerializeField] private TextMeshProUGUI daySeason;
    [SerializeField] private TextMeshProUGUI dayWeek;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private Selectable tutorialSelectButton;
    [SerializeField] private GameObject popUpUI;
    [SerializeField] private Selectable popUpSelectButton;
    [SerializeField] private GameObject popUpUI_quitApp;
    [Header("Parameter")]
    [Tooltip("Title画面から遷移するときに何秒経てば遷移できるか")]
    [SerializeField] private float activateInput = 3f;
    static bool NoTitle = false;

    public enum State
    {
        none,
        title,
        stageSelect,
        selected,
        tutorial
    }
    State m_state = State.none;
    float m_time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        firstSelectButton.Select();
        if (!NoTitle)
            m_state = State.title;
        else
            m_state = State.stageSelect;
        SwitchState(m_state, true);

        NoTitle = true;
        daySeason.text = SaveManager.Instance.GetSeasonKanji();
        daySeason.color = SaveManager.Instance.GetSeasonColor();
//        Debug.Log(SaveManager.Instance.GetSeasonColor());
        dayWeek.text = $"<size=30>{SaveManager.Instance.Year}</size>年目";
        foreach (var i in scenesData)
        {
            string description = i.place.description;
            description += "\n";
            if (i.place.AchievementInfo().Item2 != 0)
            {
                description += "達成率:";
                var item = i.place.AchievementInfo();
                description += item.Item1 + "/" + item.Item2;

            }
            i.button.Initialize(description, i.place.placeName, () => { SceneManager.LoadScene(i.sceneName); });
            
        }
        money.text = $"現在の所持金：{SaveManager.Instance.money}";
        m_time = 0f;
        tutorialUI.SetActive(false);
        popUpUI.SetActive(false);
        optionCanvas.interactable = false;
        optionCanvas.alpha = 0f;
        optionCanvas.gameObject.SetActive(false);
        popUp2UI.gameObject.SetActive(false);
        SetMapFromSeason();
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_state)
        {
            case State.none:
                break;
            case State.title:
                if (Input.anyKeyDown)
                {
                    if (SaveManager.Instance.canSkipTutorial)
                        SwitchState(State.stageSelect);
                    else
                        SwitchState(State.tutorial);

                }
                break;
            case State.stageSelect:
                break;
            case State.selected:
                break;
            default:
                break;
        }
        m_time += Time.deltaTime;
    }

    public void Jump(int ID)
    {
        SceneManager.LoadScene(scenesData[ID].sceneName);
    }

    public void SwitchState(State state, bool ignore = false)
    {
        if (!ignore && m_time < activateInput) return;
        var previousState = m_state;
        m_state = state;
        switch (m_state)
        {
            case State.none:
                break;
            case State.title:
                titleCanvas.gameObject.SetActive(true);
                titleCanvas.DOFade(1f, 0.5f);
                stageCanvas.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    stageCanvas.gameObject.SetActive(false);
                    stageCanvas.interactable = false;
                    titleCanvas.interactable = true;
                });
                break;
            case State.stageSelect:
                stageCanvas.gameObject.SetActive(true);
                SelectButtonMgr.Instance.BackButton.Select();
                Debug.Log(previousState);
                if (previousState == State.title)
                {
                    titleCanvas.DOFade(0f, 0.5f);
                    stageCanvas.DOFade(1f, 0.5f).OnComplete(() =>
                    {
                        titleCanvas.gameObject.SetActive(false);
                        titleCanvas.interactable = false;
                        stageCanvas.interactable = true;
                    });

                }
                else if (previousState == State.tutorial)
                {
                    tutorialUI.GetComponent<CanvasGroup>().DOFade(0f, 0.5f);
                    stageCanvas.DOFade(1f, 0.5f).OnComplete(() =>
                    {
                        tutorialUI.SetActive(false);
                        stageCanvas.interactable = true;
                    });
                }


                break;
            case State.selected:
                break;
            case State.tutorial:
                tutorialUI.SetActive(true);
                if (previousState == State.title)
                {
                    titleCanvas.DOFade(0f, 0.5f);
                }else if(previousState == State.stageSelect)
                {
                    stageCanvas.DOFade(0f, 0.5f);
                }
                tutorialUI.GetComponent<CanvasGroup>().alpha = 0;
                tutorialUI.GetComponent<TutorialUI>().Initialize();

                tutorialUI.GetComponent<CanvasGroup>().DOFade(1f, 0.8f).OnComplete(() =>
                {
                    SaveManager.Instance.canSkipTutorial = true;

                });

                break;
            default:
                break;
        }
    }
    /// <summary>
    /// オプションからチュートリアルへ
    /// </summary>
    public void ActivateTutorial()
    {
        popUpBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        Debug.Log(popUpBefore.gameObject);
        tutorialUI.SetActive(true);
        tutorialUI.GetComponent<TutorialUI>().Initialize();
        tutorialUI.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
        SaveManager.Instance.canSkipTutorial = true;
    }
    /// <summary>
    /// チュートリアルからオプションに戻る
    /// </summary>
    public void DeActivateTutorial()
    {
        if (m_state == State.tutorial)
        {
            SwitchState(State.stageSelect);
            return;
        }
        tutorialUI.SetActive(false);
        popUpBefore.Select();
    }



    Selectable optionBefore;
    public void ActivateOption()
    {
        SelectButtonMgr.Instance.isStop = true;
        optionBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        optionCanvas.gameObject.SetActive(true);
        optionCanvas.DOFade(1f, 0.5f).OnComplete(() =>
        {
            stageCanvas.interactable = false;
            optionCanvas.interactable = true;
            tutorialSelectButton.Select();
        });
    }
    public void DeActivateOption()
    {
        optionCanvas.interactable = false;
        optionCanvas.DOFade(1f, 0.5f).OnComplete(() =>
        {
            stageCanvas.interactable = true;
            optionCanvas.gameObject.SetActive(false);
        });
        optionBefore.Select();
        SelectButtonMgr.Instance.isStop = false;
    }

    Selectable popUpBefore;

    public void ActivatePopUp()
    {
        popUpBefore = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        popUpUI.SetActive(true);
        popUpSelectButton.Select();
    }

    public void DeActivatePopUp()
    {
        popUpUI.SetActive(false);
        popUpBefore.Select();
    }
    public void ResetSaveData()
    {
        popUp2UI.gameObject.SetActive(true);
        popUp2SelectButton.Select();
        FindObjectOfType<SoundOnDestroy>().PlaySound();
        NoTitle = false;
        SaveManager.Instance.DeleteAll();
    }
    public void CloseResetSaveData()
    {
        popUp2UI.gameObject.SetActive(false);
        optionCanvas.gameObject.SetActive(false);
        DeActivateOption();
    }
    void SetMapFromSeason()
    {
        switch (SaveManager.Instance.GetSeason())
        {
            case SaveManager.Season.spring:
                back.sprite = backSpring;
                break;
            case SaveManager.Season.summer:
                back.sprite = backSummer;
                break;
            case SaveManager.Season.autumn:
                back.sprite = backAutumn;
                break;
            case SaveManager.Season.winter:
                back.sprite = backWinter;
                break;
            default:
                break;
        }
    }

    public void DebugSeason()
    {
        SaveManager.Instance.AddWeek();
        daySeason.text = SaveManager.Instance.GetSeasonKanji();
        daySeason.color = SaveManager.Instance.GetSeasonColor();
        dayWeek.text = $"<size=30>{ SaveManager.Instance.Year}</size>年目";
    }

    static public void SetTitleActive()
    {
        NoTitle = false;
    }

public void QuitApp()
    {
        SaveManager.Instance.Save();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
