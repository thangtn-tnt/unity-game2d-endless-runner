using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] private Transform[] movePoints;
    [SerializeField] private int nextPosition;
    [SerializeField] private float trapSpeed;

    [SerializeField] private float rotationMultiplier;
    [SerializeField] private float chanceToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(1,100) > chanceToSpawn)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoints[nextPosition].position, trapSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoints[nextPosition].position) < 0.5f)
        {
            nextPosition = Random.Range(0, movePoints.Length);
            if (nextPosition >= movePoints.Length)
            {
                nextPosition = 0;
            }
        }
        if (transform.position.x > movePoints[nextPosition].position.x)
        {
            transform.Rotate(new Vector3(0, 0, 100 * rotationMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 100 * -rotationMultiplier) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
