using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float chanceToSpawn;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (Random.Range(1, 100) > chanceToSpawn)
        {
            Destroy(this.gameObject);
        }  // chance to spawn
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (player.moveSpeed >= player.moveSpeedNeededToSurvive)
            {
                player.knockback();
            }
            else
            {
                GameManager.instance.gameRestart(); 
            }
        }
    }
}
