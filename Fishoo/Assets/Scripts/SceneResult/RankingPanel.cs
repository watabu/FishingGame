using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingPanel : MonoBehaviour
{
    [SerializeField] GameObject contentPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] public Scrollbar scrollbar;
    [SerializeField] bool Debug;

    public bool canScroll;
    public SaveData saveData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll && (Player.InputSystem.GetKeyDown(KeyCode.UpArrow) || Player.InputSystem.GetKeyDown(KeyCode.DownArrow)))
        {
            scrollbar.Select();
        }
    }
}
