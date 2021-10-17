using UnityEngine;

// Handles raycasts

public static class RaycastSystem
{
    public static RaycastHit Raycast(Vector3 startPosition, Vector3 direction, float range, LayerMask mask, bool debug=false)
    {
        bool hit = Physics.Raycast(startPosition, direction, out RaycastHit raycast, range, mask);
        
        if (debug)
        {
            if (hit)
            {
                Debug.DrawRay(startPosition, direction * raycast.distance, Color.yellow);
                Debug.Log("Raycast hit " + raycast.collider.name + "!");
            }
            else
            {
                Debug.DrawRay(startPosition, direction * raycast.distance, Color.white);
                Debug.Log("Raycast did not hit!");
            }
        }

        return raycast;
    }
}