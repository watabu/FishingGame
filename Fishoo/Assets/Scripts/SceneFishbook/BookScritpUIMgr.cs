using Fish;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookScritpUIMgr : MonoBehaviour
{
    public enum BookScriptState
    {
        list,
        description
    }

    
    [SerializeField] GameObject fishPrefab;
    [Header("References")]
    [SerializeField] GameObject bookList;
    [SerializeField] GameObject bookListContent;
    [SerializeField] GameObject bookDescription;
    [SerializeField] Button descriptionBackButton;
    BookDescriptionUI descriptionUI;
    [SerializeField] Button SelectedButton;
    [SerializeField] FishInfoFolder FishFolder;
    [SerializeField] SaveData data;
    private void Awake()
    {
        descriptionUI = bookDescription.GetComponent<BookDescriptionUI>();
    }
    // Start is called before the first frame update
    void Start()
    {

        foreach (var d in FishFolder.AvailableFishes)
        {
            if (d == null) continue;
          var script=  Instantiate(fishPrefab, bookListContent.transform).GetComponent<FishButtonUIScript>();
            //捕まえたなら詳しい情報を見れる
            if (data.isCaught(d))
            {
                script.SetOnClicked(() =>
                {
                    Switch(BookScriptState.description);
                    descriptionUI.Set(d.icon, d.FishName, d.description);
                });
            }
            script.icon.sprite = d.icon;
            //図鑑に戻ったときの初期選択
            if (SelectedButton == null)
                SelectedButton = script.GetComponent<FishButtonUIScript>().GetButton;
        }
        Switch(BookScriptState.list);
        descriptionBackButton.onClick.AddListener(()=> { Switch(BookScriptState.list); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch(BookScriptState state)
    {
        switch (state)
        {
            case BookScriptState.list:
                bookList.SetActive(true);
                bookDescription.SetActive(false);
                SelectedButton.Select();
                break;
            case BookScriptState.description:
                bookList.SetActive(false);
                bookDescription.SetActive(true);
                //戻るボタンを選択状態に
                descriptionBackButton.Select();
                break;
            default:
                break;
        }
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("StageSelect");

    }

}
