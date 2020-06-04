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
    public enum PanelState
    {
        result,
        ranking,
    }
    State m_state;
    PanelState panelState;
    bool isMessagePanelActive;
    [SerializeField] ResultPanel result;
    [SerializeField] Animator messagePanel;
    [SerializeField] RankingPanel ranking;
    [SerializeField] SaveData saveData;

    // Start is called before the first frame update
    void Awake()
    {
        isMessagePanelActive = false;
        messagePanel.gameObject.SetActive(false);
        result.saveData = saveData;
        ranking.saveData = saveData;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.InputSystem.GetKeyDown(KeyCode.B))
        {
            //メッセージパネルがあるならスクロール操作をしない
            isMessagePanelActive ^= true;
            messagePanel.gameObject.SetActive(isMessagePanelActive);
            if (panelState == PanelState.result)
            {
                result.canScroll = !isMessagePanelActive;
            }
            else if(panelState == PanelState.ranking)
            {
                ranking.canScroll = !isMessagePanelActive;
            }
        }


        if (Player.InputSystem.GetKeyDown(KeyCode.R))
            ChangePanel(PanelState.ranking);
        if (Player.InputSystem.GetKeyDown(KeyCode.L))
            ChangePanel(PanelState.result);

    }

    public void SetList(List<Fish.FishInfo> fishList)
    {
        result.UpdateResult(fishList);
    }
    public void ActivateMessage()
    {
        messagePanel.gameObject.SetActive(true);

    }

    void ChangePanel(PanelState state)
    {
        if (state == PanelState.result)
        {
            ranking.gameObject.SetActive(false);
            messagePanel.gameObject.SetActive(false);
            //リザルトのアニメーション
            result.gameObject.SetActive(true);
        }
        else if (state == PanelState.ranking)
        {
            result.gameObject.SetActive(false);
            messagePanel.gameObject.SetActive(false);
            //ランキングのアニメーション
            ranking.gameObject.SetActive(true);
        }
    }
    
}
