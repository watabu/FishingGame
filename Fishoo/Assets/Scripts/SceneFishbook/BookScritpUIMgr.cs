using Fish;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookScritpUIMgr : MonoBehaviour
{
    public enum State
    {
        none,
        list,
        description
    }

    [SerializeField] GameObject fishPrefab;
    [SerializeField] Sprite fishNotFound;
    [Header("References")]
    [SerializeField] GameObject bookList;
    [SerializeField] GameObject bookListContent;
    [SerializeField] GameObject bookDescription;
    [SerializeField] Button SelectedButton;
    [SerializeField] FishInfoFolder FishFolder;
    [SerializeField] Button backButton;
    [Header("Debug")]
    [SerializeField] bool showNonCaughtFish = false;

    AutoScrollList autoScroll;
    BookDescriptionUI descriptionUI;

    State m_currentState = State.none;
    float m_changeTime = 0f;//切り替わってから何秒経ったか

    private void Awake()
    {
        descriptionUI = bookDescription.GetComponent<BookDescriptionUI>();
        autoScroll = GetComponent<AutoScrollList>();
        m_currentState = State.none;
    }

    void SetData(IEnumerable<FishInfo> list)
    {
        foreach (var d in list)
        {
            if (d == null) continue;
            var script = Instantiate(fishPrefab, bookListContent.transform).GetComponent<FishButtonUIScript>();
            //捕まえたなら詳しい情報を見れる
            if (SaveManager.Instance.isCaught(d))
            {
                script.SetOnClicked(() =>
                {
                    Switch(State.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description, SaveManager.Instance.GetFishCount(d));
                    SelectedButton = script.GetButton;
                });
            }
            else if (showNonCaughtFish)
            {
                script.SetOnClicked(() =>
                {
                    Switch(State.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description, 0);
                    SelectedButton = script.GetButton;
                });
            }
            script.Interactable = showNonCaughtFish || SaveManager.Instance.isCaught(d);
            script.icon.sprite = d.icon;
            script.SetFound(SaveManager.Instance.isCaught(d));
        }
    }

    void Start()
    {
        backButton.onClick.AddListener(() => { SceneManager.LoadScene("StageSelect"); });
        FishFolder.InitiateList();
        SetData(FishFolder.AvailableFishes);
        SetData(FishFolder.AvailableGomi);
        //図鑑に戻ったときの初期選択
        SelectedButton = bookListContent.transform.GetChild(0).GetComponent<Button>();
        Switch(State.list, true);
    }

    private void Update()
    {
        if (m_currentState == State.description)
        {
            if (Player.InputSystem.anyKeyDown)
            {
                Switch(State.list);
            }
        }
        m_changeTime += Time.deltaTime;
        autoScroll.Scroll();
    }

    public void Switch(State state, bool ignore = false)
    {
        if (!ignore && m_changeTime < 0.1f) return;//切り替わってから0.5秒経たないとシーン切り替えができないように
        if (m_currentState == state) return;
        //Debug.Log($"Switch {state}");
        m_currentState = state;
        m_changeTime = 0f;
        switch (state)
        {
            case State.list:
                bookList.SetActive(true);
                bookDescription.SetActive(false);
                backButton.interactable = true;
                Debug.Log("Select", SelectedButton);
                SelectedButton.Select();
                break;
            case State.description:
                //bookList.SetActive(false);
                bookDescription.SetActive(true);
                backButton.interactable = false;
                break;
            default:
                break;
        }
    }

}

