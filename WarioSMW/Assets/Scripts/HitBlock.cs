using UnityEngine;
using System.Collections;

public class HitBlock : MonoBehaviour {

    private Animator blockAnim;
    private Animator anim;
    public GameObject item;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        blockAnim = transform.parent.Find("Block").GetComponent<Animator>();
    }	    

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform.tag == "Player")
        {
            blockAnim.SetBool("Hit", true);
            anim.SetBool("Hit", true);
        }
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    public void CreateItem()
    {
        Vector3 aboveBlock = new Vector3(transform.parent.position.x, transform.parent.position.y + 1, transform.parent.position.z);
        Instantiate(item, aboveBlock, Quaternion.identity);
    }
}
