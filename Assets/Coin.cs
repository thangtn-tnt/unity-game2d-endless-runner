using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    /*  ----//Start "animate collectable objects"----
    //[SerializeField] private float rotationMultiplier;

    //// Update is called once per frame
    //void Update()
    //{
    //    transform.Rotate(new Vector3(0, 0, 199 * rotationMultiplier) * Time.deltaTime);
    //}
        ----//End "animate collectable objects"---- */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.coin++;
            Destroy(this.gameObject);
        }
    }
}
