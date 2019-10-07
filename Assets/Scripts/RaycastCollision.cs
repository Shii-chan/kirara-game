using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class RaycastCollision : MonoBehaviour
{
   	public const float skin = 0.02f;
    public BoxCollider2D bcollider;
	public RaycastOrigins origins;
	public float horRaySpacing;
	public float vertRaySpacing;
   	public LayerMask collisionMask;
	public CollisionInfo collisions;
    public float vertRayCount, horiRayCount = 3;

	public virtual void Start() {
		bcollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
		collisions.faceDir = (int)Mathf.Sign(bcollider.transform.localScale.x);

	}
    public void UpdateRaycastOrigins() {
		Bounds bounds = bcollider.bounds;
		bounds.Expand(skin * -2);
		origins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		origins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		origins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		origins.topRight = new Vector2(bounds.max.x, bounds.max.y);

	}

	public struct CollisionInfo {
		public bool above, below, left, right;
		public bool ascSlope, descSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 oldVelocityl;
		public int faceDir;
		public bool fallThrough;
		public void Reset(){
			above = below = left = right = ascSlope = descSlope = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

	public void CalculateRaySpacing(){
		Bounds bounds = bcollider.bounds;
		bounds.Expand(skin * -2);

		horRaySpacing = bounds.size.y / (horiRayCount -1 );
		vertRaySpacing = bounds.size.x / (vertRayCount -1);

	}
	public struct RaycastOrigins {
	    public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }
}
