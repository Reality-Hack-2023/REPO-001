using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensifyMaterialOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    Material mat;
    float targetIntensity = 10; // kind of a random number, results in an intensity of around 3
    
    void Start()
    {
        StartCoroutine("IntensifyMaterial");
    }
    
    IEnumerator IntensifyMaterial()
    {
        mat = GetComponent<Renderer>().material;
        Color startEmissionColor = mat.GetColor("_EmissionColor");
        for (float i = 0; i <= targetIntensity; i += 0.1f)
        {
            mat.SetColor("_EmissionColor", startEmissionColor * (1 + i));
            Debug.Log("e color: " + mat.GetColor("_EmissionColor"));
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}
