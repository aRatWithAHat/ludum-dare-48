using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityUtils 
{
    public static float GetAngleBetweenPositions( Vector2 origin, Vector2 target ){
        Vector2 direction = GetDirectionBetweenPosition( origin, target );
        return Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg;
    }

    public static Vector2 GetDirectionBetweenPosition( Vector2 origin, Vector2 target, bool isNormalized = false){
        Vector2 direction = target - origin;
        return isNormalized ? direction.normalized : direction;
    }
}
