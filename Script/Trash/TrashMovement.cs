using UnityEngine;

public class TrashMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Float Speed")]
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.8f;

    [Header("Floating")]
    public float changeDirectionTime = 2f;
    public float smoothTurnSpeed = 3f;

    [Header("Hit By Fish")]
    public float pushForce = 2f; // dorongan saat ditabrak ikan

    private Vector2 currentDir;
    private Vector2 targetDir;
    private float speed;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        pushForce = ConfigManager.Data.pushForce;
        PickDirection();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
            PickDirection();
    }

    private void FixedUpdate()
    {
        currentDir = Vector2.Lerp(
            currentDir,
            targetDir,
            smoothTurnSpeed * Time.fixedDeltaTime
        ).normalized;

        Vector2 floatVelocity = currentDir * speed;
        rb.linearVelocity = Vector2.Lerp
        (
        rb.linearVelocity,
        floatVelocity,
        2f * Time.fixedDeltaTime
        );
    }

    void PickDirection()
    {
        targetDir = Random.insideUnitCircle.normalized;
        speed = Random.Range(minSpeed, maxSpeed);
        timer = changeDirectionTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;

        // Mantul dari wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            currentDir = Vector2.Reflect(currentDir, normal).normalized;
            targetDir = currentDir;

            transform.position += (Vector3)(normal * 0.05f);
        }

        // Kalau ditabrak ikan -> terdorong sedikit
        if (collision.gameObject.CompareTag("Fish"))
        {
            Vector2 pushDir =
                ((Vector2)transform.position -
                (Vector2)collision.transform.position).normalized;

            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }
    }
}