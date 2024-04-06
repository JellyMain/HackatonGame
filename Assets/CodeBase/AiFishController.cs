using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
public class AiFishController : FishBase
{
    [SerializeField] private Vector2 searchAreaSize;

    [SerializeField] private float seeingDistance;
    [SerializeField] private float seeingAngle;

    Transform target = null;

    private float nextPointDistance = 1f;
    bool reachedPoint = false;

    Path path;

    Seeker seeker;
    Rigidbody2D rb;
    AIPath aIPath;

    int currentWaypoint = 0;

    Vector3 lastPosition;

    SpriteRenderer sprite;

    PolygonCollider2D polygonCollider;

    private float stillTimer;

    Queue<FishBase> threats = new Queue<FishBase>();

    private float threatTimer;

    FishBase chaseTarget;

    private const float threatRemoveTime = 15;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        aIPath = GetComponent<AIPath>();

        sprite = GetComponentInChildren<SpriteRenderer>();

        polygonCollider = GetComponent<PolygonCollider2D>();

        aIPath.maxSpeed = movementSpeed;

        seeingAngle /= 2;

        Vector2[] points = {
            new Vector2(0, 0),
            new Vector2( (Mathf.Tan(seeingAngle) * Mathf.Rad2Deg) * seeingDistance, seeingDistance),
            new Vector2( -(Mathf.Tan(seeingAngle) * Mathf.Rad2Deg) * seeingDistance, seeingDistance),
        };

        polygonCollider.SetPath(0, points);

        target = new GameObject().transform;

        GetRandomPoint();
        InvokeRepeating("SeekPath", 0f, 1f);
    }

    private void Update()
    {
        if (threats.Count > 0)
        {
            if (threatTimer <= 0)
            {
                Debug.Log(threats.Count);
                threats.Dequeue();
                threatTimer = threatRemoveTime;
            }
            else
            {
                threatTimer -= Time.deltaTime;
            }
        }

        if (path == null)
        {
            return;
        }
    
        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedPoint = true;
            
            if(threats.Count > 0)
            {
                GetEscapePoint();
            }
            else if(chaseTarget != null)
            {
                ChaseTarget();
            }
            else
            {
                GetRandomPoint();
            }

            return;
        }
        else
        {
            reachedPoint = false;
        }
    
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextPointDistance)
        {
            currentWaypoint++;
        }

    }

    private void LateUpdate()
    {
        Vector3 movement = transform.position - lastPosition;

        /*if(movement.x <= 0.01f)
        {
            sprite.transform.localScale = new Vector3(1,-1, 1);
        }
        else
        {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }*/

        if (movement == Vector3.zero)
            stillTimer -= Time.deltaTime;
        else
            stillTimer = 3;

        if (stillTimer <= 0)
            GetRandomPoint();


        lastPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishBase fish = collision.GetComponent<FishBase>();
        if (fish == null) return;

        AiFishController aiFish = fish.GetComponent<AiFishController>();
        if(aiFish != null)
        {
            if (aiFish.IsSeeingCollider(collision))
                return;
        }

        if (fish.size > size)
        {
            if (!threats.Contains(fish))
            {
                threats.Enqueue(fish);
                threatTimer = threatRemoveTime;
            }

            GetEscapePoint();
        }
        else if(fish.size < size)
        {
            if (chaseTarget == null)
                chaseTarget = fish;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FishBase fish = collision.GetComponent<FishBase>();
        if (fish == null) return;

        if (fish == chaseTarget)
            chaseTarget = null;
    }

    private void SeekPath()
    {
        if (chaseTarget != null)
            ChaseTarget();

        if(seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    private void GetRandomPoint()
    {
        Vector3 targetPosition = new Vector2(
                Random.Range(-searchAreaSize.x, searchAreaSize.x),
                Random.Range(-searchAreaSize.y, searchAreaSize.y)
            );

        target.position = targetPosition + transform.position;

        SeekPath();
    }

    private void GetEscapePoint()
    {
        Vector3 targetPosition = threats.Peek().transform.position;

        foreach(FishBase f in threats)
        {
            targetPosition = Vector3.Lerp(targetPosition, f.transform.position, 1f / threats.Count);
        }

        target.position = transform.position - targetPosition;
    }

    private void ChaseTarget()
    {
        target.position = chaseTarget.transform.position;
    }

    public bool IsSeeingCollider(Collider2D collider)
    {
        return collider == polygonCollider;
    }


    private void OnDrawGizmos()
    {
        if(target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.position, .1f);
        }
    
        if(threats.Count > 0)
        {
            Gizmos.color = Color.red;
        }
        else if (chaseTarget != null)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up * .5f, .5f);
    }
}
