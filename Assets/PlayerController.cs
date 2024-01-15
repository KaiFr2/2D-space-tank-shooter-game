using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float bulletSpeed = 10f;
    public GameObject orbPrefab;
    public Transform bulletSpawnPoint; // Create an empty GameObject and assign it to this field

    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Player rotation to face the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Shooting when the player clicks
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }


    void Shoot()
    {
        // Instantiate the orb at the bulletSpawnPoint position with the player's rotation
        GameObject bullet = Instantiate(orbPrefab, bulletSpawnPoint.position, transform.rotation);

        // Access the rigidbody component of the bullet and check if it exists
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set the velocity of the bullet to make it move in the direction the player is facing
            rb.velocity = transform.up * bulletSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on orbPrefab!");
        }

        // Destroy the bullet after 3 seconds (adjust the time as needed)
        Destroy(bullet, 3f);
    }
}
