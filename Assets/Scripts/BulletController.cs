using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float bulletSpeed = 10.0F;

    [SerializeField]
    float bulletLifetime = 10.0F;

    [SerializeField]
    float lifeTimer = 0.0F;

    [SerializeField]
    private float destroyPosition = -15f;

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);

        // Update the lifetime timer
        lifeTimer += Time.deltaTime;

        // Check if the bullet has exceeded its lifetime
        if (lifeTimer >= bulletLifetime)
        {
            Destroy(gameObject); // Destroy the bullet
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PointController pointController = FindObjectOfType<PointController>();

        if (other.CompareTag("Enemy") && !hitEnemies.Contains(other.gameObject))
        {

            hitEnemies.Add(other.gameObject);

            pointController.AddPoint();

            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PoweUp"))
        {
            Destroy(gameObject);
        }
    }
}
