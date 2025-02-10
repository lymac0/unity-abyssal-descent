using UnityEngine;
using UnityEngine.U2D;

public class parallax: MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float ParallaxEffect;
    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }
    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - ParallaxEffect));
        float dist = (cam.transform.position.x * ParallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
