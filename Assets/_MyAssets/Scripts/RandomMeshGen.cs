using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter),(typeof(MeshRenderer)), (typeof(Shader)))]
public class RandomMeshGen : MonoBehaviour {

    [System.Serializable]
    public class Toppings
    {
        public string toppingName;
        public Mesh toppingMesh;
        public Material toppingShader;
        public float scaleFactor;
    }

    /// <summary>
    /// Set the total number of toppings as size of list
    /// and drag and drop each mesh and corresponding shader
    /// </summary>
    public List<Toppings> toppingsList = new List<Toppings>();

    // This is just how the toppings were received initially
    private float defaultToppingScale = 50.0f;  

    void OnEnable()
    {
        GetRandomTopping();
        GetRandomRotation();
    }

    private void GetRandomTopping()
    {
        Toppings randomTopping = toppingsList[Random.Range(0, toppingsList.Count)];
        gameObject.GetComponent<MeshFilter>().mesh = randomTopping.toppingMesh;
        gameObject.GetComponent<Renderer>().material = randomTopping.toppingShader;
        gameObject.name = randomTopping.toppingName;
        gameObject.transform.localScale = Vector3.one * (randomTopping.scaleFactor * defaultToppingScale);
    }

    private void GetRandomRotation()
    {
        transform.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));
    }
}
