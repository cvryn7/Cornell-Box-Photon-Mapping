/**
 * Author : Karan Bhagat
 * Global Illumination Final Project
 * */

using UnityEngine;
using System.Collections;

/**
 * Main cornell box rendering program
 * */
public class PhotonMappingStart : MonoBehaviour
{

	//Texture to draw on
	private Texture2D resutTex;
	public PhotonMappingUtils variableUtils;
	
	int imageSize = 512;
	
	Color[] colorData;

	float ambienceValue = 0.1f;
	public static float[] lightSource = { 0.0f, 1.2f, 3.75f };
	
	public Color black = new Color (0, 0, 0.1f, 1);
	public static bool[] metalObject = { false, false, false, false };

	public static Objects objects = new Objects ();

	public static Photon photonMap = new Photon ();

	bool UseMapping = true;
	float squareRadius = 0.7f;
	float exposure = 50.0f;

	bool noData = true;

	int pointOnX, pointOnY, interateNumberOnPoint, maximumPosition;

	//Initiate all data
	void Start ()
	{
		
		objects.addSphere (1.0f, 0.0f, 4.0f, 0.5f);
		objects.addSphere (-0.6f, -1.0f, 4.5f, 0.5f);
		objects.addSphere (-1.0f, 0.0f, 4.0f, 0.5f);
		objects.addSphere (0.0f, 0.0f, 4.0f, 0.5f);
		objects.addPlane (0, 1.5f);
		objects.addPlane (1, -1.5f);
		objects.addPlane (0, -1.5f);
		objects.addPlane (1, 1.5f);
		objects.addPlane (2, 5.0f);
		
		init ();
	}

	public void init ()
	{
		float[] noDataPoint = new float[3];
		noDataPoint [0] = noDataPoint [1] = noDataPoint [2] = 0.0f;
		variableUtils = new PhotonMappingUtils (false, 0, 0, -1.0f, -1.0f, noDataPoint);

		photonMap.initializePhotonData ();

		colorData = new Color[imageSize * imageSize];
	
		resutTex = new Texture2D (imageSize, imageSize);
		GetComponent<Renderer> ().material.mainTexture = resutTex;
		reset ();
	}

	void Update ()
	{
		setData ();
		resutTex.SetPixels (colorData);
		resutTex.Apply ();
	}

	public void reset ()
	{
		cleanData ();
		cleanresutTex ();
	}

	void cleanData ()
	{
		pointOnX = 0; 
		pointOnY = 0; 
		interateNumberOnPoint = 1;
		maximumPosition = 2;
		noData = true;
		executePhotonEmitters ();
	}

	void cleanresutTex ()
	{
		for (int x = 0; x < imageSize; x++) {
			for (int y = 0; y < imageSize; y++) {
				resutTex.SetPixel (x, y, black);
			}
		}
		resutTex.Apply ();
	}


	//Function to find sphere intersection
	void intersectSphere (int index, float[] ray, float[] origin)
	{ 
		float[] tempSphere = substractXYZ (objects.getSphereObject (index), origin); 
		float radius = objects.getSphereData (index, 3);
		float A = dotProduct (ray, ray); 
		float B = -2.0f * dotProduct (tempSphere, ray);
		float C = dotProduct (tempSphere, tempSphere) - (radius * radius);
		float D = B * B - 4 * A * C;
	  
		if (D > 0.0) { 
			float sign = (C < -0.00001) ? 1 : -1; 
			float distance = (-B + sign * Mathf.Sqrt (D)) / (2 * A);
			checkDistance (distance, 0, index);
		}  
	}

	//Function to find plane intersection
	void intersectPlane (int index, float[] ray, float[] origin)
	{ 
		int y = (int)objects.getPlaneData (index, 0);
		if (ray [y] != 0.0) {
			float distance = (objects.getPlaneData (index, 1) - origin [y]) / ray [y];
			checkDistance (distance, 1, index);
		}
	}

	//find object intersection
	void intersectObject (int objectType, int index, float[] ray, float[] origin)
	{
		if (objectType == 0)
			intersectSphere (index, ray, origin);
		else
			intersectPlane (index, ray, origin);
	}
		
	void checkDistance (float distance, int objectType, int index)
	{
		if (distance < variableUtils.getDistance () && distance > 0.0) {
			variableUtils.setType (objectType);
			variableUtils.setIndex (index);
			variableUtils.setDistance (distance);
			variableUtils.setIntersect (true);
		}
	}

