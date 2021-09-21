using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    public Transform EnemyGFX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f); //repeat method updatepath every 0.5 seconds
    }

    //Method that finds the best path between the enemy and player
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }

    //reset current waypoint when destination was reached
    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

        rb2d.AddForce(force);

        //check if the distance from enemy to player is smaller than the next waypoint if so add a waypoint
        if (distance < nextWayPointDistance)
        {
            currentWaypoint++;
        }

        //flip enemyGFX when target goes behind you
        if (rb2d.velocity.x >= 0.01f)
        {
            EnemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb2d.velocity.x <= 0.01f)
        {
            EnemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }

        animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
    }
}
