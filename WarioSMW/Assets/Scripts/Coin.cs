using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
