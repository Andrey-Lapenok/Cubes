using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shift : MonoBehaviour
{
    [HideInInspector] List<GameObject> objects = new List<GameObject>();
    [HideInInspector] List<float> curs = new List<float>();
    [HideInInspector] List<float> pos_y = new List<float>();

    [SerializeField] AnimationCurve _moving;
    
    public float speed;
    public Vector3 pos2;
    private Vector3 pos1;
    private float buffer, upper;
    private GameObject obj;
    public AudioSource a;

    private float currentTime, totalTime;

    void Start()
    {
    	pos1 = new Vector3(transform.position.x, 1f, transform.position.z);
        totalTime = _moving.keys[_moving.keys.Length - 1].time;
        StartCoroutine("Moving");
    }

    void OnTriggerEnter(Collider collision) {
        obj = collision.gameObject;
        if (obj.transform.tag == "cube") obj.GetComponent<Moving>().canMove = false;
        a.Play();
        StartCoroutine("PreMoving");
    }
    IEnumerator PreMoving() {
    	yield return new WaitForSeconds(0.15f);

    	buffer = obj.transform.position.y;

        if (obj.transform.tag == "cube") {obj.GetComponent<Moving>().pos1 = pos2; obj.GetComponent<Moving>().pos2 = pos2;}
        else if (obj.transform.tag == "clone") {obj.GetComponent<CloneControll>().pos1 = pos2; obj.GetComponent<CloneControll>().pos2 = pos2;}

        upper = Vector3.Distance(new Vector3(pos1.x, 0, pos1.z), new Vector3(pos2.x, 0, pos2.z));
        objects.Add(obj);
        curs.Add(0);
        pos_y.Add(1f);
    }
    IEnumerator Moving() {
        yield return null;
        for (int i = 0; i < objects.Count; i++) {
            curs[i] += Time.deltaTime * speed;
            pos_y[i] = upper * (Mathf.Sqrt(0.25f - Mathf.Pow(_moving.Evaluate(curs[i]) - 0.5f, 2f))) + buffer;

            objects[i].transform.position = new Vector3(objects[i].transform.position.x, pos_y[i], objects[i].transform.position.z);
            objects[i].transform.position = Vector3.Lerp(new Vector3(pos1.x, objects[i].transform.position.y, pos1.z), new Vector3(pos2.x, pos_y[i], pos2.z), _moving.Evaluate(curs[i]));
        }
        for (int i = 0; i < objects.Count; i++)
            if (curs[i] >= totalTime) {if (objects[i].transform.tag == "cube") obj.GetComponent<Moving>().canMove = true; objects.Remove(objects[i]); curs.Remove(curs[i]); pos_y.Remove(pos_y[i]);}
        StartCoroutine("Moving");
    }
}