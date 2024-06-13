using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private float focusTime = 1f;

    [Header("Hover Settings")]
    [SerializeField] private float hoverSmoothing = 10;
    [SerializeField] private float hoverAmplitude = 0.5f;
    [SerializeField] private float hoverFreq = 5;

    private MeshRenderer mr;
    private float timeCtr;
    private bool countDown = false;

    private void Start() {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;

        var mf = GetComponent<MeshFilter>();
        mf.mesh = meshes[Random.Range(0, meshes.Length)];
    }

    private void Update()
    {

        if (countDown)
        {
            timeCtr += Time.deltaTime;
            if(timeCtr > focusTime)
            {
                mr.enabled = true;
            }
        }

        var pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, pos.y + hoverAmplitude * Mathf.Sin(hoverFreq * Time.time), hoverSmoothing * Time.deltaTime);
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Viewer"))
        {
            countDown = true;
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<M_FPSPlayerMovement>().CmdCollectItem();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Viewer"))
        {
            countDown = false;
        }
    }

}
