using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    [SerializeField] AnimationCurve changing;
    public float speed;

    private Collider _collision;
    private float currentTime, totalTime;
    private Renderer rend;
    private Color startColor = new Color(0, 1, 1, 1), endColor = new Color(1, 1, 0.5f, 1);

    void Start()
    {
        totalTime = changing.keys[changing.keys.Length - 1].time;
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider collision) {
    	_collision = collision;
        currentTime = 0;
        startColor = new Color(0, 1, 1, 1); endColor = new Color(1, 1, 0.5f, 1);
        StartCoroutine("ChangeColor");
    }
    void OnTriggerExit(Collider collision) {
        currentTime = 0;
        startColor = new Color(1, 1, 0.5f, 1); endColor = new Color(0, 1, 1, 1);
        StartCoroutine("ChangeColor");
    }

    IEnumerator ChangeColor()
    {
        yield return null;
        currentTime += Time.deltaTime * speed;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(currentTime));
        if (currentTime < totalTime) StartCoroutine("ChangeColor");
        else {
        	if (_collision != null && _collision.transform.tag == "clone") {
                print(1);
        		_collision.gameObject.GetComponent<CloneControll>().Respawn();
                _collision = null;
        	}
        }
    }
}
