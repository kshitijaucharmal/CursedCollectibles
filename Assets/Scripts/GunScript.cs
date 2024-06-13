using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GunScript : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject impactFx;
    [SerializeField] private GameObject shootFx;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask layerMask;
    private Transform camer;

    private void Awake()
    {
        camer = Camera.main.transform;
    }

    public void Shoot()
    {
        // Raycast
        Ray ray = new Ray(camer.position, camer.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            // Spawn Impact
            var imp = Instantiate(impactFx, hit.point, Quaternion.identity);
            Destroy(imp, 1);

            if (hit.transform.CompareTag("Ghost"))
            {
                hit.transform.GetComponent<GhostScript>().Damage();
            }
        }

        // Spawn Shoot Particles
        var fx = Instantiate(shootFx, shootPoint.position, Quaternion.identity);
        Destroy(fx, 0.1f);

        // Play animation
        animator.SetTrigger("shoot");
        
    }

    // Tetsubg
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

}
