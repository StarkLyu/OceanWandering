using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFish : MonoBehaviour
{
    public float life;
    public float hunger;
    public float speed;
    public float turnSpeed;
    public float floatSpeed;
    public float verticalAngle;
    public float verticalAdjustSpeed;
    public Transform testTarget;
    public bool testChase;
    private Vector3 targetPos;
    enum state
    {
        FREE,
        ESCAPE,
        FIND_FOOD,
        CONTACT_PLAYER,
        DEAD
    }

    enum MoveDir
    {
        FRONT, LEFT, RIGHT, UP, DOWN
    }

    private MoveDir direction;
    private float dirTime;
    private float lastTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        lastTimestamp = Time.time;
        dirTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Swim();
        if (testChase)
        {
            Chase(testTarget);
        }
        else
        {
            AdjustVerticleDir();
            if (Time.time - lastTimestamp >= dirTime)
            {
                direction = (MoveDir)Random.Range(1, 5);
                lastTimestamp = Time.time;
                dirTime = Random.value * 3 + 3;
                Debug.Log(direction);
            }
        }
    }

    private void AdjustVerticleDir()
    {
        Quaternion target = transform.rotation;
        if (direction == MoveDir.UP)
        {
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, verticalAngle);
        }
        else if (direction == MoveDir.DOWN)
        {
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, -verticalAngle);
        }
        else
        {
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, verticalAdjustSpeed * Time.deltaTime);
    }

    private void Swim()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
        float turnAngle = turnSpeed * Time.deltaTime;
        switch (direction)
        {
            case MoveDir.FRONT:
                break;
            case MoveDir.LEFT:
                transform.Rotate(0, -turnAngle, 0, Space.World);
                break;
            case MoveDir.RIGHT:
                transform.Rotate(0, turnAngle, 0, Space.World);
                break;
            default:
                break;
        }
    }
    
    public void Chase(Transform target)
    {
        Vector3 targetDir = transform.InverseTransformPoint(target.position);
        if (targetDir.z > 0)
        {
            direction = MoveDir.LEFT;
        }
        else if (targetDir.z < 0)
        {
            direction = MoveDir.RIGHT;
        }
        else if (targetDir.x < 0)
        {
            direction = Random.value < 0.5 ? MoveDir.LEFT : MoveDir.RIGHT;
        }
        else if (targetDir.x > 0)
        {
            direction = MoveDir.FRONT;
        }
        if (targetDir.y > 0)
        {
            transform.Rotate(0, 0, verticalAdjustSpeed * Time.deltaTime);
        }
        else if (targetDir.y < 0)
        {
            transform.Rotate(0, 0, -verticalAdjustSpeed * Time.deltaTime);
        }
    }

    public void Escape(Transform chaser)
    {
        Vector3 targetDir = transform.InverseTransformPoint(chaser.position);
        if (targetDir.z < 0)
        {
            direction = MoveDir.LEFT;
        }
        else if (targetDir.z > 0)
        {
            direction = MoveDir.RIGHT;
        }
        else if (targetDir.x > 0)
        {
            direction = Random.value < 0.5 ? MoveDir.LEFT : MoveDir.RIGHT;
        }
        else if (targetDir.x < 0)
        {
            direction = MoveDir.FRONT;
        }
        if (targetDir.y < 0)
        {
            transform.Rotate(0, 0, verticalAdjustSpeed * Time.deltaTime);
        }
        else if (targetDir.y > 0)
        {
            transform.Rotate(0, 0, -verticalAdjustSpeed * Time.deltaTime);
        }
    }
}
