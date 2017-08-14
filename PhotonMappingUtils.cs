using System;


public class PhotonMappingUtils
{
	// ----- Raytracing Globals -----
	bool objectIntersect = false;       //For Latest Raytracing Call... Was Anything Intersected by the Ray?
	int objectType;                        //... Type of the Intersected Object (Sphere or Plane)
	int objectIdx;                       //... Index of the Intersected Object (Which Sphere/Plane Was It?)
	float objSquareDistance;
	float objDistance = -1.0f;      //... Distance from Ray Origin to Intersection
	float[] intersectPoint = {0.0f, 0.0f, 0.0f}; //... Point At Which the Ray Intersected the Object
	float[] worldOrigin = {0.0f,0.0f,0.0f};

	public PhotonMappingUtils (bool interset, int type, int index, float sqDistance, float distance, float[] point)
	{
		setIntersect (interset);
		setType (type);
		setIndex (index);
		setSqDistance (sqDistance);
		setDistance (distance);
		setPoint (point);
	}

	public void setIntersect(bool intersect) {
		objectIntersect = intersect;
	}

	public bool getIntersect() {
		return objectIntersect;
	}

	public void setType(int type) {
		objectType = type;
	}

	public int getType() {
		return objectType;
	}

	public void setIndex(int index) {
		objectIdx = index;
	}

	public int getIndex(){
		return objectIdx;
	}

	public void setSqDistance(float sqDistance) {
		objSquareDistance = sqDistance;
	}

	public float getSqDistance() {
		return objSquareDistance;
	}

	public void setDistance(float distance) {
		objDistance = distance;
	}

	public float getDistance() {
		return objDistance;
	}

	public void setPoint(float[] point) {
		intersectPoint = point;
	}

	public float[] getPoint() {
		return intersectPoint;
	}

	public void setWorldOrigin(float[] o) {
		worldOrigin = o;
	}

	public float[] getWorldOrigin() {
		return worldOrigin;
	}
}


