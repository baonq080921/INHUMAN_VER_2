using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class ExampleClass : MonoBehaviour
{
    public float time = 1.0f;
    private TrailRenderer tr;

    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
    }

    void Update()
    {
        tr.time = time;
        tr.transform.position = new Vector3(Mathf.Sin(Time.time * 1.51f) * 7.0f, Mathf.Cos(Time.time * 1.27f) * 4.0f, 0.0f);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(25, 20, 200, 30), "Time");
        time = GUI.HorizontalSlider(new Rect(125, 25, 200, 30), time, 0.1f, 10.0f);
    }
}
