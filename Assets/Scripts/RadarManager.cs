using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadarManager : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private TMP_Text disttext;

    [Header("Color Settings")]
    [SerializeField] private Color blinkColor = Color.black;
    [SerializeField] private Color normalColor = Color.white;

    // Slow
    private float blinkFreq = 0.1f;

    private float time;

    public void SetBlinkRate(float b, float dist)
    {
        blinkFreq = b;
        disttext.text = dist.ToString();
    }

    // Start is called before the first frame update
    void Start() {
        image.color = normalColor;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float t = (Mathf.Sin(time * blinkFreq * 2f * Mathf.PI) + 1) / 2;
        image.color = Color.Lerp(normalColor, blinkColor, t);
    }
}
