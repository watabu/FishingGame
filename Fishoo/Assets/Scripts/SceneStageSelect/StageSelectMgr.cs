using Environment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI daySeason;
    [SerializeField] private TextMeshProUGUI dayWeek;
    [SerializeField] private SaveData data;
    [Header("Parameter")]
    [Tooltip("Title画面から遷移するときに何秒経てば遷移できるか")]
    [SerializeField] private float activateInput = 3f;

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
        stageCanvas.interactable = false;
        daySeason.text = data.GetSeasonKanji();
        dayWeek.text = $"第<size=30>{data.week}</size>週";
        foreach (var i in scenesData)
        {
            i.button.Initialize(i.place.description, i.sceneName, ()=> { SceneManager.LoadScene(i.sceneName); });
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
                if (m_time >= activateInput && Input.anyKeyDown)
                {
                    m_state = StageSelectState.stageSelect;
                    titleCanvas.alpha = 0;
                    stageCanvas.alpha = 1;
                    stageCanvas.interactable = true;
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