	//Throw photon ray and find intersection
	void raytrace (float[] ray, float[] origin)
	{
		variableUtils.setIntersect (false);
		variableUtils.setDistance (5.9f);
	  
		for (int t = 0; t < objects.getNumberOfObjectTypes (); t++)
			for (int i = 0; i < objects.getObjectsPerType (t); i++)
				intersectObject (t, i, ray, origin);
	}

	//Method to generate find color array by tracing ray to each coordinate of world
	//then find the photon on the point to get final color
	float[] generateRender (float posX, float posY)
	{
		float[] colorValue = { 0.0f, 0.0f, 0.0f };
		float[] ray = {  posX / imageSize - 0.5f, 
			-(posY / imageSize - 0.5f), 1.0f
		};
		raytrace (ray, variableUtils.getWorldOrigin ()); 

		if (variableUtils.getIntersect ()) { 
			variableUtils.setPoint (multiplyThree (ray, variableUtils.getDistance ()));
			
			if (variableUtils.getType () == 0 && variableUtils.getIndex () == 1 && metalObject [0]) { 
				ray = bounceOnNormal (ray, variableUtils.getWorldOrigin ());
				raytrace (ray, variableUtils.getPoint ()); 
				if (variableUtils.getIntersect ()) { 
					variableUtils.setPoint (additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), variableUtils.getPoint ()));
				}
			}

