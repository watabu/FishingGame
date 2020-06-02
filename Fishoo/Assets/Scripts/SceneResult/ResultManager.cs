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

    public void SetList(List<Fish.FishInfo> fishList)
    {
        panel.UpdateResult(fishList);
    }
    public void ActivateMessage()
    {
        messagePanel.gameObject.SetActive(true);

    }

    
}
