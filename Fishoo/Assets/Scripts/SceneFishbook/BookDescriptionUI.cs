using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookDescriptionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] new TextMeshProUGUI name;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Sprite icon,string name_,string description_)
    {
        image.sprite = icon;
        description.text = description_;
        name.text = name_;
    }
}
