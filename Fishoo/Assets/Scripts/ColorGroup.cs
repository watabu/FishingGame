using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Buttonの子にあるオブジェクトにも色の変化を適用させる
/// </summary>
[RequireComponent(typeof(Button))]
public class ColorGroup : MonoBehaviour
{
    Button button;
    [SerializeField] bool isMultiply=false;
    List<Graphic> targets;
    
    void Start()
    {
        button = GetComponent<Button>();
        targets = GetComponentsInChildren<Graphic>().Where(c => c.gameObject != gameObject).ToList();
    }

    void Update()
    {
        targets.ForEach(t => {
            if (isMultiply)
            {
                t.color = button.targetGraphic.canvasRenderer.GetColor();
            }
            else
            {
                t.color = button.targetGraphic.canvasRenderer.GetColor();
            }
        });
    }
}
