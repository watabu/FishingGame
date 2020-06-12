using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class StageSelectButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI buttonName;
    [SerializeField] Button button;


    public void Initialize(string description_,string buttonName_,UnityAction OnButtonClicked=null)
    {
        description.text = description_;
        
        buttonName.text = buttonName_;
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
