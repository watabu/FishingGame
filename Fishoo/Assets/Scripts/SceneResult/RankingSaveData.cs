using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/RankingSaveData", order = 1)]
/// <summary>
/// 各ステージのランキングを保存する
/// </summary>
public class RankingSaveData : ScriptableObject
{
    [SerializeField]
    public string PlaceName;

    [System.Serializable]
    public struct Record
    {
        public Sprite icon;
        public string Name;
        public int FishCount;
        public int Score;
    };
    

    [SerializeField,Tooltip("Nameを「消去する」にするとそのレコードを削除")] public List<Record> Ranking = new List<Record>();
    [SerializeField] public int MaxRanking;

    private void OnEnable()
    {
        Ranking.Sort(delegate (Record x, Record y)
        {
            return  y.Score - x.Score ;
        });
        for(int i = 0; i < Ranking.Count; i++)
        {
            if (Ranking[i].Name == "消去する")
            {
                Ranking.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// ランキングに挿入する。ランキングの順位を返す。ランキング外であれば-1を返す。
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public int InsertRecord(Record record)
    {

        for(int i = 0; i < Ranking.Count; i++)
        {
            if( record.Score > Ranking[i].Score )
            {
                Ranking.Insert(i, record);
                if (Ranking.Count > MaxRanking)
                    Ranking.RemoveAt(MaxRanking);
                return i;
            }
        }
        if (Ranking.Count == MaxRanking)
            return -1;
        Ranking.Add(record);
        return Ranking.Count;
    }
    
}
