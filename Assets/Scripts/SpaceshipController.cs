using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletSpeed = 10.0F;

    [SerializeField]
    float fireCooldown = 0.5F;

    [SerializeField]
    float cooldownTimer = 0F;

    [Header("Movement")]
    [SerializeField]
    Vector2 topRightEdge = Vector2.zero;

    [SerializeField]
    Vector2 bottomLeftEdge = Vector2.zero;

    [SerializeField]
    float speed = 5.0F;

    [Header("Rotation")]
    [SerializeField]
    float pitchFactor = 2.0F;

    [SerializeField]
    float pitchControl = -10.0F;

    [SerializeField]
    float yawFactor = 2.0F;

    [SerializeField]
    float rollControl = 5.0F;

    [SerializeField]
    GameOverManager gameOverManager;

    [Header("Health")]
    [SerializeField]
    float health = 100.0F;

    [SerializeField]
    Slider healthbar;

    Rigidbody _rb;

    Vector3 _lastPosition;

    float _inputX;
    float _inputY;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        healthbar.maxValue = health;
        healthbar.value = health;
    }

    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");

        if (cooldownTimer <= 0f)
        {
            // Check for fire input (you can customize the input key as needed)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BulletHandler();
                cooldownTimer = fireCooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
        if (health <= 0)
        {
            gameOverManager.SetGameOver();
        }
    }

    private void BulletHandler()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
    }

    void FixedUpdate()
    {
        Move();
        Rotation();
    }

    void Rotation()
    {
        float pitchPosition = transform.localPosition.y * pitchFactor;
        float pitchThrow = _inputY * pitchControl;
        float pitch = pitchPosition + pitchThrow;

        float yaw = transform.localPosition.x * yawFactor;
        float roll = _inputX * rollControl;

        transform.localRotation = Quaternion.Euler(-pitch, yaw, roll);
    }

    void Move()
    {
        if (
                (_inputX > 0.0F && _lastPosition.x >= topRightEdge.x) ||
                (_inputX < 0.0F && _lastPosition.x <= bottomLeftEdge.x)
            )
        {
            return;
        }

        if (
                (_inputY > 0.0F && _lastPosition.y >= topRightEdge.y) ||
                (_inputY < 0.0F && _lastPosition.y <= bottomLeftEdge.y)
            )
        {
            return;
        }

        Vector3 velocity = new Vector3(_inputX, _inputY, transform.position.z);
        _rb.velocity = velocity * speed * Time.fixedDeltaTime;

        Vector3 currentPosition = _rb.position;
        Vector3 fixedPosition = CalculateFixedPosition(currentPosition);

        if (fixedPosition != currentPosition)
        {
            _rb.position = fixedPosition;
            _rb.velocity = Vector3.zero;
        }

        _lastPosition = currentPosition;
    }

    Vector3 CalculateFixedPosition(Vector3 currentPosition)
    {
        Vector3 fixedPosition = currentPosition;
        fixedPosition.x = Mathf.Clamp(currentPosition.x, bottomLeftEdge.x, topRightEdge.x);
        fixedPosition.y = Mathf.Clamp(currentPosition.y, bottomLeftEdge.y, topRightEdge.y);
        return fixedPosition;
    }

    public void TakeDamage(float damage)
    {
        health -= Mathf.Abs(damage);
        if (health >= 0)
        {
            healthbar.value = health;
        }
    }

    public void TakeHeal(float heal)
    {
        health += Mathf.Abs(heal);
        if (health >= 0)
        {
            healthbar.value = health;
        }
    }
}
