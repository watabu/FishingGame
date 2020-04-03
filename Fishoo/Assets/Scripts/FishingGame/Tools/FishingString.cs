using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingString : MonoBehaviour
{
    public HingeJoint2D begin;
    public DistanceJoint2D end;

    [Range(5,10)]
    public int jointCount;

    public float lineWidth;

    [Header("Object References"),SerializeField]
    Transform stringParent;
    [SerializeField]
    LineRenderer m_line;
    [SerializeField]
    GameObject baseString;

    List<Transform> m_strings=new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < jointCount; i++)
        {
            Vector2 position = Vector2.Lerp(begin.transform.position, end.transform.position, (float)i / ((float)jointCount -1f));
            var obj = Instantiate(baseString, position, Quaternion.identity, stringParent);
            m_strings.Add(obj.transform);
            if (i != 0)
            {
                var joint = m_strings[i - 1].GetComponent<HingeJoint2D>();
                joint.connectedBody = obj.GetComponent<Rigidbody2D>();
            }
            else
            {
                begin.connectedBody = obj.GetComponent<Rigidbody2D>();
            }
        }
        end.connectedBody = begin.GetComponent<Rigidbody2D>();

        // m_strings[0].GetComponent<Rigidbody2D>().isKinematic=true;

        m_line.material = new Material(Shader.Find("Unlit/Color"));
        m_line.positionCount = m_strings.Count;
        m_line.startWidth = lineWidth;
        m_line.endWidth = lineWidth;
        baseString.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int idx = 0;
        foreach (var v in m_strings)
        {
            m_line.SetPosition(idx, v.position);
            idx++;
        }
    }
}
