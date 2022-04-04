using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visual")]
    public Camera playerCamera;
    [Header("Gameplay")]
    private int initialAmmo = 8;
    private int ammo;
    public int Ammo { get { return ammo; } }

    public int initialHealth = 100;
    private int health;
    public int Health { get { return health; } }

    public float knockbackForce = 75f;
    private bool isHurt;
    public float hurtDuration = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                // Decrease the Ammo
                ammo--;
                // Object Pooling Scripts + Creating a Bullet
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet();
                bulletObject.transform.position = playerCamera.transform.position + transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }

        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<AmmoCrate>() != null)

        {
            // Get Ammo and Store
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();

            // Add Ammo
            ammo += ammoCrate.ammo;

            // Destroy the Ammo Crate
            Destroy(ammoCrate.gameObject);
        }

        else if (otherCollider.GetComponent<Enemy>() != null)
        {
            if (isHurt == false)
            {
                // Get Enemy and Store 
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                // Taking actual damage
                health -= enemy.damage;
                // is Hurt is triggering
                isHurt = true;

                // Perform the knockback
                Vector3 hurtDirection = (transform.position - enemy.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());


            }

        }


    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false; 
    }

}

