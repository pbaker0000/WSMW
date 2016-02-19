using UnityEngine;
using System.Collections;

public class PickedUp : MonoBehaviour {

    private bool update;

    public GameObject player;

	// Update is called once per frame
	void Update () {
        if (update)
        {
            if(player.transform.Find("Player Small").GetComponent<Player>().toss)
            {
                update = false;
                player.transform.Find("Player Small").GetComponent<Player>().toss = false;
            }
            Vector3 position = player.transform.Find("Player Small").position;
            transform.parent.position = new Vector3(position.x, position.y, position.z);
        }
	}

    public void EndPickUp()
    {
        update = true;
    }

    public void EndTossMe()
    {
        transform.parent.GetComponent<Koopa>().gravity = -65;
        transform.parent.GetComponent<Koopa>().maxFall = 12;
    }
}
