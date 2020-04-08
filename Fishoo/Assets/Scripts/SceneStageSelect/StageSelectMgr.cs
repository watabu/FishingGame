using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ボタンを押すと特定のシーンに移動できるようにするクラス
/// </summary>
public class StageSelectMgr : MonoBehaviour
{

    [SerializeField] private string[] scenesName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Jump(int ID)
    {
        SceneManager.LoadScene(scenesName[ID]);
    }
}
