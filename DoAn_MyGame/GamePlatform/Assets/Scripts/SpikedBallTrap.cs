using UnityEngine;
using System.Collections.Generic;

public class SpikedBallTrap : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] public bool closedLoop = false;
    [SerializeField] public bool clockWise = false;
    [SerializeField] public GameObject chainPoint;
    [SerializeField] public GameObject spikedBallPrefab;

    [Header("Config")]
    [SerializeField] private float closedLoopSpeedMultiplier = 0.3f;
    [SerializeField] public float delta = 0.5f;
    [SerializeField] public float radius = 5.0f;
    [Range(0, 360)] public float angleRange = 180f;
    [Range(0, 360)] public float startingAngle = 0f;
    [SerializeField] public float rotationSpeed = 2f;

    public enum EaseType { Linear, InOutSine }
    public EaseType rotationEase = EaseType.InOutSine;

    private GameObject ballInstance;
    private List<GameObject> chainLinks = new List<GameObject>();
    private float currentAngle;
    private float time;

    void Start()
    {
        currentAngle = startingAngle;

        if (spikedBallPrefab != null)
        {
            Vector3 pos = GetPositionFromAngle(currentAngle, radius) + transform.position;
            ballInstance = Instantiate(spikedBallPrefab, pos, Quaternion.identity, transform);
        }

        if (chainPoint != null)
        {
            chainLinks.Clear();
            for (float d = 0; d < radius; d += delta)
            {
                Vector3 pos = GetPositionFromAngle(currentAngle, d) + transform.position;
                var link = Instantiate(chainPoint, pos, Quaternion.identity, transform);
                chainLinks.Add(link);
            }
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        float direction = clockWise ? 1f : -1f;

        float t;
        float eased;

        if (closedLoop)
        {
            t = Mathf.Repeat(time * rotationSpeed * closedLoopSpeedMultiplier, 1f);

            switch (rotationEase)
            {
                case EaseType.Linear:
                    eased = t-3;
                    break;
                case EaseType.InOutSine:
                    eased = EaseInOutSine(t);
                    break;
                default:
                    eased = t;
                    break;
            }

            currentAngle = startingAngle + direction * angleRange * eased;
        }
        else
        {
            switch (rotationEase)
            {
                case EaseType.Linear:
                    t = Mathf.PingPong(time * rotationSpeed * 0.3f, 1f);
                    eased = t;
                    break;

                case EaseType.InOutSine:
                    t = Mathf.PingPong(time * rotationSpeed * 1.8f, 1f);
                    eased = EaseInOutSine(t);
                    break;

                default:
                    t = Mathf.PingPong(time * rotationSpeed, 1f);
                    eased = t;
                    break;
            }

            currentAngle = startingAngle + direction * angleRange * eased;
        }

        if (ballInstance != null)
        {
            ballInstance.transform.position = GetPositionFromAngle(currentAngle, radius) + transform.position;
        }

        for (int i = 0; i < chainLinks.Count; i++)
        {
            float d = i * delta;
            chainLinks[i].transform.position = GetPositionFromAngle(currentAngle, d) + transform.position;
        }
    }

    float EaseInOutSine(float t)
    {
        return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
    }

    Vector3 GetPositionFromAngle(float angleDeg, float r)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * r;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        int segments = 60;
        float direction = clockWise ? 1f : -1f;
        float drawRange = angleRange;
        float step = drawRange / segments;

        Vector3 prev = transform.position + GetPositionFromAngle(startingAngle, radius);
        for (int i = 1; i <= segments; i++)
        {
            float angle = startingAngle + direction * step * i;
            Vector3 next = transform.position + GetPositionFromAngle(angle, radius);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }

        Gizmos.color = Color.yellow;
        Vector3 spikeStart = transform.position + GetPositionFromAngle(startingAngle, radius);
        Gizmos.DrawLine(transform.position, spikeStart);
    }
}
