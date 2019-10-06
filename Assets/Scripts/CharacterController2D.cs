using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : RaycastCollision
{
	float maxClimbAngle = 65f;
	float maxDescendAngle = 60f;

	public void Move(Vector3 velocity, bool standingOn){
		UpdateRaycastOrigins();
		collisions.Reset();
		collisions.oldVelocityl = velocity;
		if (velocity.y < 0) DescendSlope(ref velocity);
		if (velocity.x != 0) HorisontalCollisions(ref velocity);
		if (velocity.y != 0) VerticalCollisions(ref velocity);
		transform.Translate(velocity);

		if (standingOn){
			collisions.below = true;
		}
	}

	void VerticalCollisions(ref Vector3 velocity){
		float dirY = Mathf.Sign(velocity.y);
		float rayLenght = Mathf.Abs(velocity.y)+skin;
		Vector2 rayOrigin;
		

		for (int i = 0; i < vertRayCount; i++){
			if (dirY == -1) rayOrigin = origins.bottomLeft;
			else rayOrigin = origins.topLeft;
			rayOrigin +=Vector2.right * (vertRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLenght, collisionMask);
			

			if (hit) {
				velocity.y = (hit.distance - skin) * dirY;
				rayLenght = hit.distance;

				if (collisions.ascSlope){
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.above = dirY == 1;
				collisions.below = dirY == -1;
			}

			Debug.DrawRay(rayOrigin, Vector2.up*dirY*rayLenght, Color.red);
		}
		if (collisions.ascSlope) {
			float dirX = Mathf.Sign(velocity.x);
			rayLenght = Mathf.Abs(velocity.x) + skin;
			if (dirX == -1) rayOrigin = origins.bottomLeft + Vector2.up * velocity.y;
			else rayOrigin = origins.bottomRight + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right*dirX, rayLenght, collisionMask);
			if (hit){
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle){
					velocity.x = (hit.distance - skin) * dirX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void HorisontalCollisions(ref Vector3 velocity){
		float dirX = Mathf.Sign(velocity.x);
		float rayLenght = Mathf.Abs(velocity.x)+skin;
		Vector2 rayOrigin;
		

		for (int i = 0; i < horiRayCount; i++){
			if (dirX == -1) rayOrigin = origins.bottomLeft;
			else rayOrigin = origins.bottomRight;
			rayOrigin +=Vector2.up * (horRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLenght, collisionMask);
			

			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descSlope){
						collisions.descSlope = false;
						velocity = collisions.oldVelocityl;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle <= collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skin;
						velocity.x -= distanceToSlopeStart * dirX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * dirX;
				}
				if (!collisions.ascSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skin) * dirX;
					rayLenght = hit.distance;

					if (collisions.ascSlope){
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}
					collisions.left = dirX == -1;
					collisions.right = dirX == 1;
				}
			}
			Debug.DrawRay(rayOrigin, Vector2.right*dirX*rayLenght, Color.red);
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle){
		float moveDistance = Mathf.Abs(velocity.x);
		float climbVelocityY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad) * moveDistance;
		if (velocity.y <= climbVelocityY){
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisions.below = true;
			collisions.ascSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity){
		float dirX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin;
		if (dirX == -1) rayOrigin = origins.bottomRight;
		else rayOrigin = origins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, bcollider.bounds.size.y, collisionMask);
		if (hit){
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle){
				if (Mathf.Sign(hit.normal.x) == dirX){
					if (hit.distance - skin <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)){
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;
						collisions.slopeAngle = slopeAngle;
						collisions.descSlope = true;
						collisions.below = true;
					}
				}
			}
		}

	}



}