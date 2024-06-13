using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRadarSync : MonoBehaviour {

    private Collectible[] items;
    private RadarManager radar;

    // Start is called before the first frame update
    void Start() {
        items = GameObject.FindObjectsOfType<Collectible>();
        
        radar = GameManager.FindObjectOfType<RadarManager>();
    }

    // Update is called once per frame
    void Update() {
        var (nearestItem, nearestDist) = GetNearestItemDistance();

        if(nearestItem != null)
        {
            var rate = 20 / nearestDist;
            rate = Mathf.Clamp(rate, 0.1f, 10);
            radar.SetBlinkRate(rate, nearestDist);
        }
    }

    (GameObject, float) GetNearestItemDistance() {
        float min = 1000;
        GameObject minItem = null;
        foreach(var item in items) {
            if(item == null) continue;
            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < min)
            {
                min = dist;
                minItem = item.gameObject;
            }

        }

        return (minItem, min);
    }
}
