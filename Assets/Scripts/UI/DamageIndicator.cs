using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] RawImage img;
    private float t;
    private float dt;

    private void Start()
    {
        t = 0;
        dt = 1.0f / fadeTime;

        Destroy(gameObject, fadeTime);
    }

    private void Update()
    {
        Color color = new Color(255,255, 255, Mathf.Lerp(255,0,t));
        t += dt * Time.deltaTime;
        img.color = color;
    }
}
