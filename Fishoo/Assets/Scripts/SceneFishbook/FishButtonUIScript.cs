using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FishButtonUIScript : MonoBehaviour
{
    public Image icon;
    public Sprite notFound;
    public int verticalIndex = 0;
    Button button;
    public Button GetButton {
        get { 
            if(button==null) button = GetComponent<Button>();
            return button;
        } 
    }
    public bool Interactable
    {
        set
        {
            GetButton.interactable = value;
        }
    }

    public void SetOnClicked(UnityAction func)
    {
        GetButton.onClick.AddListener(func);
    }

    public void SetFound(bool isFound)
    {
        if (!isFound)
            icon.sprite = notFound;
    }
    
}
