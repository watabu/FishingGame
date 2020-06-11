using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// シーンに入って最初に指定されるボタンやBボタンを押したときに指定されたボタンへ遷移させる
/// </summary>
public class SelectButtonMgr : SingletonMonoBehaviour<SelectButtonMgr>
{

    [Tooltip("Bボタンを押したときに指定されるボタン"), SerializeField]
    Selectable backButton;
    public Selectable BackButton { get { return backButton; } }

    public Selectable temporaryBackButton;

    [Tooltip("シーンで最初に指定されるボタン"), SerializeField]
    Button firstButton;
    public Button FirstButton { get { return firstButton; } }

    public bool isStop;
    // Use this for initialization
    void Start()
    {
        isStop = false;
        if (firstButton != null)
            firstButton.Select();
    }

    // Update is called once per frame
    void Update()
    {

        if (isStop) return;
        if (Player.InputSystem.GetKeyDown(KeyCode.B))
        {
            if (temporaryBackButton != null && temporaryBackButton.IsActive())
                temporaryBackButton.Select();
            else if (backButton!= null && backButton.IsActive())
                backButton.Select();
        }
    }

    public void LoadStageSelect()
    {
        SceneManager.LoadScene("StageSelect");
        
    }
    public void LoadStage(string Stage)
    {
        SceneManager.LoadScene(Stage);

    }

    public void DebugPrint()
    {
        Debug.Log("aaaaaaaaaaa");
    }
}
