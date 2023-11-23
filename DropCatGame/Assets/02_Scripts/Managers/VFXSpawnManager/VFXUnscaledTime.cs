using UnityEngine;

public class VFXUnscaledTime : MonoBehaviour
{
    private ParticleSystem ps;
    public float hSliderValue = 1.0f;
    public bool useUnscaledTime = false;

    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        var main = ps.main;
        main.useUnscaledTime = useUnscaledTime;

        Time.timeScale = hSliderValue;
    }
}