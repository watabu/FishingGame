using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/RankingSaveData", order = 1)]
/// <summary>
/// 各ステージのランキングを保存する
/// </summary>
public class RankingSaveData : ScriptableObject
{

    [System.Serializable]
    public struct Record
    {
        public Sprite icon;
        public string name;
        public int fishCount;
        public int score;
    };
    

    [SerializeField,Tooltip("ランキングはスコアで自動でソートされる")] public List<Record> ranking = new List<Record>();
    [SerializeField] public int maxRanking;

    private void OnEnable()
    {
        ranking.Sort(delegate (Record x, Record y)
        {
            return  y.score - x.score ;
        });

    }

    /// <summary>
    /// ランキングに挿入する。ランキングの順位を返す。ランキング外であれば-1を返す。
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public int InsertRecord(Record record)
    {

        for(int i = 0; i < ranking.Count; i++)
        {
            if( record.score > ranking[i].score )
            {
                ranking.Insert(i, record);
                if (ranking.Count > maxRanking)
                    ranking.RemoveAt(maxRanking);
                return i;
            }
        }
        if (ranking.Count == maxRanking)
            return -1;
        ranking.Add(record);
        return ranking.Count;
    }
    
}
