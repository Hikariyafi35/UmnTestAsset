using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Swim")]
    public float minSpeed = 1.5f;
    public float maxSpeed = 3f;

    [Header("Feeding")]
    public float detectionRadius = 5f;
    public LayerMask foodLayer;

    private Vector2 moveDir;
    private float speed;

    private Transform targetFood;
    private Hunger hunger;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        hunger = GetComponent<Hunger>();
    }

    private void Start()
    {
        RandomSwim();
    }

    private void Update()
    {
        FlipSprite();
    }

    private void FixedUpdate()
    {
        if (hunger != null && hunger.IsHungry())
        {
            Feeding();
        }

        rb.linearVelocity = moveDir * speed;
    }

    void Feeding()
    {
        if (targetFood == null)
            targetFood = FindNearestFood();

        if (targetFood != null)
        {
            moveDir = (targetFood.position - transform.position).normalized;
            speed = maxSpeed;
        }
        else
        {
            RandomSwim();
        }
    }

    Transform FindNearestFood()
    {
        Collider2D[] foods = Physics2D.OverlapCircleAll(transform.position, detectionRadius, foodLayer);

        float closest = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider2D food in foods)
        {
            float dist = Vector2.Distance(transform.position, food.transform.position);

            if (dist < closest)
            {
                closest = dist;
                nearest = food.transform;
            }
        }

        return nearest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);

            if (hunger != null)
                hunger.EatFood();

            targetFood = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        moveDir = normal + Random.insideUnitCircle * 0.5f;
        moveDir.Normalize();

        targetFood = null;
    }

    void RandomSwim()
    {
        moveDir = Random.insideUnitCircle.normalized;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void FlipSprite()
    {
        if (moveDir.x > 0.05f)
            sr.flipX = false;
        else if (moveDir.x < -0.05f)
            sr.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}