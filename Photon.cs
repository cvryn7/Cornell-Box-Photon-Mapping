/**
 * Author : Karan Bhagat
 * Global Illumination Final Project
 * */

using System;

/**
 * Class defining photon map
 * */
public class Photon
{
	public int numberOfPhotons = 3000;
	public int numberOfBounces = 3;
	public int[][] photonsPerObject = new int[2][];
	public float[][][][][] photonData = new float[5][][][][];
	public Photon ()
	{
		int[] object1 = new int[5];
		object1 [0] = object1 [1] = object1 [2] = object1 [3] = object1 [4] = 0;
		int[] object2 = new int[5];
		object2 [0] = object2 [1] = object2 [2] = object2 [3] = object2 [4] = 0;
		photonsPerObject [0] = object1;
		photonsPerObject [1] = object2;
		initializePhotonData ();
	}


	public void initializePhotonData() {
		int size = photonData.Length;
		int sizeA = 5;
		int sizeB = 5000;
		int sizeC = 3;
		int sizeD = 3;
		for (int a = 0; a < size; ++a)
		{
			photonData[a] = new float[sizeA][][][];
			for (int b = 0; b < sizeA; ++b)
			{
				photonData[a][b] = new float[sizeB][][];
				for (int c = 0; c < sizeB; ++c)
				{
					photonData[a][b][c] = new float[sizeC][];
					for (int d = 0; d < sizeC; ++d)
					{
						photonData[a][b][c][d] = new float[sizeD];
					}
				}
			}
		}
	}

	public float getPhotoData(int a, int b, int c, int d, int e) {
		return photonData [a] [b] [c] [d] [e];
	}

	public float[] getPhotonData(int a, int b, int c, int d) {
		return photonData [a] [b] [c] [d];
	}

	public float[][] getPhotonData(int a, int b, int c) {
		return photonData [a] [b] [c];
	}

	public float[][][] getPhotonData(int a, int b) {
		return photonData [a] [b]; 
	}

	public float[][][][] getPhotonData(int a) {
		return photonData [a];
	}

	public float[][][][][] getPhotonData() {
		return photonData;
	}

	public void setPhotoData(int a, int b, int c, int d, int e, float data) {
		photonData [a] [b] [c] [d] [e] = data;
	}

	public void setPhotonData(int a, int b, int c, int d, float[] data) {
		photonData [a] [b] [c] [d] = data;
	}

	public void setPhotonData(int a, int b, int c, float[][] data) {
		photonData [a] [b] [c] = data;
	}

	public void setPhotonData(int a, int b, float[][][] data) {
		photonData [a] [b] = data; 
	}

	public void setPhotonData(int a, float[][][][] data) {
		photonData [a] = data;
	}
		
	public int getNumberOfPhoton() {
		return numberOfPhotons;
	}

	public void setNumberOfPhoton(int numPhotons) {
		numberOfPhotons = numPhotons;
	}

	public int getNumberOfBounces() {
		return numberOfBounces;
	}

	public void setNumberOfBounces(int numBounces) {
		numberOfBounces = numBounces;
	}
}


