using UnityEngine;

public class DestroySound : MonoBehaviour
{
    public float delay = 180;

    private void Start()
    {
        if (delay > 0)
        {
            Destroy(gameObject, delay);
        }
    }
}
