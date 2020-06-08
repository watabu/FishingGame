using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
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
    [Header("References")]
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private CanvasGroup titleCanvas;
    [SerializeField] private CanvasGroup stageCanvas;
    [SerializeField] private CanvasGroup optionCanvas;
    [SerializeField] private TextMeshProUGUI daySeason;
    [SerializeField] private TextMeshProUGUI dayWeek;
    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private SaveData data;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject popUpUI;
    [Header("Parameter")]
    [Tooltip("Title画面から遷移するときに何秒経てば遷移できるか")]
    [SerializeField] private float activateInput = 3f;
    static bool NoTitle = false;

    public enum State
    {
        none,
        title,
        stageSelect,
        selected
    }
    State m_state = State.none;
    float m_time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (!NoTitle)
            m_state = State.title;
        else
            m_state = State.stageSelect;
        SwitchState(m_state, true);

        NoTitle = true;
        daySeason.text = data.GetSeasonKanji();
        dayWeek.text = $"第<size=30>{data.week}</size>週";
        foreach (var i in scenesData)
        {
            i.button.Initialize(i.place.description, i.sceneName, () => { SceneManager.LoadScene(i.sceneName); });
        }
        money.text = $"現在の所持金：{data.money}";
        m_time = 0f;
        tutorialUI.SetActive(false);
        popUpUI.SetActive(false);
        optionCanvas.interactable = false;
        optionCanvas.alpha = 0f;
        optionCanvas.gameObject.SetActive(false);
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
                    SwitchState(State.stageSelect);
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
                titleCanvas.DOFade(0f, 0.5f);
                stageCanvas.DOFade(1f, 0.5f).OnComplete(() =>
                {
                    titleCanvas.gameObject.SetActive(false);
                    titleCanvas.interactable = false;
                    stageCanvas.interactable = true;
                });
                break;
            case State.selected:
                break;
            default:
                break;
        }
    }

    public void ActivateTutorial()
    {
        tutorialUI.SetActive(true);
        tutorialUI.GetComponent<TutorialUI>().Initialize();
    }
    public void DeActivateTutorial()
    {
        tutorialUI.SetActive(false);
    }

    public void ActivateOption()
    {
        optionCanvas.gameObject.SetActive(true);
        optionCanvas.DOFade(1f, 0.5f).OnComplete(() =>
        {
            stageCanvas.interactable = false;
            optionCanvas.interactable = true;
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
    }

    public void ActivatePopUp()
    {
        popUpUI.SetActive(true);
    }
    public void DeActivatePopUp()
    {
        popUpUI.SetActive(false);
    }

}
