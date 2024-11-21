using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Se necesita para gestionar el Canvas
using UnityEngine.SceneManagement;  // Se necesita para gestionar la escena

public class RayCast : MonoBehaviour
{
    public Text countdownText; // Se necesita para gestionar el texto del Canvas

    bool raycastUsed = false; // Se necesita para inhabilitar el raycast

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && raycastUsed == false) // Si pulsamos el boton izquierdo del raton y no hemos usado el raycast
        {
            Ray ray =  Camera.main.ScreenPointToRay(Input.mousePosition); // Creamos el rayo en la posicion del raton en la cámara

            RaycastHit hit;  // Se necesita para almacenar la informacion de colisiones
            
            if(Physics.Raycast(ray, out hit)) // Si el rayo choca con algo
            {
                if(hit.transform.tag == "1") // Si el objeto chocado tiene el tag 1
                {    
                    StartCoroutine(Countdown(0)); // Se inicia el contador y me carga la escena 0
                    raycastUsed = true; // Se habilita el raycast
                }
                else if(hit.transform.tag == "2") // Si el objeto chocado tiene el tag 2
                {
                    StartCoroutine(Countdown(1)); // Se inicia el contador y me carga la escena 1
                    raycastUsed = true; // Se habilita el raycast
                }
                else if(hit.transform.tag == "3") // Si el objeto chocado tiene el tag 3
                {
                    StartCoroutine(Countdown(2)); // Se inicia el contador y me carga la escena 2
                    raycastUsed = true; // Se habilita el raycast
                }

            }
        }
    }

     IEnumerator Countdown(int scene) // Funcion para gestionar el contador
    {
        while(countdownText.text != "Cargado!") // Mientras el texto no sea GO
        {
            countdownText.text = "5";
            yield return new WaitForSeconds(1f);
            countdownText.text = "4";
            yield return new WaitForSeconds(1f);
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            countdownText.text = "Cargado!";
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(scene);
            raycastUsed = false;
        }


    }
}