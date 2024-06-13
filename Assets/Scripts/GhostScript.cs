using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    private Animator animator;

    private int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        var p = GameObject.FindObjectOfType<M_FPSPlayerMovement>();
        if(p != null) player = p.transform;
    }

    public void Damage()
    {
        health -= Random.Range(10, 20);
        // Play hit animation
        animator.SetTrigger("hit");
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if(Vector3.Distance(player.transform.position, transform.position) < 10) { 
                agent.destination = player.position;
            }
        }
        else
        {
            var p = GameObject.FindObjectOfType<M_FPSPlayerMovement>();
            if (p != null) player = p.transform;
        }
    }
}
