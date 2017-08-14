<h2>Cornell Box Using Photon Mapping</h2>
<p>Developed using Unity and C#.</p>

<h3>Description</h3>
<p>
For constructing a realistic Cornell Box, I use photon mapping. Program by default emits 1000 photons. Each photon is allowed to bounce three times from the objects. By default, there are total of 7 objects in the scene: two spheres and 5 planes. During execution a photon map is created which stores number of photon for each object. Each object is differentiated based on their index in the photon map. For each photon in the map, it stores location of the incident, direction of the photon when it intersects an object and energy (color) on that point of the surface. This map is filled recursively by bouncing a photo from the objects’ surfaces. Initially the photon starts from a light source, and then once the photon hits an object, the photon is reflected from the surface in the direction of the normal of the surface. During the photon hit, the energy, location and direction of the photon is stored in the photon map under that type of object and index of that particular object. Then the bounce process of photons continues and follows the same process. Once the whole photo map is filled the render process starts a ray for every coordinate of the render area and checks which all points the ray hit in the world. For each point hit, that point in looked up in the photon map and all the photons for that point are aggregated to form the final energy on the particular point. 
For the interactivity, number of bounces can be increased or decreased in the real time. The bounces slider on the UI changes the value of the bounces variable in the PhotonMappingStart class. Similarly, both the default spheres and light source can be translated in the scene using the provided sliders on the UI. Application also provides ability to add more spheres or remove the already present spheres.
</p>

![alt tag](https://github.com/cvryn7/Cornell-Box-Photon-Mapping/blob/master/images/p1.jpg)
![alt tag](https://github.com/cvryn7/Cornell-Box-Photon-Mapping/blob/master/images/p2.png)
![alt tag](https://github.com/cvryn7/Cornell-Box-Photon-Mapping/blob/master/images/p3.png)