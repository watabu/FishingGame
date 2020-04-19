using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class StageSelectButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Button button;

    public void Initialize(string description_,UnityAction OnButtonClicked=null)
    {
        description.text = description_;
        button.onClick.AddListener(OnButtonClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
