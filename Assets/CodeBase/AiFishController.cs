using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class AiFishController : FishBase
{
    /*private Vector2 diretion;
    private Vector2 velocity;
    
    [SerializeField] private float accelerationTime;
    [SerializeField] private float speed;

    [SerializeField] private float seeingAngle;
    [SerializeField] private float seeingDistance;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private PolygonCollider2D polygonCollider;

    private float swimTime = 3;

    private Vector2 boundry = new Vector2(12f, 12f);

    private enum State
    {
        Normal,
        Atacking,
        Fleeing
    }

    private State state;

    FishBase fish;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();

        seeingAngle /= 2;

        Vector2[] points = { 
            new Vector2(0, 0), 
            new Vector2(seeingDistance, (Mathf.Tan(seeingAngle) / Mathf.PI) * seeingDistance), 
            new Vector2(seeingDistance, -(Mathf.Tan(seeingAngle) / Mathf.PI) * seeingDistance), 
        };

        polygonCollider.SetPath(0, points);

        SetRandomDirrection();
    }

    // Update is called once per frame
    void Update()
    {
        //Applying velocity
        if (transform.position.x > boundry.x)
        {
            diretion.x = -1;
        }
        else if (transform.position.x < -boundry.x)
        {
            diretion.x = 1;
        }

        if (transform.position.y > boundry.y)
        {
            diretion.y = -1;
        }
        else if (transform.position.y < -boundry.y)
        {
            diretion.y = 1;
        }

        velocity = Vector2.Lerp(velocity, diretion * speed, (1f / accelerationTime) * Time.deltaTime);

        rb.velocity = velocity;

        if(state == State.Normal)
        {
            if (swimTime > 0)
            {
                swimTime -= Time.deltaTime;
            }
            else
            {
                swimTime = 10;
                SetRandomDirrection();
            }
        }
        else if(state == State.Atacking) 
        {
            diretion = transform.position - transform.position;
        }
        // Rotating
        float swapX = velocity.x;

        Angle a = Vector2.Angle(new Vector2(s, 0), velocity);

        Quaternion rotation = Quaternion.AngleAxis(a.value, Vector3.forward);

        rotation.Set(0, swapX < 0 ? -2 : 2, rotation.z, rotation.w);

        transform.rotation = rotation;

        //transform.localScale = new Vector3(swapX < 0 ? -1 : 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishBase f = 
    }



    private void SetRandomDirrection()
    {
        diretion = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
            ).normalized;
    }

    public int GetSize()
    {
        return size;
    }*/

    [SerializeField] float searchAreaBottomWidth = 4;
    [SerializeField] float searchAreaUpperWidth = 6;
    [SerializeField] float searchAreaHeight = 3;

    [SerializeField] float seeingAngle;
    [SerializeField] float seeingDistance;

    Vector2 aPoint, bPoint, cPoint, dPoint, destinationPoint = Vector2.zero;

    Rigidbody2D rb;
    PolygonCollider2D polygonCollider;

    //FishBase fish;
    Dictionary<string, FishBase> fishesInView = new Dictionary<string, FishBase> ();

    float targetAngle = 0;

    float stun = 0;

    private enum State
    {
        Normal,
        Atacking,
        Fleeing
    }

    private State state = State.Normal;
    private FishBase target;
    private bool hasStarted;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        CalculateSearchArea();

        polygonCollider = GetComponent<PolygonCollider2D>();

        seeingAngle /= 2;

        Vector2[] points = {
            new Vector2(0, 0),
            new Vector2(seeingDistance, (Mathf.Tan(seeingAngle) / Mathf.PI) * seeingDistance),
            new Vector2(seeingDistance, -(Mathf.Tan(seeingAngle) / Mathf.PI) * seeingDistance),
        };

        polygonCollider.SetPath(0, points);
    }

    protected override void FixedUpdate()
    {
        /*if(fishesInView != null)
        {
            List<FishBase> biggerFishes = new List<FishBase>();
            List<FishBase> smallerFishes = new List<FishBase>();

            foreach(FishBase f in fishesInView.Values)
            {
                if(f.size > size)
                {
                    biggerFishes.Add(f);
                }
                else if(f.size < size)
                {
                    smallerFishes.Add(f);
                }
            }
            
            if(biggerFishes.Count > 0)
            {
                state = State.Normal;

                Vector3 fishPosition = biggerFishes[0].transform.position;

                Vector2 escapePoint =  transform.position - fishPosition;

                destinationPoint = escapePoint.normalized * 3 * movementSpeed;
            }
            else if(smallerFishes.Count > 0)
            {
                state = State.Atacking;
                destinationPoint = smallerFishes[0].transform.position;
            }
        }*/
        base.FixedUpdate();

        if(stun > 0)
        {
            stun -= Time.fixedDeltaTime;
            rb.velocity = Vector2.zero;
        }
        else
        {
            SetMovement();
        }
    }

    public void SetMovement()
    {
        Vector2 velocity = CalculateMove();

        /*if(state == State.Atacking)
        {
            velocity = Vector2.zero;
        }
        else if(state == State.Fleeing)
        {
            velocity = Vector2.zero;
        }
        else
        {
            velocity = CalculateMove();
        }*/


        rb.velocity = velocity;


        targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    public Vector2 CalculateMove()
    {
        if ((destinationPoint - (Vector2)transform.position).magnitude < 1 || !hasStarted)
        {
            destinationPoint = GetRandomPoint(aPoint, bPoint, cPoint, dPoint);

            if (state == State.Atacking)
                stun = 0.5f;
            else if (state == State.Fleeing)
            {
                destinationPoint = GetEscapePoint();
            }
            else
                state = State.Normal;
        
            hasStarted = true;
        }

        if(state == State.Fleeing) 
        {
            destinationPoint = GetEscapePoint();
        }

        else if(state == State.Atacking)
        {
            if(target == null)
                state = State.Normal;
            destinationPoint = target.transform.position;
        }

        Vector2 destinationPointDirection = (destinationPoint - (Vector2)transform.position).normalized;

        return destinationPointDirection;
    }

    private Vector2 GetRandomPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        if (Random.value < 0.5f)
        {
            return GetdestinationPointInTriangle(a, b, d);
        }
        else
        {
            return GetdestinationPointInTriangle(d, b, c);
        }
    }


    private Vector2 GetdestinationPointInTriangle(Vector2 a, Vector2 b, Vector2 c)
    {
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1f)
        {
            u = 1f - u;
            v = 1f - v;
        }

        return (1 - u - v) * a + u * b + v * c;
    }

    private void CalculateSearchArea()
    {
        Vector2 up = transform.up;
        Vector2 forward = transform.right;

        aPoint = (Vector2)transform.position + (up * searchAreaBottomWidth);
        bPoint = (Vector2)transform.position + (up * searchAreaUpperWidth) + (forward * searchAreaHeight);
        cPoint = (Vector2)transform.position - (up * searchAreaUpperWidth) + (forward * searchAreaHeight);
        dPoint = (Vector2)transform.position - (up * searchAreaBottomWidth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FishBase fish = collision.GetComponent<FishBase>();

        if(fish != null)
        {
            if(!fishesInView.ContainsKey(collision.name))
                fishesInView.Add(collision.name, fish);
            
            if(fish.size > size && target == null)
            {

                target = fish;
                state = State.Fleeing;
                destinationPoint = GetEscapePoint();
            }

            if(state != State.Fleeing && fish.size < size)
            {
                state = State.Atacking;
                target = fish;
                destinationPoint = fish.transform.position;
            }
        }
    }

    public bool IsEyeCollider(Collider2D collider)
    {
        return collider == polygonCollider;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FishBase fish = collision.GetComponent<FishBase>();

        if(state == State.Atacking)
        {
            target = null;
            state = State.Normal;
        }    

        if (fish != null)
        {
            fishesInView.Remove(collision.name);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(aPoint, bPoint);
        Gizmos.DrawLine(bPoint, cPoint);
        Gizmos.DrawLine(cPoint, dPoint);
        Gizmos.DrawLine(dPoint, aPoint);

        Gizmos.DrawLine(transform.position, destinationPoint);
    }

    private Vector2 GetEscapePoint()
    {
        Vector3 fishPosition = Vector3.zero;

        List<FishBase> biggerFishes = new List<FishBase>();
        foreach (FishBase f in fishesInView.Values)
        {
            if (fishPosition != Vector3.zero)
            {
                fishPosition = Vector3.Lerp(f.transform.position, fishPosition, 0.5f);
            }
            else
            {
                fishPosition = f.transform.position;
            }
        }

        Vector2 escapePoint = Vector3.MoveTowards(transform.position, fishPosition, 10000) * -1 ;

        state = State.Fleeing;
        return escapePoint.normalized * movementSpeed;
    }
}
