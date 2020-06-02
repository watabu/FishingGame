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
    [SerializeField, ReadOnly] List<Fish.Behavior.CommonFish> m_fishList;
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
            messagePanel.gameObject.SetActive(messagePanel.gameObject.activeSelf ^true);
        }
    }

    public void SetList(List<Fish.Behavior.CommonFish> fishList)
    {
        m_fishList = fishList;
        Debug.Log("合計:" + m_fishList.Count);
    }
    public void ActivateMessage()
    {
        messagePanel.gameObject.SetActive(true);

    }


}
