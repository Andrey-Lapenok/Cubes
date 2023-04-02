using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneControll : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moving, changing;

    [SerializeField] public List<string> moves = new List<string>();
    [HideInInspector] public List<float> delays = new List<float>();
    [HideInInspector] public Vector3 pos1, pos2;
    [HideInInspector] public Color startColor = new Color(1, 1, 1, 0.5f), endColor = new Color(1, 1, 1, 1f);
    [HideInInspector] public bool isMoving = false;
    public GameObject cube;
    public float speed = 3.5f;
    private bool isClonesMoving = false;
    private float currentTime = 0, totalTime, colorCurentTime = 0, colorTotalTime, buffer;
    private Quaternion rot1, rot2;
    private int i = 0;
    private Vector3 startPosition;
    private Renderer rend;

    void Start()
    {
        foreach (string i in cube.GetComponent<Moving>().moves) moves.Add(i);
        foreach (float i in cube.GetComponent<Moving>().delays) delays.Add(i);
        rend = GetComponent<Renderer>();
        totalTime = _moving.keys[_moving.keys.Length - 1].time;
        colorTotalTime = changing.keys[changing.keys.Length - 1].time;
        startPosition = transform.position;
        // startColor = new Color(1, 1, 1, 0.5f);
    }

    void OnEnable() => StartCoroutine("_Update");

    IEnumerator _Update()
    {
        yield return null;
        RaycastHit hit;
        if ((transform.position == cube.transform.position && !cube.GetComponent<Moving>().isRecording) || Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out hit, 1f)) Destroy(gameObject);

        if (((moves.Count != 0 && Input.GetKey(KeyCode.Q)) || isClonesMoving))
        {
            isClonesMoving = true;
            if (isMoving)
            {
                if (currentTime >= totalTime)
                {
                    pos1 = pos2;
                    currentTime = 0;
                    isMoving = false;
             
                    if (i == moves.Count) {
                        yield return new WaitForSeconds(delays[i]);
                        Respawn();
                    }
                }
                else
                {
                    currentTime += Time.deltaTime * speed;
                }
                pos1.y = (Mathf.Sqrt(0.5f - Mathf.Pow(_moving.Evaluate(currentTime) - 0.5f, 2f)) - 0.5f + buffer);
                pos2.y = pos1.y;
                transform.position = Vector3.Lerp(pos1, pos2, _moving.Evaluate(currentTime));
                transform.rotation = Quaternion.Lerp(rot1, rot2, _moving.Evaluate(currentTime));
            }
            else
            {
                isMoving = true;
                yield return new WaitForSeconds(delays[i]);

                if (moves[i] == "MoveRight") MoveRight();

                else if (moves[i] == "MoveLeft") MoveLeft();

                else if (moves[i] == "MoveForward") MoveForward();

                else MoveBackward();

                buffer = transform.position.y;

                i++;
            }
        }
        StartCoroutine("_Update");
    }
    void MoveRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(1f, 0, 0), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(1f, 1f, 0), Vector3.down, out hit, 1f))
        {   
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(1f, 0, 0);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, 0, -90f);
        }
        else {
            Respawn();
        }
    }

    void MoveLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(-1f, 0, 0), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(-1f, 1f, 0), Vector3.down, out hit, 1f))
        {
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(-1f, 0, 0);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, 0, 90f);
        }
        else {
            Respawn();
        }
    }

    void MoveForward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 1f), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(0f, 1f, 1f), Vector3.down, out hit, 1f))
        {
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(0, 0, 1);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(90f, 0, 0);
        }
        else {
            Respawn();
        }

    }
    void MoveBackward()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 0, -1f), Vector3.down, out hit, 1f) && !Physics.Raycast(transform.position + new Vector3(0, 1f, -1f), Vector3.down, out hit, 1f))
        {
            pos1 = transform.position;
            pos2 = transform.position + new Vector3(0, 0, -1);
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(-90f, 0, 0);
        }
        else {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = startPosition;
        transform.rotation = rot1;
        pos1 = startPosition;
        isClonesMoving = false;
        isMoving = false;
        i = 0;
        currentTime = 0;
    }

    public IEnumerator ChangeColor()
    {
        yield return null;
        colorCurentTime += Time.deltaTime * speed;
        rend.material.color = Color.Lerp(startColor, endColor, changing.Evaluate(colorCurentTime));
        if (colorCurentTime < colorTotalTime) StartCoroutine("ChangeColor");
        else colorCurentTime = 0;
    }

    void OnCollisionEnter(Collision collision) { if (collision.gameObject.tag == "clone") Respawn(); }
}