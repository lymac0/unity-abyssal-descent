using UnityEngine;
using UnityEngine.U2D;

public class parallax: MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float ParallaxEffect;
    private float startY; // Baþlangýç Y pozisyonu

    private void Start()
    {
        startpos = transform.position.x;
        startY = transform.position.y; // Yükseklik baþlangýcý
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float tempX = (cam.transform.position.x * (1 - ParallaxEffect));
        float distX = (cam.transform.position.x * ParallaxEffect);

        float tempY = (cam.transform.position.y * (1 - ParallaxEffect * 0.5f)); // Y ekseni için parallax
        float distY = (cam.transform.position.y * (ParallaxEffect * 0.5f));

        transform.position = new Vector3(startpos + distX, startY + distY, transform.position.z);

        if (tempX > startpos + length) startpos += length;
        else if (tempX < startpos - length) startpos -= length;
    }
}
