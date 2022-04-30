using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    private Animation fadeOutAnimation;
    private void Start()
    {
        fadeOutAnimation = GetComponent<Animation>();
        fadeOutAnimation.Play();
    }
    private void Update()
    {
        if (!fadeOutAnimation.isPlaying)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
