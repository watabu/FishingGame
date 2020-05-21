using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FishButtonUIScript : MonoBehaviour
{
    public Image icon;
    Button button;
    public Button GetButton { get { return button; } }
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.Select();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnClicked(UnityAction func)
    {
        button.onClick.AddListener(func);
    }
}
