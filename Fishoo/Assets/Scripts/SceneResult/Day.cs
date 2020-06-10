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
        seasontext.color = SaveManager.Instance.GetSeasonColor();
        seasontext.text = SaveManager.Instance.GetSeasonKanji();
        weekText.text = $"<size=30>{SaveManager.Instance.Year}</size>年目";
    }

    
}
