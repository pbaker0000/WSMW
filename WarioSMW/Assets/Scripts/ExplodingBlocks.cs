using UnityEngine;
using System.Collections;

public class ExplodingBlocks : MonoBehaviour {

    private Collider2D collider;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //void OnCollisionStay2D(Collision2D col)
    //{
    //    if(col.gameObject.tag == "Player")
    //    {

    //    }
    //}

    public void BeginExplode()
    {
        gameObject.SetActive(false);
    }
}
