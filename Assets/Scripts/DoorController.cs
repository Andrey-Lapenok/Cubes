using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] AnimationCurve changing;
    [HideInInspector] public Color startColor = new Color(1, 1, 1, 0), endColor = new Color(1, 1, 1, 1);
    public float speed;
    private Renderer rend;
    private float currentTime, totalTime;

    void Start()
    {
        totalTime = changing.keys[changing.keys.Length - 1].time;
        rend = GetComponent<Renderer>();
    }

    public void Open()
    {
        startColor = rend.material.color;
        endColor = new Color(1, 1, 1, 0);
        currentTime = 0;
        StartCoroutine("OpenOrClose");
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void Close()
    {
        startColor = rend.material.color;
        endColor = new Color(1, 1, 1, 1);
        currentTime = 0;
        StartCoroutine("OpenOrClose");
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator OpenOrClose()
    {
        yield return null;
        currentTime += Time.deltaTime * speed;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(currentTime));
        if (currentTime < totalTime) StartCoroutine("OpenOrClose");
    }
}