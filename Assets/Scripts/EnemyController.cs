using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float speed = 350.0F;

    [SerializeField]
    float damage = 10.0F;

    [SerializeField]
    private float destroyPosition = -15f;

        void Update()
    {
        if (transform.position.z <= destroyPosition)
        {
            Destroy(gameObject);
        }
    }

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = Vector3.back * speed * Time.fixedDeltaTime;
        _rb.velocity = direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpaceshipController controller = other.GetComponent<SpaceshipController>();
            controller.TakeDamage(damage);
        }
    }
}
