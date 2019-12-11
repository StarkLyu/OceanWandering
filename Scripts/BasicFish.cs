using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFish : MonoBehaviour
{

    [System.Serializable]
    public struct FishSpeed
    {
        public float speed;
        public float turnSpeed;
        public float floatSpeed;
        public float verticalAngle;
        public float verticalAdjustSpeed;
    }

    public float size;
    public string fishType;
    public float life;
    public float hunger;
    public float sight;

    public FishSpeed normalSpeed;
    public FishSpeed fastSpeed;
    private FishSpeed nowSpeed;
    
    public Transform target;
    public Dictionary<string, Transform> chasers;
    public bool testChase;
    public bool testEscape;
    private MainConfig mainConfig;
    [SerializeField] private SphereCollider sightSphere;
    public enum State
    {
        FREE,
        ESCAPE,
        CHASE,
        CONTACT_PLAYER,
        DEAD
    }

    enum MoveDir
    {
        FRONT, LEFT, RIGHT, UP, DOWN
    }

    enum HorizontalDir
    {
        FRONT, LEFT, RIGHT
    }

    enum VerticalDir
    {
        FRONT, UP, DOWN
    }

    public State state;
    [SerializeField] private MoveDir direction;
    private HorizontalDir horizontalDir;
    private VerticalDir verticalDir;

    // Start is called before the first frame update
    void Start()
    {
        state = State.FREE;
        StartCoroutine(RandMove());
        testChase = false;
        nowSpeed = normalSpeed;
        mainConfig = GameObject.FindGameObjectWithTag("MainConfig").GetComponent<MainConfig>();
        transform.localScale = new Vector3(size, size, size);
        sightSphere.radius = sight / size;
        chasers = new Dictionary<string, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Swim();
        if (testChase)
        {
            ChangeState(State.CHASE);
            testChase = false;
        }
        if (testEscape)
        {
            ChangeState(State.ESCAPE);
            testEscape = false;
        }
    }

    private IEnumerator RandMove()
    {
        nowSpeed = normalSpeed;
        while (true)
        {
            horizontalDir = (HorizontalDir)Random.Range(0, 3);
            verticalDir = (VerticalDir)Random.Range(0, 3);
            yield return new WaitForSeconds(Random.value * 3 + 3);
        }
    }

    public void ChangeState(State s)
    {
        if (s == state)
        {
            return;
        }
        state = s;
        Debug.Log(s);
        switch (s)
        {
            case State.FREE:
                StartCoroutine(RandMove());
                break;
            case State.CHASE:
                StartCoroutine(ChaseMove());
                break;
            case State.ESCAPE:
                StartCoroutine(Escape());
                break;
        }
    }

    private void AdjustVerticleDir()
    {
        Quaternion target = transform.rotation;
        switch (verticalDir)
        {
            case VerticalDir.FRONT:
                target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
                break;
            case VerticalDir.UP:
                target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, nowSpeed.verticalAngle);
                break;
            case VerticalDir.DOWN:
                target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, -nowSpeed.verticalAngle);
                break;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, nowSpeed.verticalAdjustSpeed * Time.deltaTime);
    }

    private void Swim()
    {
        transform.Translate(nowSpeed.speed * Time.deltaTime, 0, 0, Space.Self);
        float turnAngle = nowSpeed.turnSpeed * Time.deltaTime;
        switch (horizontalDir)
        {
            case HorizontalDir.FRONT:
                break;
            case HorizontalDir.LEFT:
                transform.Rotate(0, -turnAngle, 0, Space.World);
                break;
            case HorizontalDir.RIGHT:
                transform.Rotate(0, turnAngle, 0, Space.World);
                break;
            default:
                break;
        }
        AdjustVerticleDir();
    }

    public IEnumerator ChaseMove()
    {
        while (true)
        {
            if (state == State.CHASE)
            {
                Chase(target);
                yield return null;
            }
            else
            {
                yield break;
            }
        }
    }

    public IEnumerator EscapeMove()
    {
        while (true)
        {
            if (state == State.ESCAPE)
            {
                Escape();
                yield return null;
            }
            else
            {
                break;
            }
        }
    }
    
    public void Chase(Transform target)
    {
        nowSpeed = fastSpeed;
        Vector3 targetDir = transform.InverseTransformPoint(target.position);
        if (targetDir.z > 0)
        {
            horizontalDir = HorizontalDir.LEFT;
        }
        else if (targetDir.z < 0)
        {
            horizontalDir = HorizontalDir.RIGHT;
        }
        else if (targetDir.x < 0)
        {
            horizontalDir = Random.value < 0.5 ? HorizontalDir.LEFT : HorizontalDir.RIGHT;
        }
        else if (targetDir.x > 0)
        {
            horizontalDir = HorizontalDir.FRONT;
        }
        if (targetDir.y > 0)
        {
            verticalDir = VerticalDir.UP;
        }
        else if (targetDir.y < 0)
        {
            verticalDir = VerticalDir.DOWN;
        }
        else
        {
            verticalDir = VerticalDir.FRONT;
        }
    }

    public IEnumerator Escape()
    {
        nowSpeed = fastSpeed;
        while (true)
        {
            if (state == State.ESCAPE)
            {
                Vector3 targetDir = new Vector3();
                if (chasers.Count == 0)
                {
                    ChangeState(State.FREE);
                    yield break;
                }
                foreach (Transform i in chasers.Values)
                {
                    targetDir += transform.InverseTransformPoint(i.position);
                }
                if (targetDir.z < 0)
                {
                    horizontalDir = HorizontalDir.LEFT;
                }
                else if (targetDir.z > 0)
                {
                    horizontalDir = HorizontalDir.RIGHT;
                }
                else
                {
                    horizontalDir = Random.value < 0.5 ? HorizontalDir.LEFT : HorizontalDir.RIGHT;
                }
                if (targetDir.y > 0)
                {
                    verticalDir = VerticalDir.DOWN;
                }
                else if (targetDir.y < 0)
                {
                    verticalDir = VerticalDir.UP;
                }
                else
                {
                    verticalDir = Random.value < 0.5 ? VerticalDir.UP : VerticalDir.DOWN;
                }
                if (targetDir.x < 0)
                {
                    yield return new WaitForSeconds(Random.value + 1);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield break;
            }
        }
    }

    public void SeeFish(GameObject seenFish)
    {
        if (mainConfig.PredatorMatch(seenFish.name, name))
        {
            Debug.Log("Don't eat me!!!");
            chasers.Add(seenFish.name, seenFish.transform);
            if (state != State.ESCAPE)
            {
                ChangeState(State.ESCAPE);
            }
        }
    }

    public void LostSightFish(GameObject lostFish)
    {
        if (chasers.ContainsKey(lostFish.name))
        {
            chasers.Remove(lostFish.name);
        }
    }
}
