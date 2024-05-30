using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3 lastSeen;
    Vector3 target;

    public GameObject player;
    public float viewDistance;
    public float viewAngle;
    public float rotationSpeed; 

   
    void Start()
    {
        lastSeen = transform.position;
        target = player.transform.position;
    }

    bool SeePlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        if (dir.magnitude < viewDistance)
        {
            Debug.DrawRay(transform.position, transform.up * viewDistance, Color.yellow);
            Debug.DrawRay(transform.position, dir.normalized * viewDistance, Color.yellow);
            float angle = Vector3.Dot(transform.up, dir.normalized);
            if ((Mathf.Acos(angle)) * Mathf.Rad2Deg < viewAngle)
            {
                return true;
            }
        }
        return false;
    }

   
    void Update()
    {
        bool seen = SeePlayer();
        if (seen)
        {
            
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            
            transform.position += transform.up * 6f * Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
                target = lastSeen + (rotation * transform.up * 5.0f);
            }
            else
            {
                Vector3 dir = target - transform.position;
                float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
                Quaternion p = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, p, Time.deltaTime * 10f);

                transform.position += transform.up * 5f * Time.deltaTime;
            }
        }

        Debug.DrawRay(transform.position, transform.up * viewDistance, seen ? Color.red : Color.yellow, 0);
        Quaternion rayAngle = Quaternion.Euler(0, 0, -viewAngle);
        Debug.DrawRay(transform.position, rayAngle * transform.up * viewAngle, seen ? Color.red : Color.yellow, 0);
        rayAngle = Quaternion.Euler(0, 0, viewAngle);
        Debug.DrawRay(transform.position, rayAngle * transform.up * viewAngle, seen ? Color.red : Color.yellow, 0);
    }
}
