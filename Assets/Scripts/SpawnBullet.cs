using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject enemy;
    public float projectileSpeed = 10f;
    public float deactivateTime = 5f;
    public int maxProjectiles = 80;

    private Stack<GameObject> projectiles = new Stack<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && projectiles.Count < maxProjectiles)
        {
            SpawnProjectile();

        }
    }

    void SpawnProjectile()
    {

        GameObject projectile;
        if (projectiles.Count > 0)
        {
            projectile = projectiles.Pop();
            projectile.SetActive(true);
        }
        // Si no hay proyectiles en la pila, se crea un nuevo proyectil
        else
        {
            projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }

        StartCoroutine(LaunchAndDeactivate(projectile));
    }

    IEnumerator LaunchAndDeactivate(GameObject projectile)
    {
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        float startTime = Time.time;

        while (Time.time - startTime < deactivateTime)
        {
            if (!projectile.activeSelf)
            {
                break;
            }

            projectile.transform.position += direction * projectileSpeed * Time.deltaTime;
            yield return null;
        }

        projectile.SetActive(false);
        projectiles.Push(projectile);
    }
}
