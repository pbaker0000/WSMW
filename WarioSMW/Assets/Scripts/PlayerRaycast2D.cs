using UnityEngine;
using System.Collections;

public static class PlayerRaycast2D {

    private static float skinWidth = .015f;

    private static RaycastOrigins raycastOrigins;


    public static void HorizontalCollisions(ref Vector2 velocity, Collider2D collider, int rayCount, LayerMask collisionMask, out bool collidedLeft, out bool collidedRight, out Collider2D collidedWith)
    {
        collidedLeft = false;
        collidedRight = false;
        collidedWith = null;

        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        float raySpacing = UpdateRaycastOriginsAndCalculateSpacing(collider, rayCount, "Horizontal");

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collidedLeft = directionX == -1;
                collidedRight = directionX == 1;
                collidedWith = hit.collider;
            }
        }
    }

    public static void VerticalCollisions(ref Vector2 velocity, Collider2D collider, int rayCount, LayerMask collisionMask, out bool collidedAbove, out bool collidedBelow, out Collider2D collidedWith)
    {
        collidedAbove = false;
        collidedBelow = false;
        collidedWith = null;

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        float raySpacing = UpdateRaycastOriginsAndCalculateSpacing(collider, rayCount, "Vertical");


        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collidedBelow = directionY == -1;
                collidedAbove = directionY == 1;
                collidedWith = hit.collider;
            }
        }
    }

    private static float UpdateRaycastOriginsAndCalculateSpacing(Collider2D collider, int rayCount, string direction)
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

        return (direction == "Vertical") ? bounds.size.x / (rayCount - 1) : bounds.size.y / (rayCount - 1);       
    }

    

    private struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
