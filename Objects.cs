using System;

public class Objects
{
	float[][] sphereObject = new float[4][];
	float[][] planeObject = new float[5][];
	int numberOfSpheres = 0;
	int numberOfPlanes = 0;
	int numberOfObjectTypes = 2;
	public int[] objectsPerType = new int[2];
	public Objects ()
	{
		objectsPerType [0] = 2;
		objectsPerType [1] = 5;
	}

	public int getNumberOfObjectTypes() {
		return numberOfObjectTypes;
	}

	public int getObjectsPerType(int type) {
		return objectsPerType [type];
	}

	public void incrementObjectsPerType(int type) {
		objectsPerType [type]++;
	}

	public void addSphere(float x, float y, float z, float radius) {
		float[] newData = new float[4];
		newData [0] = x;
		newData [1] = y;
		newData [2] = z;
		newData [3] = radius;
		sphereObject[numberOfSpheres] = newData;
		numberOfSpheres++;
	}

	public void addPlane(float axis, float size) {
		float[] newData = new float[2];
		newData [0] = axis;
		newData [1] = size;

		planeObject [numberOfPlanes] = newData;
		numberOfPlanes++;
	}

	public float getSphereData(int i, int j) {
		return sphereObject [i] [j];
	}

	public void setSphereData(int i, int j, float data) {
		sphereObject [i] [j] = data;
	}

	public float[] getSphereObject(int i) {
		return sphereObject [i];
	}

	public float getPlaneData(int i, int j) {
		return planeObject [i] [j];
	}

	public float[] getPlaneObject(int i) {
		return planeObject [i];
	}
}


