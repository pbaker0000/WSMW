using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float moveSpeed = 2;
    public float top = 4.5f;
    public float bottom = -1.5f;

    private float speed;
    
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        speed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        rb2d.velocity = new Vector2(0, speed);

        if (transform.localPosition.y > top)
        {
            MoveDown();
        }
        else if (transform.localPosition.y < bottom)
        {
            MoveUp();
        }
	}

    void MoveUp()
    {
        speed = moveSpeed;
    }

    void MoveDown()
    {
        speed = -moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.transform.tag == "Player")
        {
            Destroy(gameObject);
            col.gameObject.transform.parent.parent.GetComponent<Player>().Win();
        }
    }
}
