using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTimeBase;
    private float fadeOutTime;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        fadeOutTime = fadeOutTimeBase;
    }
    private void Update()
    {
        fadeOutTime -= Time.deltaTime;
        Color holder = image.color;
        holder.a = fadeOutTimeBase - fadeOutTime / fadeOutTimeBase;
        image.color = holder;
        if (fadeOutTime < 0f)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
