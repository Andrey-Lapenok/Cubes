using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moving;

    public float speed = 3.5f;
    [HideInInspector] public GameObject clone, allClones;

    [HideInInspector] public List<string> moves = new List<string>();
    [HideInInspector] public List<float> delays = new List<float>();
    [HideInInspector] public bool isRecording = false;
    [HideInInspector] public bool isMoving = false, canMove = true;
    [HideInInspector] public Vector3 pos1, pos2;
    [HideInInspector] public UnityAction moveRight, moveLeft, moveForward, moveBackward;
    [HideInInspector] public UnityAction[] actions = new UnityAction[4]; //{moveRight, moveLeft, moveForward, moveBackward};
    
    private AudioSource a;
    private float currentTime, totalTime, buffer, delay = 0;
    private Vector3 startPosition;
    private Quaternion rot1, rot2;

    void Start()
    {
        a = gameObject.GetComponent<AudioSource>();
        actions[0] = MoveRight;
        actions[1] = MoveLeft;
        actions[2] = MoveForward;
        actions[3] = MoveBackward;
        moveRight = MoveRight;
        moveLeft = MoveLeft;
        moveForward = MoveForward;
        moveBackward = MoveBackward;
        totalTime = _moving.keys[_moving.keys.Length - 1].time;
    }

    void Update()
    {
        if (canMove && !isMoving) {
            if (Input.GetKey(KeyCode.RightArrow)) {a.Play(); moveRight.Invoke();}

            else if (Input.GetKey(KeyCode.LeftArrow)) {a.Play(); moveLeft.Invoke();}

            else if (Input.GetKey(KeyCode.UpArrow)) {a.Play(); moveForward.Invoke();}

            else if (Input.GetKey(KeyCode.DownArrow)) {a.Play(); moveBackward.Invoke();}
        }

        if (isRecording && !isMoving) delay += Time.deltaTime;

        if (isMoving)
        {
            if (currentTime >= totalTime)
            {
                pos1 = pos2;
                currentTime = 0;
                isMoving = false;
            }
            else
            {
                currentTime += Time.deltaTime * speed;
            }
            pos1.y = (Mathf.Sqrt(0.5f - Mathf.Pow( _moving.Evaluate(currentTime) - 0.5f, 2f)) - 0.5f + buffer);
            pos2.y = pos1.y;
            transform.position = Vector3.Lerp(pos1, pos2, _moving.Evaluate(currentTime));
            transform.rotation = Quaternion.Lerp(rot1, rot2, _moving.Evaluate(currentTime));
        }
        if (Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            isRecording = !isRecording;
            if (isRecording)
            {
                moves = new List<string>();
                delays = new List<float>();
                startPosition = transform.position;
                foreach (Transform i in allClones.transform)
                {
                    i.GetComponent<CloneControll>().startColor = new Color(1, 1, 1, 1f);
                    i.GetComponent<CloneControll>().endColor = new Color(1, 1, 1, 0.5f);
                    i.GetComponent<CloneControll>().StartCoroutine("ChangeColor");
                }
            }
            else
            {
                delays.Add(delay);
                Instantiate(clone, startPosition, Quaternion.identity, allClones.transform);
                foreach (Transform i in allClones.transform)
                {
                    i.GetComponent<CloneControll>().startColor = new Color(1, 1, 1, 0.5f);
                    i.GetComponent<CloneControll>().endColor = new Color(1, 1, 1, 1f);
                    i.GetComponent<CloneControll>().StartCoroutine("ChangeColor");
                }
            }
            delay = 0;
        }
    }

    void MoveRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(1f, 0, 0), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(1f, 1f, 0), Vector3.down, out hit, 1f)) {
            if (isRecording) {
                moves.Add("MoveRight");
                delays.Add(delay);
                delay = 0;
            }

            isMoving = true;
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(1f, 0, 0);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, 0, -90f);
            buffer = transform.position.y;
        }
    }

    void MoveLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(-1f, 0, 0), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(-1f, 1f, 0), Vector3.down, out hit, 1f))
        {
            if (isRecording) {
                moves.Add("MoveLeft");
                delays.Add(delay); 
                delay = 0;
            }
            isMoving = true;
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(-1f, 0, 0);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, 0, 90f);
            buffer = transform.position.y;
        }
    }

    void MoveForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 1f), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(0f, 1f, 1f), Vector3.down, out hit, 1f))
        {
            if (isRecording) {
                moves.Add("MoveForward");
                delays.Add(delay);
                delay = 0;
            }
            isMoving = true;
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(0, 0, 1f);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(90f, 0, 0);
            buffer = transform.position.y;
        }
    }

    void MoveBackward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0, -1f), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(0, 1f, -1f), Vector3.down, out hit, 1f))
        {
            if (isRecording) {
                moves.Add("MoveBackward");
                delays.Add(delay);
                delay = 0;
            }
            isMoving = true;
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(0, 0, -1f);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(-90f, 0, 0);
            buffer = transform.position.y;
        }
    }
}