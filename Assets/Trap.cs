using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float chanceToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(1, 100) > chanceToSpawn)
        {
            Destroy(this.gameObject);
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
