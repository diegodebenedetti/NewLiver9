using UnityEngine;
public static class Helpers
{
    public static Vector3 ChooseRandomPositionInsideCollider(BoxCollider pCollider)
    {
        Vector3 extents = pCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range( -extents.x, extents.x ),
            Random.Range( -extents.y, extents.y ),
            Random.Range( -extents.z, extents.z )
        )  + pCollider.center;
        return pCollider.transform.TransformPoint( point );
    }
    
}