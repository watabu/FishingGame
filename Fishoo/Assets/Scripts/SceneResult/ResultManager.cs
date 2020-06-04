using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{

    public enum State
    {
        none,
        result,
        askEnd,
        finish
    }
    State m_state;
    [SerializeField] ResultPanel panel;
    [SerializeField] Animator messagePanel;
    [SerializeField] RankingPanel ranking;

    // Start is called before the first frame update
    void Awake()
    {
        messagePanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.InputSystem.GetKeyDown(KeyCode.B))
        {
            bool isActive = messagePanel.gameObject.activeSelf;
            messagePanel.gameObject.SetActive(isActive ^true);
            ranking.gameObject.SetActive(isActive ^ true);
        }
    }

    public void SetList(List<Fish.FishInfo> fishList)
    {
        panel.UpdateResult(fishList);
    }
    public void ActivateMessage()
    {
        messagePanel.gameObject.SetActive(true);

    }

    
}
