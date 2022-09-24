using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Color nextColor;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        nextColor = UnityEngine.Random.ColorHSV();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            spriteRenderer.color = nextColor;

            //if (ColorUtility.TryParseHtmlString("#8A1E11", out nextColor))
            //{
            //    spriteRenderer.color = nextColor;
            //}
        }
    }
}
