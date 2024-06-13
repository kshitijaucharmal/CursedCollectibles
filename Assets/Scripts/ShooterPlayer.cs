using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayer : MonoBehaviour
{
    [SerializeField] private GameObject gunPrefab;

    // Start is called before the first frame update
    void Start() {
        var gun = Instantiate(gunPrefab, transform).GetComponent<GunScript>();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
