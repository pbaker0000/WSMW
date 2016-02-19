using UnityEngine;
using System.Collections;

public static class MultiRayCast2D
{
    public static bool Up(Collider2D collider, float rayLength, int rayCount, float skinWidth, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, skinWidth, Vector2.up) == 1) ? true : false;
    }

    public static bool Down(Collider2D collider, float rayLength, int rayCount, float skinWidth, LayerMask collisionMask)
    {
        return( RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, skinWidth, Vector2.down) == -1) ? true : false;
    }

    public static bool Left(Collider2D collider, float rayLength, int rayCount, float skinWidth, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, skinWidth, Vector2.left) == -1) ? true : false;
    }

    public static bool Right(Collider2D collider, float rayLength, int rayCount, float skinWidth, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, skinWidth, Vector2.right) == 1) ? true : false;
    }

    public static bool Up(Collider2D collider, float rayLength, int rayCount, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, 0, Vector2.up) == 1) ? true : false;
    }

    public static bool Down(Collider2D collider, float rayLength, int rayCount, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, 0, Vector2.down) == -1) ? true : false;
    }

    public static bool Left(Collider2D collider, float rayLength, int rayCount, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, 0, Vector2.left) == -1) ? true : false;
    }

    public static bool Right(Collider2D collider, float rayLength, int rayCount, LayerMask collisionMask)
    {
        return (RaycastAnyDirection(collider, rayLength, rayCount, collisionMask, 0, Vector2.right) == 1) ? true : false;
    }

    private static int RaycastAnyDirection(Collider2D collider, float rayLength, int rayCount, LayerMask collisionMask, float skinWidth, Vector2 direction)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -4);

        Vector2 raySpacing = (direction == Vector2.up || direction == Vector2.down) ? Vector2.right*bounds.size.x / (rayCount - 1) : Vector2.up*bounds.size.y / (rayCount - 1);

        int connected = 0;

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = Vector2.zero;
            if (direction == Vector2.up)
                rayOrigin = new Vector2(collider.bounds.min.x + skinWidth, collider.bounds.max.y);
            else if (direction == Vector2.down)
                rayOrigin = new Vector2(collider.bounds.min.x + skinWidth, collider.bounds.min.y);
            else if (direction == Vector2.left)
                rayOrigin = new Vector2(collider.bounds.min.x, collider.bounds.min.y + skinWidth);
            else if (direction == Vector2.right)
                rayOrigin = new Vector2(collider.bounds.max.x, collider.bounds.min.y + skinWidth);

            rayOrigin += (raySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, direction * rayLength, Color.red);
            if (hit)
            {
                connected = (direction == Vector2.up) || (direction == Vector2.right) ? 1 : -1;                
            }
        }
        return connected;
    }
}
