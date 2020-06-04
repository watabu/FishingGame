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
        list,
        description
    }

    
    [SerializeField] GameObject fishPrefab;
    [Header("References")]
    [SerializeField] GameObject bookList;
    [SerializeField] GameObject bookListContent;
    [SerializeField] GameObject bookDescription;
    BookDescriptionUI descriptionUI;
    [SerializeField] Button SelectedButton;
    [SerializeField] FishInfoFolder FishFolder;
    [SerializeField] SaveData data;
    [SerializeField] Button backButton;
    [SerializeField] bool showNonCaughtFish = false;

    State m_currentState;
    float m_changeTime=0f;//切り替わってから何秒経ったか


    private void Awake()
    {
        descriptionUI = bookDescription.GetComponent<BookDescriptionUI>();
    }
    void Start()
    {
        FishFolder.InitiateList();
        FishButtonUIScript lastObj = null;//最後に生成されたボタン(最初に生成されたボタンは右下に追いやられるようなので？要検討)
        foreach (var d in FishFolder.AvailableFishes)
        {
            if (d == null) continue;
            var script = Instantiate(fishPrefab, bookListContent.transform).GetComponent<FishButtonUIScript>();
            //捕まえたなら詳しい情報を見れる
            if ( data.isCaught(d))
            {
                script.SetOnClicked(() =>
                {
                    Switch(State.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description, data.fishes[d].count);
                    SelectedButton = script.GetButton;
                });

            }else if (showNonCaughtFish)
            {
                script.SetOnClicked(() =>
                {
                    Switch(State.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description,0);
                    SelectedButton = script.GetButton;
                });
            }
            script.Interactable = showNonCaughtFish||data.isCaught(d);
            script.icon.sprite = d.icon;
            lastObj = script;
        }

        foreach (var d in FishFolder.AvailableGomi)
        {
            if (d == null) continue;
            var script = Instantiate(fishPrefab, bookListContent.transform).GetComponent<FishButtonUIScript>();
            //捕まえたなら詳しい情報を見れる
            if (showNonCaughtFish || data.isCaught(d))
            {
                script.SetOnClicked(() =>
                {
                    Switch(State.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description, data.fishes[d].count);
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
            script.Interactable = showNonCaughtFish||data.isCaught(d);
            script.icon.sprite = d.icon;
            lastObj = script;
        }
        //図鑑に戻ったときの初期選択
        if (SelectedButton == null) SelectedButton = lastObj.GetButton;
        Switch(State.list);
        backButton.onClick.AddListener(()=> { SceneManager.LoadScene("StageSelect"); });
    }


    private void Update()
    {
        if(m_currentState == State.description)
        {
            if (Input.anyKeyDown&& m_changeTime>=0.5f)//切り替わってから0.5秒経たないとシーン切り替えができないように
            {
                Switch(State.list);
            }
        }
        m_changeTime += Time.deltaTime;
    }
    public void Switch(State state)
    {
        m_currentState = state;
        m_changeTime = 0f;
        switch (state)
        {
            case State.list:
                bookList.SetActive(true);
                bookDescription.SetActive(false);
                backButton.interactable = true;
                SelectedButton.Select();
                break;
            case State.description:
                bookList.SetActive(false);
                bookDescription.SetActive(true);
                backButton.interactable = false;
                break;
            default:
                break;
        }
    }

}
