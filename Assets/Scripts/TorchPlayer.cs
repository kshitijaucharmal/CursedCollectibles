using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TorchPlayer : NetworkBehaviour {

    private Light flashLight;
    [SerializeField] private Transform torchModel;
    [SerializeField] private GameObject viewCone;
    [SerializeField] private float smoothing = 10f;

    public Vector3 offPosition = new Vector3(0.4f, -0.15f, 0.46f);
    public Vector3 onPosition = new Vector3(0f, -0.2f, 0f);

    private bool flashOn
    {
        get
        {
            return _flashOn;
        }
        set
        {
            _flashOn = value;
            flashLight.enabled = value;
            viewCone.SetActive(value);
        }

    }
    private bool _flashOn = false;

    // Start is called before the first frame update
    void Start() {
        flashLight = GetComponent<Light>();
        flashOn = false;
        viewCone.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer) return;
        if (Input.GetButton("Fire1")) {
            flashOn = true;
        }
        if (Input.GetButtonUp("Fire1")) {
            flashOn = false;
        }

        if (flashOn)    torchModel.localPosition = Vector3.Lerp(torchModel.localPosition, onPosition, smoothing * Time.deltaTime);
        else            torchModel.localPosition = Vector3.Lerp(torchModel.localPosition, offPosition, smoothing * Time.deltaTime);
    }
}
