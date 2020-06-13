using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject description;
    [SerializeField] Button FirstSelectedButton;
    [SerializeField] SelectButtonMgr buttonMgr;
    int m_pageCount = 0;
    int m_currentPage = 0;

    public void Initialize()
    {
        m_pageCount = description.transform.childCount;
        m_currentPage = 0;
        description.transform.GetChild(m_currentPage).gameObject.SetActive(true);
        buttonMgr.temporaryBackButton = FirstSelectedButton;
        FirstSelectedButton.Select();
    }
    public void ToPrev()
    {
        description.transform.GetChild(m_currentPage).gameObject.SetActive(false);
        m_currentPage = Mathf.Max((m_currentPage - 1),0) % m_pageCount;
        description.transform.GetChild(m_currentPage).gameObject.SetActive(true);
    }
    public void ToNext()
    {
        description.transform.GetChild(m_currentPage).gameObject.SetActive(false);
        m_currentPage = (m_currentPage + 1) % m_pageCount;
        description.transform.GetChild(m_currentPage).gameObject.SetActive(true);
    }
    public void ToPlayGame()
    {
        GameMgr.Instance.SwitchState(GameMgr.State.Playing);
    }

    public void CloseUI()
    {
        foreach(Transform panel in description.transform)
        {
            panel.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
        Selectable selectable = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        buttonMgr.temporaryBackButton = selectable;
        selectable.Select();
    }

}
