using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class TeleportControll : MonoBehaviour
{
    [SerializeField] AnimationCurve changing;
    [HideInInspector] public bool isTeleporting = true;
    public float speed;
    public enum typeM {level = 1, scene = 2};
    public enum filter {cube = 1, clone = 2, cube_and_clone = 3};
    public typeM typeOfMovement;
    public filter typeOfFilter;
    public string goToScene;
    public Transform goTo;
    
    private AudioSource a;
    private bool isStaying = false;
    private GameObject _object;
    private float currentTime, totalTime;
    private Renderer rend;
    private Color startColor = new Color(0, 1, 1, 1), endColor = new Color(1, 1, 0.5f, 1);

    void Start()
    {
        a = gameObject.GetComponent<AudioSource>();
        totalTime = changing.keys[changing.keys.Length - 1].time;
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider collider)
    {
    	if ((typeOfFilter == filter.cube && collider.transform.tag == "cube") || (typeOfFilter == filter.clone && collider.transform.tag == "clone") || (typeOfFilter == filter.cube_and_clone)) {
	    	if (isTeleporting) {
		    	_object = collider.gameObject;
		        currentTime = 0;
		        startColor = new Color(0, 1, 1, 1); endColor = new Color(1, 1, 0.5f, 1);
		        StartCoroutine("ChangeColor");
		        a.Play();
	    	}
	    	isStaying = true;
	    }
    }
    void OnTriggerExit(Collider collider)
    {
    	if ((typeOfFilter == filter.cube && collider.transform.tag == "cube") || (typeOfFilter == filter.clone && collider.transform.tag == "clone") || (typeOfFilter == filter.cube_and_clone)) {
	    	if (isTeleporting) {
	        	currentTime = 0;
	        	startColor = new Color(1, 1, 0.5f, 1); endColor = new Color(0, 1, 1, 1);
	        	StartCoroutine("ChangeColor");
	        }
	    	isTeleporting = true;
	    	isStaying = false;
	    }
    }

    IEnumerator ChangeColor()
    {
        yield return null;
        currentTime += Time.deltaTime * speed;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(currentTime));
        if (currentTime < totalTime) StartCoroutine("ChangeColor");
        else StartCoroutine("Teleport");
    }
    IEnumerator Teleport() {
    	yield return null;
    	if (isStaying) {
	    	switch(typeOfMovement) {
	      		case typeM.level:
	       			goTo.gameObject.GetComponent<TeleportControll>().isTeleporting = false;

	       			_object.transform.position = new Vector3(goTo.position.x, 1f, goTo.position.z);
	       			break;
	       		case typeM.scene: SceneManager.LoadScene(goToScene);
	       			break;
	       	}
	    }
    }
} 