using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] private GameObject overlayCanvas;
    [SerializeField] private GameObject objects;
    [SerializeField] private RawImage image;
    
    public bool startActive = false;

    void Awake()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, startActive ? 1 : 0); 
    }
    
    public IEnumerator FadeIn() {
        overlayCanvas.SetActive(true);

        Color c = image.color;
        for (float alpha = 0f; alpha <= 1f; alpha += 0.01f) {
            c.a = alpha;
            image.color = c;
            yield return null;
        }

        if(objects != null) {
            objects.SetActive(true);
        }
    } 
    
    public IEnumerator FadeOut() {
        if (objects != null) {
            objects.SetActive(false);
        }

        Color c = image.color;
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.01f) {

            c.a = alpha;
            image.color = c;
            yield return new WaitForSeconds(0.01f);
        }

        overlayCanvas.SetActive(false);
    }
}
