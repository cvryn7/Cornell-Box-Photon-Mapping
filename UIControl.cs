using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    private Canvas canvas;
    public Slider sliderSphere1X;
    public Slider sliderSphere1Y;
    public Slider sliderSphere2X;
    public Slider sliderSphere2Y;
    public Slider numBouncSlider;
    public Slider lightSliderX;
    public Slider lightSliderY;
    public Slider lightSliderZ;

    public Toggle sphere1ReflectionToggle;
    public Toggle sphere2ReflectionToggle;

    public Button addSphereButton;
    public Button removeSphereButton;

    public InputField xInput;
    public InputField yInput;
    public InputField zInput;

    public Toggle newSphereReflectionToggle;

    public PhotonMappingStart rayTracing;
    public Plane plane;
    public float sliderSphere1XOld;
    public float sliderSphere1YOld;
    public float sliderSphere2XOld;
    public float sliderSphere2YOld;
    public float numBounceSliderOld;
    public float lightSliderXOld;
    public float lightSliderYOld;
    public float lightSliderZOld;

    public bool sphere1ReflectionOld;
    public bool sphere2ReflectionOld;

    public float newValue;
    public bool newReflectionToggle;

    public int numOfSpheresOld;
    public int numOfSpheresNew;
	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        sliderSphere1X = GameObject.Find("Slider").GetComponent<Slider>();
        sliderSphere1Y = GameObject.Find("Slider (1)").GetComponent<Slider>();
        sliderSphere2X = GameObject.Find("Slider (2)").GetComponent<Slider>();
        sliderSphere2Y = GameObject.Find("Slider (3)").GetComponent<Slider>();
        numBouncSlider = GameObject.Find("Slider (4)").GetComponent<Slider>();
        lightSliderX = GameObject.Find("Slider (5)").GetComponent<Slider>();
        lightSliderY = GameObject.Find("Slider (6)").GetComponent<Slider>();
        lightSliderZ = GameObject.Find("Slider (7)").GetComponent<Slider>();

        sphere1ReflectionToggle = GameObject.Find("Toggle").GetComponent<Toggle>();
        sphere2ReflectionToggle = GameObject.Find("Toggle (1)").GetComponent<Toggle>();
        newSphereReflectionToggle = GameObject.Find("Toggle (2)").GetComponent<Toggle>();

        addSphereButton = GameObject.Find("Button").GetComponent<Button>();
        removeSphereButton = GameObject.Find("Button (1)").GetComponent<Button>();
        addSphereButton.onClick.AddListener(addSphere);
        removeSphereButton.onClick.AddListener(removeSphere);

        xInput = GameObject.Find("InputField").GetComponent<InputField>();
        yInput = GameObject.Find("InputField (1)").GetComponent<InputField>();
        zInput = GameObject.Find("InputField (2)").GetComponent<InputField>();

        //plane = GameObject.Find("Plane").GetComponent<Plane>();
        sliderSphere1X.value = sliderSphere1XOld = PhotonMappingStart.objects.getSphereData(0,0);
        sliderSphere1Y.value = sliderSphere1YOld = PhotonMappingStart.objects.getSphereData(0,1);
        sliderSphere2X.value = sliderSphere2XOld = PhotonMappingStart.objects.getSphereData(1,0);
        sliderSphere2Y.value = sliderSphere2YOld = PhotonMappingStart.objects.getSphereData(1,1);
		numBouncSlider.value = numBounceSliderOld = PhotonMappingStart.photonMap.getNumberOfBounces();
        lightSliderX.value = lightSliderXOld = PhotonMappingStart.lightSource[0];
        lightSliderY.value = lightSliderYOld = PhotonMappingStart.lightSource[1];
        lightSliderZ.value = lightSliderZOld = PhotonMappingStart.lightSource[2];

        sphere1ReflectionToggle.isOn = sphere1ReflectionOld = PhotonMappingStart.metalObject[0];
        sphere2ReflectionToggle.isOn = sphere2ReflectionOld = PhotonMappingStart.metalObject[1];

		numOfSpheresOld = numOfSpheresNew = PhotonMappingStart.objects.getObjectsPerType(0);
    }
	
    void addSphere()
    {
        numOfSpheresNew++;
    }

    void removeSphere()
    {
        numOfSpheresNew--;
    }

	// Update is called once per frame
	void Update () {
        newValue = sliderSphere1X.value;
        if (newValue != sliderSphere1XOld)
        {
            PhotonMappingStart.objects.setSphereData(0,0, newValue);
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sliderSphere1XOld = newValue;
        }

        newValue = sliderSphere1Y.value;
        if (newValue != sliderSphere1YOld)
        {
            PhotonMappingStart.objects.setSphereData(0, 1, newValue);
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sliderSphere1YOld = newValue;
        }
        newValue = sliderSphere2X.value;
        if (newValue != sliderSphere2XOld)
        {
            PhotonMappingStart.objects.setSphereData(1, 0, newValue);
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sliderSphere2XOld = newValue;
        }
        newValue = sliderSphere2Y.value;
        if (newValue != sliderSphere2YOld)
        {
            PhotonMappingStart.objects.setSphereData(1, 1, newValue);
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sliderSphere2YOld = newValue;
        }
        newValue = numBouncSlider.value;
        if (newValue != numBounceSliderOld)
        {
			PhotonMappingStart.photonMap.setNumberOfBounces((int)newValue);
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            numBounceSliderOld = newValue;
        }
        newValue = lightSliderX.value;
        if (newValue != lightSliderXOld)
        {
            PhotonMappingStart.lightSource[0] = newValue;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            lightSliderXOld = newValue;
        }
        newValue = lightSliderY.value;
        if (newValue != lightSliderYOld)
        {
            PhotonMappingStart.lightSource[1] = newValue;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            lightSliderYOld = newValue;
        }
        newValue = lightSliderZ.value;
        if (newValue != lightSliderZOld)
        {
            PhotonMappingStart.lightSource[2] = newValue;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            lightSliderZOld = newValue;
        }
        newReflectionToggle = sphere1ReflectionToggle.isOn;
        if(newReflectionToggle != sphere1ReflectionOld)
        {
            PhotonMappingStart.metalObject[0] = newReflectionToggle;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sphere1ReflectionOld = newReflectionToggle;
        }
        newReflectionToggle = sphere2ReflectionToggle.isOn;
        if (newReflectionToggle != sphere2ReflectionOld)
        {
            PhotonMappingStart.metalObject[1] = newReflectionToggle;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
            sphere2ReflectionOld = newReflectionToggle;
        }

        if (numOfSpheresNew != numOfSpheresOld)
        {
			PhotonMappingStart.objects.objectsPerType[0] = numOfSpheresNew;
            if (numOfSpheresNew > numOfSpheresOld)
            {
				PhotonMappingStart.objects.setSphereData(numOfSpheresNew-1, 0, float.Parse(xInput.text));
				PhotonMappingStart.objects.setSphereData(numOfSpheresNew-1, 1, float.Parse(yInput.text));
				PhotonMappingStart.objects.setSphereData(numOfSpheresNew-1, 2, float.Parse(zInput.text));
                if(newSphereReflectionToggle.isOn)
                {
                    PhotonMappingStart.metalObject[numOfSpheresNew - 1] = true;
                }
            }
            numOfSpheresOld = numOfSpheresNew;
            GameObject.Find("Plane").GetComponent<PhotonMappingStart>().init();
        }
    }
}
