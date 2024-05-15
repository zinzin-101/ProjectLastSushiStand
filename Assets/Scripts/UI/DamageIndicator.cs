using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] RawImage img;
    private float alpha;

    private void Start()
    {
        alpha = 255;
        Destroy(gameObject, fadeTime);
    }

    private void Update()
    {
        Color color = new Color(255,255, 255, Mathf.MoveTowards(alpha,0, fadeTime * Time.deltaTime));
        img.color = color;
    }
}
