using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotStupidPostProcessing : MonoBehaviour
{
    public Material rTMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, rTMat);
    }
}
