using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// ボタンを押すと特定のシーンに移動できるようにするクラス
/// </summary>
public class StageSelectMgr : MonoBehaviour
{
    [System.Serializable]
    public class PlaceAndData
    {
        public string sceneName;
        public PlaceData place;
        public StageSelectButton button;
    }

    [SerializeField] private PlaceAndData[] scenesData;
    [Header("References")]
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private CanvasGroup titleCanvas;
    [SerializeField] private CanvasGroup stageCanvas;

    public enum StageSelectState
    {
        none,
        title,
        stageSelect,
        selected
    }
    StageSelectState m_state = StageSelectState.none;
    float m_time=0f;
    // Start is called before the first frame update
    void Start()
    {
        m_state = StageSelectState.title;
        titleCanvas.alpha = 1;
        stageCanvas.alpha = 0;
        foreach (var i in scenesData)
        {
            i.button.Initialize(i.place.description,()=> { });
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case StageSelectState.none:
                break;
            case StageSelectState.title:
                if (m_time >= 3f&&Input.anyKeyDown)
                {
                    m_state = StageSelectState.stageSelect;
                    titleCanvas.alpha = 0;
                    stageCanvas.alpha = 1;
                }
                break;
            case StageSelectState.stageSelect:
                break;
            case StageSelectState.selected:
                break;
            default:
                break;
        }
        m_time += Time.deltaTime;
    }

    public void Jump(int ID)
    {
        SceneManager.LoadScene(scenesData[ID].sceneName);
    }
}
