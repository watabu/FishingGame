using Fish.Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookScritpUIMgr : MonoBehaviour
{
    public enum BookScriptState
    {
        list,
        description
    }

    
    [SerializeField] GameObject fishPrefab;
    [SerializeField] FishData[] data;
    [Header("References")]
    [SerializeField] GameObject bookList;
    [SerializeField] GameObject bookListContent;
    [SerializeField] GameObject bookDescription;
    [SerializeField] Button descriptionBackButton;
    BookDescriptionUI descriptionUI;

    private void Awake()
    {
        descriptionUI = bookDescription.GetComponent<BookDescriptionUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var d in data)
        {
          var script=  Instantiate(fishPrefab, bookListContent.transform).GetComponent<FishButtonUIScript>();
            script.SetOnClicked(()=> { 
                Switch(BookScriptState.description);
                descriptionUI.Set(d.icon,d.FishName,d.description);
            });
            script.icon.sprite = d.icon;
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
                break;
            case BookScriptState.description:
                bookList.SetActive(false);
                bookDescription.SetActive(true);
                break;
            default:
                break;
        }
    }
}
