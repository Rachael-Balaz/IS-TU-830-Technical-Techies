using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    
    public class Fader : MonoBehaviour 
    {
        CanvasGroup canvasGroup;
        private void Start() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }


        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            print("Faded Out");
            yield return FadeIn(1f);
            print("Faded In");

        }
        public IEnumerator FadeOut(float time)
        {
            //When Alpha is less than 1
            while(canvasGroup.alpha < 1) 
            {
                //moving alpha toward 1
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            //When Alpha is more than 0
            while(canvasGroup.alpha > 0)
            {
                //decrease alpha back to 0
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}