			if (variableUtils.getType () == 0 && variableUtils.getIndex () == 0 && metalObject [1]) {   
				ray = bounceOnNormal (ray, variableUtils.getWorldOrigin ()); 
				raytrace (ray, variableUtils.getPoint ()); 
				if (variableUtils.getIntersect ()) { 
					variableUtils.setPoint (additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), variableUtils.getPoint ()));
				}
			} 

			if (variableUtils.getType () == 0 && variableUtils.getIndex () == 2 && metalObject [2] && objects.getObjectsPerType (0) >= 3) {    
				ray = bounceOnNormal (ray, variableUtils.getWorldOrigin ()); 
				raytrace (ray, variableUtils.getPoint ()); 
				if (variableUtils.getIntersect ()) { 
					variableUtils.setPoint (additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), variableUtils.getPoint ()));
				}
			}
			if (variableUtils.getType () == 0 && variableUtils.getIndex () == 4 && metalObject [3] && objects.getObjectsPerType (0) >= 4) {      
				ray = bounceOnNormal (ray, variableUtils.getWorldOrigin ());  
				raytrace (ray, variableUtils.getPoint ());  
				if (variableUtils.getIntersect ()) { 
					variableUtils.setPoint (additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), variableUtils.getPoint ()));
				}
			}

			colorValue = collectPhotonsFromBounces (variableUtils.getPoint (), variableUtils.getType (), variableUtils.getIndex ());
		}
		return colorValue;
	}

	//Reflect the photon
	float[] bounceOnNormal (float[] ray, float[] fromPoint)
	{  
		float[] N = surfaceNormal (variableUtils.getType (), variableUtils.getIndex (), variableUtils.getPoint (), fromPoint);
		return normalize3 (substractXYZ (ray, multiplyThree (N, (2 * dotProduct (ray, N)))));
	}

	float[] collectPhotonsFromBounces (float[] p, int type, int id)
	{
		float[] color = { 0.0f, 0.0f, 0.0f };  
		float[] N = surfaceNormal (type, id, p, variableUtils.getWorldOrigin ());                 
		for (int i = 0; i < photonMap.photonsPerObject [type] [id]; i++) {
		  
			if (findSqureDistance (p, photonMap.getPhotonData (type, id, i, 0), squareRadius)) {
				float weight = Mathf.Max (0.0f, -dotProduct (N, photonMap.getPhotonData (type, id, i, 1))); 
				weight *= (1.0f - Mathf.Sqrt (variableUtils.getSqDistance ())) / exposure;
				color = additionXYZ (color, multiplyThree (photonMap.getPhotonData (type, id, i, 2), weight));
			}
		} 
		return color;
	}

	//Photon emitter engine
	void executePhotonEmitters ()
	{                       
		Random.seed = 0;
		for (int t = 0; t < objects.getNumberOfObjectTypes (); t++)
			for (int i = 0; i < objects.getObjectsPerType (t); i++)
				photonMap.photonsPerObject [t] [i] = 0;
		
		float tempX;
		float tempY;
		int xPoint;
		int yPoint;
		float maxX = -100f;
		float minX = 100f;
		float maxY = -100f;
		float minY = 100;
		for (int i = 0; i < photonMap.numberOfPhotons; i++) { 
			int bounces = 1;
			float[] color = { 1.0f, 1.0f, 1.0f };
			float[] ray = normalize3 (randomThree (1.0f)); 
			float[] oldPoint = lightSource; 
            
			while (oldPoint [1] >= lightSource [1]) {
				oldPoint = additionXYZ (lightSource, multiplyThree (normalize3 (randomThree (1.0f)), 0.75f));
			}
			if (abs (oldPoint [0]) > 1.5 || abs (oldPoint [1]) > 1.2f ||
			    findSqureDistance (oldPoint, objects.getSphereObject (0), objects.getSphereData (0, 3) * objects.getSphereData (0, 3)))
				bounces = photonMap.getNumberOfBounces () + 1;
		
			raytrace (ray, oldPoint); 
		
			while (variableUtils.getIntersect () && bounces <= photonMap.getNumberOfBounces ()) {
				variableUtils.setPoint (additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), oldPoint));
				color = multiplyThree (recieveColorFromObject (color, variableUtils.getType (), variableUtils.getIndex ()), 1.0f / Mathf.Sqrt (bounces));
 				savePhotonHitData (variableUtils.getType (), variableUtils.getIndex (), variableUtils.getPoint (), ray, color); 
  
				mapShadowPhoton (ray);
				ray = bounceOnNormal (ray, oldPoint); 
				raytrace (ray, variableUtils.getPoint ()); 
				oldPoint = variableUtils.getPoint ();
           
				bounces++;
			}
		}

	}

	//Save photon data in photon map
	void savePhotonHitData (int type, int id, float[] location, float[] direction, float[] energy)
	{
		photonMap.setPhotonData (type, id, photonMap.photonsPerObject [type] [id], 0, location);
		photonMap.setPhotonData (type, id, photonMap.photonsPerObject [type] [id], 1, direction);
		photonMap.setPhotonData (type, id, photonMap.photonsPerObject [type] [id], 2, energy);
		photonMap.photonsPerObject [type] [id]++;
	}

	//Save shadow data
	void mapShadowPhoton (float[] ray)
	{
		float[] shadowData = { -0.25f, -0.25f, -0.25f };
		float[] tempPoint = variableUtils.getPoint (); 
		int tempType = variableUtils.getType ();
		int tempIndex = variableUtils.getIndex (); 
		float[] mappedPoint = additionXYZ (variableUtils.getPoint (), multiplyThree (ray, 0.00001f));
		raytrace (ray, mappedPoint);
		float[] shadowPoint = additionXYZ (multiplyThree (ray, variableUtils.getDistance ()), mappedPoint);
		savePhotonHitData (variableUtils.getType (), variableUtils.getIndex (), shadowPoint, ray, shadowData);
		variableUtils.setPoint (tempPoint); 
		variableUtils.setType (tempType); 
		variableUtils.setIndex (tempIndex);
	}

	//color filtration
	float[] processColorData (float[] clrData, float red, float green, float blue)
	{
		float[] clrDataOutput = { red, green, blue };
		for (int x = 0; x < 3; x++)
			clrDataOutput [x] = Mathf.Min (clrDataOutput [x], clrData [x]);
		return clrDataOutput;
	}

	//Get color of the hit object
	float[] recieveColorFromObject (float[] clrData, int objectType, int objectIndex)
	{
		if (objectType == 1 && objectIndex == 0) {
			return processColorData (clrData, 0.0f, 1.0f, 0.0f);
		} else if (objectType == 1 && objectIndex == 2) {
			return processColorData (clrData, 1.0f, 0.0f, 0.0f);
		} else if (objectType == 1 && objectIndex == 3) {
			return processColorData (clrData, 0.74f, 0.466f, 0.913f);
		} else if (objectType == 1 && objectIndex == 4) {
			return processColorData (clrData, 0.866f, 0.905f, 0.462f);
		} else if (objectType == 1 && objectIndex == 1) {
			return processColorData (clrData, 0.294f, 0.686f, 0.710f);
		} else {
			return processColorData (clrData, 1.0f, 1.0f, 1.0f);
		}
	}

	//generates the final render int passes
	void setData ()
	{ 
		if (noData) {
     
			int i, j, count = 0;
			float[] clr = { 0.0f, 0.0f, 0.0f };

			while (count < Mathf.Max (maximumPosition, 256)) {
				if (pointOnY >= maximumPosition) {
					pointOnX++;
					pointOnY = 0; 
					if (pointOnX >= maximumPosition) {
						interateNumberOnPoint++;
						pointOnX = 0;
						maximumPosition = (int)pow (2, interateNumberOnPoint);
					}
				}
				bool required = (interateNumberOnPoint == 1 || odd (pointOnX) || (!odd (pointOnX) && odd (pointOnY)));
				i = pointOnY * (imageSize / maximumPosition);
				j = pointOnX * (imageSize / maximumPosition);
				pointOnY++;

				if (required) {
					count++;
					clr = generateRender (i, j); 

					colorData [j * imageSize + i] = new Color (clr [0], clr [1], clr [2], 1);
				}
			}
			if (pointOnX == imageSize - 1) {
				noData = false;
			}
		}
		if (!noData) {
			resutTex.SetPixels (colorData);
			resutTex.Apply ();
		}
	}


	/**
	 *  UTILITY FUNCTIONS
	 * */
	float abs (float value)
	{
		return Mathf.Abs (value);
	}


	float pow (float value, float power)
	{
		return Mathf.Pow (value, power);
	}

	bool odd (int value)
	{
		return value % 2 != 0;
	}

	float[] normalize3 (float[] v)
	{ 
		Vector3 tmp = new Vector3 (v [0], v [1], v [2]).normalized;
		v [0] = tmp.x;
		v [1] = tmp.y;
		v [2] = tmp.z;
		return v;

	}

	float[] substractXYZ (float[] a, float[] b)
	{ 
		float[] result = { a [0] - b [0], a [1] - b [1], a [2] - b [2] };
		return result;
	}

	float[] additionXYZ (float[] a, float[] b)
	{
		float[] result = { a [0] + b [0], a [1] + b [1], a [2] + b [2] };
		return result;
	}

	float[] multiplyThree (float[] a, float c)
	{ 
		float[] result = { c * a [0], c * a [1], c * a [2] };
		return result;
	}

	float dotProduct (float[] a, float[] b)
	{
		return a [0] * b [0] + a [1] * b [1] + a [2] * b [2];
	}

	float[] randomThree (float s)
	{
		float[] rand = { Random.Range (-s, s), Random.Range (-s, s), Random.Range (-s, s) };
		return rand;
	}

	bool findSqureDistance (float[] a, float[] b, float sqradius)
	{
		float c = a [0] - b [0]; 
		float d = c * c;   
		if (d > sqradius)
			return false;
		c = a [1] - b [1]; 
		d += c * c;
		if (d > sqradius)
			return false;
		c = a [2] - b [2];
		d += c * c;
		if (d > sqradius)
			return false; 
		variableUtils.setSqDistance (d);
		return true; 
	}

	float lightDiffuse (float[] N, float[] P)
	{
		float[] L = normalize3 (substractXYZ (lightSource, P));
		return dotProduct (N, L);
	}

	float[] sphereNormal (int idx, float[] P)
	{
		return normalize3 (substractXYZ (P, objects.getSphereObject (idx)));
	}

	float[] planeNormal (int idx, float[] P, float[] O)
	{
		int axis = (int)objects.getPlaneData (idx, 0);
		float[] N = { 0.0f, 0.0f, 0.0f };
		N [axis] = O [axis] - objects.getPlaneData (idx, 1); 
		return normalize3 (N);
	}

	float[] surfaceNormal (int type, int index, float[] P, float[] Inside)
	{
		if (type == 0) {
			return sphereNormal (index, P);
		} else {
			return planeNormal (index, P, Inside);
		}
	}

	float lightObject (int type, int idx, float[] P, float lightAmbient)
	{
		float i = lightDiffuse (surfaceNormal (type, idx, P, lightSource), P);
		return  Mathf.Min (1.0f, Mathf.Max (i, lightAmbient));
	}
}