using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] GameObject contentPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] public Scrollbar scrollbar;
    [SerializeField] bool debug;

    public bool canScroll;
    public SaveData saveData;
    [Tooltip("各ステージのランキングデータ(未実装)")]
    public RankingSaveData RankingData;
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

    public void UpdateRanking(RankingSaveData.Record record)
    {
        int rank = RankingData.InsertRecord(record);

        DrawRanking();
    }

    public void DrawRanking()
    {
        foreach (var rec in RankingData.Ranking)
        {
            Debug.Log(rec);
            var script = Instantiate(contentPrefab, contentParent.transform).GetComponent<RankingContent>();
            script.icon.sprite = rec.icon;
            //魚の値段によって枠の色を変えるとか?
            //script.Frame.sprite = ;
            script.Name.text = rec.Name;
            script.count.text = rec.FishCount.ToString();
            script.score.text = rec.Score.ToString();
            script.gameObject.transform.SetParent(contentParent.transform);
        }
        //縦の長さをそろえる
        var rectTransform = contentParent.GetComponent<RectTransform>().sizeDelta;
        rectTransform.y = 80.3f * (RankingData.Ranking.Count);
        contentParent.GetComponent<RectTransform>().sizeDelta = rectTransform;
    }

}