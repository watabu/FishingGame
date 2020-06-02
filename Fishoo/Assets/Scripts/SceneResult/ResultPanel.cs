using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{

    [SerializeField] GameObject contentPrefab;
    [SerializeField] GameObject result;
    [SerializeField] Transform contentParent;
    [SerializeField] bool Debug;
    [SerializeField,Tooltip("Debug")]List<Fish.FishInfo> DebugfishList = new List<Fish.FishInfo>();

    // Start is called before the first frame update
    void Start()
    {
        if (Debug)
        {
            UpdateResult(DebugfishList);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateResult(List<Fish.FishInfo> fishList) 
    {
        int Money = 0;
        foreach(var fish in fishList) {
            var script = Instantiate(contentPrefab, contentParent.transform).GetComponent<ResultContent>();
            script.icon.sprite = fish.icon;
            //魚の値段によって枠の色を変えるとか?
            //script.Frame.sprite = ;
            script.Name.text = fish.FishName;
            script.Money.text = (fish.sellingPrice).ToString();
            script.gameObject.transform.SetParent(contentParent.transform);
            Money += fish.sellingPrice;
        }
        var totalResult = result.GetComponent<TotalResult>();
        totalResult.Money.text = Money.ToString();
        totalResult.count.text = fishList.Count.ToString();
        result.transform.SetAsLastSibling();
    }
}
