using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCone : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
