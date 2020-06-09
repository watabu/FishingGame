using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultPanel : MonoBehaviour
{

    [SerializeField] GameObject contentPrefab;
    [SerializeField] GameObject result;
    [SerializeField] GameObject totalScore;
    [SerializeField] Transform contentParent;
    [SerializeField]Scrollbar scrollbar;
    public Scrollbar GetSrollbar { get { return scrollbar; } }
    [SerializeField, Tooltip("一匹も釣らなかった時にもらえる魚")]
    Fish.FishInfo GivenFish;
    [SerializeField]public bool debug;
    [SerializeField,Tooltip("Debug")]List<Fish.FishInfo> DebugfishList = new List<Fish.FishInfo>();

    public bool canScroll;
    public SaveData saveData;
    // Start is called before the first frame update
    void Start()
    {
        if (debug)
        {
            UpdateResult(DebugfishList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll &&( Player.InputSystem.GetKeyDown(KeyCode.UpArrow) || Player.InputSystem.GetKeyDown(KeyCode.DownArrow)))
        {
            scrollbar.Select();
        }

    }

    /// <summary>
    /// リザルト画面とセーブデータの更新
    /// </summary>
    /// <param name="fishList"></param>
    public void UpdateResult(List<Fish.FishInfo> fishList) 
    {
        Debug.Log("updateResult");
        int Money = 0;
        foreach(var fish in fishList) {
            if (fish == null) continue;
            var script = Instantiate(contentPrefab, contentParent.transform).GetComponent<ResultContent>();

            script.icon.sprite = fish.icon;
            //魚の値段によって枠の色を変えるとか?
            //script.Frame.sprite = ;
            script.Name.text = fish.FishName;
            script.Money.text = (fish.sellingPrice).ToString();
            script.gameObject.transform.SetParent(contentParent.transform);
            Money += fish.sellingPrice;
            saveData.AddFish(fish);
        }
        //一匹も釣らなかった時にもらえる魚
        if (fishList.Count < 1)
        {
            var fish = GivenFish;
            var script = Instantiate(contentPrefab, contentParent.transform).GetComponent<ResultContent>();
            script.icon.sprite = fish.icon;
            //魚の値段によって枠の色を変えるとか?
            //script.Frame.sprite = ;
            script.Name.text = fish.FishName;
            script.Money.text = (fish.sellingPrice).ToString();
            script.gameObject.transform.SetParent(contentParent.transform);
            Money += fish.sellingPrice;
            saveData.AddFish(fish);
        }

        //一日のまとめ                
        var summary= result.GetComponent<TodaysSummary>();
        summary.Money.text = Money.ToString();
        summary.count.text = fishList.Count.ToString();
        //セーブデータから累計の記録を受け取る
        saveData.money += Money;
        totalScore.GetComponent<TotalScore>().Money.text = saveData.money.ToString();
        totalScore.GetComponent<TotalScore>().count.text = saveData.caughtFishCount.ToString();
        //一番場所を下に
        result.transform.SetAsLastSibling();
        totalScore.transform.SetAsLastSibling();
        //縦の長さをそろえる
        var rectTransform = contentParent.GetComponent<RectTransform>().sizeDelta;
        rectTransform.y = 79.8f * (fishList.Count+2);
        contentParent.GetComponent<RectTransform>().sizeDelta = rectTransform;
        
    }
}
