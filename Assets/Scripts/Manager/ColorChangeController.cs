using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Color nextColor;

    private BoxCollider2D boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        //change size of this box collider according to sprite size of the parent

        boxCollider.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y + 0.001f);
        boxCollider.offset = Vector2.zero;
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
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                Transform child = transform.parent.GetChild(i);

                if (child.tag == "PlatformHeader")
                {
                    SpriteRenderer newSr = child.GetComponent<SpriteRenderer>();
                    newSr.color = nextColor;
                }
            }          
        }
    }
}
