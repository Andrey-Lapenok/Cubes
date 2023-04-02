using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moving;
    public float speed = 1f;
    private float currentTime, totalTime;
    private bool isMoving = false;
    private Quaternion rot1, rot2;
    private UnityAction buffer;
    public Transform target;

    void Start()
    {
        totalTime = _moving.keys[_moving.keys.Length - 1].time;
        // adress = target.gameObject.GetComponent<Moving>();
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        if (Input.GetKey(KeyCode.Z) && !isMoving)
        {
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, transform.localEulerAngles.y - 90f, 0);
            isMoving = true;
            buffer = target.gameObject.GetComponent<Moving>().moveRight;
            target.gameObject.GetComponent<Moving>().moveRight = target.gameObject.GetComponent<Moving>().moveForward;
            target.gameObject.GetComponent<Moving>().moveForward = target.gameObject.GetComponent<Moving>().moveLeft;
            target.gameObject.GetComponent<Moving>().moveLeft = target.gameObject.GetComponent<Moving>().moveBackward;
            target.gameObject.GetComponent<Moving>().moveBackward = buffer;
        }
        else if (Input.GetKey(KeyCode.X) && !isMoving)
        {
            rot1 = transform.rotation;
            rot2 = Quaternion.Euler(0, transform.localEulerAngles.y + 90f, 0);
            isMoving = true;
            buffer = target.gameObject.GetComponent<Moving>().moveRight;
            target.gameObject.GetComponent<Moving>().moveRight = target.gameObject.GetComponent<Moving>().moveBackward;
            target.gameObject.GetComponent<Moving>().moveBackward = target.gameObject.GetComponent<Moving>().moveLeft;
            target.gameObject.GetComponent<Moving>().moveLeft = target.gameObject.GetComponent<Moving>().moveForward;
            target.gameObject.GetComponent<Moving>().moveForward = buffer;
        }
        if (isMoving)
        {
            if (currentTime >= totalTime)
            {
                rot1 = rot2;
                currentTime = 0;
                isMoving = false;
            }
            else currentTime += Time.deltaTime * speed;

            transform.rotation = Quaternion.Lerp(rot1, rot2, _moving.Evaluate(currentTime));
        }
    }
}