using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Day : MonoBehaviour
{
    public TextMeshProUGUI weekText;
    public TextMeshProUGUI seasontext;
    // Start is called before the first frame update
    void Start()
    {
        var data = FindObjectOfType<ResultPanel>().saveData;

        seasontext.color = data.GetSeasonColor();
        seasontext.text = data.GetSeasonKanji();
        weekText.text = $"<size=30>{data.Year}</size>年目";
    }

    
}
