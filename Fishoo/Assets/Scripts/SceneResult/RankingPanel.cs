using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] GameObject contentPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] Scrollbar scrollbar;
    public Scrollbar GetSrollbar { get { return scrollbar; } }

    [SerializeField,Tooltip("釣ゲームから遷移しない場合にランキングを表示する")]public bool debug;

    public bool canScroll;
    [Tooltip("各ステージのランキングデータ")]
    public RankingSaveData rankingData;
    // Start is called before the first frame update
    void Start()
    {
        if (debug)
        {
            DrawRanking();
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll && (Player.InputSystem.GetKeyDown(KeyCode.UpArrow) || Player.InputSystem.GetKeyDown(KeyCode.DownArrow)))
        {
            scrollbar.Select();
        }
    }

    /// <summary>
    /// ゲームから遷移された際にResultPanelから呼ばれる。
    /// 更新と描写を兼ね備える
    /// </summary>
    /// <param name="record"></param>
    public void UpdateRanking(RankingSaveData.Record record)
    {
//        Debug.Log("updateRanking");
        int rank = rankingData.InsertRecord(record);

        DrawRanking();
    }

    public void DrawRanking()
    {
        foreach (var rec in rankingData.ranking)
        {
            var script = Instantiate(contentPrefab, contentParent.transform).GetComponent<RankingContent>();
            script.icon.sprite = rec.icon;
            //魚の値段によって枠の色を変えるとか?
            //script.Frame.sprite = ;
            script.fisherName.text = rec.name;
            script.count.text = rec.fishCount.ToString();
            script.score.text = rec.score.ToString();
            script.gameObject.transform.SetParent(contentParent.transform);
        }
        //縦の長さをそろえる
        var rectTransform = contentParent.GetComponent<RectTransform>().sizeDelta;
        rectTransform.y = 80.3f * (rankingData.ranking.Count);
        contentParent.GetComponent<RectTransform>().sizeDelta = rectTransform;
    }

}