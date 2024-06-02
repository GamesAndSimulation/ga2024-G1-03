using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    [SerializeField] private Renderer[] characterRenderer;
    [SerializeField] private Material whiteMaterial; 
    [SerializeField] private Material[] originalMaterial;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FlashWhite(float flashDuration)
    {

        foreach(Renderer rendered in characterRenderer)
        {
            if (rendered.gameObject.activeInHierarchy)
                rendered.material = whiteMaterial;
        }
        int i = 0;
        yield return new WaitForSeconds(flashDuration/7);
        foreach(Renderer rendered in characterRenderer)
        {
            if (rendered.gameObject.activeInHierarchy)
                rendered.material = originalMaterial[i];
            i++;
        }
        yield return new WaitForSeconds(flashDuration/5);
        foreach(Renderer rendered in characterRenderer)
        {
            if (rendered.gameObject.activeInHierarchy)
                rendered.material = whiteMaterial;
        }
        i = 0;
        yield return new WaitForSeconds(flashDuration/7);
        foreach(Renderer rendered in characterRenderer)
        {
            if (rendered.gameObject.activeInHierarchy)
                rendered.material = originalMaterial[i];
            i++;
        }
    }

}
