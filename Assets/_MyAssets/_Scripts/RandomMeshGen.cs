using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter),(typeof(MeshRenderer)), (typeof(Shader)))]
public class RandomMeshGen : MonoBehaviour {

    [System.Serializable]
    public class Toppings
    {
        public Mesh toppingMesh;
        public Material toppingShader;
    }

    /// <summary>
    /// Set the total number of toppings as size of list
    /// and drag and drop each mesh and corresponding shader
    /// </summary>
    public List<Toppings> toppingsList = new List<Toppings>();

    void OnEnable()
    {
        GetRandomTopping();
    }

    void OnDisable()
    {
        GetRandomTopping();
    }

    private void GetRandomTopping()
    {
        Toppings randomTopping = toppingsList[Random.Range(0, toppingsList.Count)];
        gameObject.GetComponent<MeshFilter>().mesh = randomTopping.toppingMesh;
        gameObject.GetComponent<Renderer>().material = randomTopping.toppingShader;
    }
}
