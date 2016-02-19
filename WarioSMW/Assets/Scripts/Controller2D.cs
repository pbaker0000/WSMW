using UnityEngine;
using System.Collections;

public abstract class Controller2D : MonoBehaviour {

    [HideInInspector]
    public bool facingRight = true;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public CollisionInfo collisions;

    public void Move(Vector2 velocity, Collider2D collider, LayerMask collisionMask)
    {
        collisions.Reset();

        if (velocity.x != 0)
        {
            PlayerRaycast2D.HorizontalCollisions(ref velocity, collider, horizontalRayCount, collisionMask, out collisions.left, out collisions.right, out collisions.collidedWith);
        }
        if (velocity.y != 0)
        {
            PlayerRaycast2D.VerticalCollisions(ref velocity, collider, verticalRayCount, collisionMask, out collisions.above, out collisions.below, out collisions.collidedWith);
        }

        transform.Translate(velocity);
    }    

    public void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public abstract void Jump();

    public abstract void Run();

    public abstract void Walk();

    public abstract void Duck();

    public abstract void StandUp();

    public abstract void TurnAround();

    public abstract void Dead();

    public abstract void PickUp();

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public Collider2D collidedWith;

        public void Reset()
        {
            collidedWith = null;
            above = below = false;
            left = right = false;
        }
    }

}
