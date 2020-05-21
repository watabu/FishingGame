using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject description;
    [SerializeField] Button FirstSelectedButton;
    int m_pageCount = 0;
    int m_currentPage = 0;

    public void Initialize()
    {
        m_pageCount = description.transform.childCount;
        description.transform.GetChild(m_currentPage).gameObject.SetActive(true);
    }
    public void ToPrev()
    {
        description.transform.GetChild(m_currentPage).gameObject.SetActive(false);
        m_currentPage = (m_currentPage - 1) % m_pageCount;
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
        GameMgr.Instance.SwitchState(GameMgr.GameState.Playing);
    }

}
