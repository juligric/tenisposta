using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHoldDetector : MonoBehaviour
{
    public float holdTime = 3f; // Segundos para activar
    private float timer = 0f;
    private bool isHolding = false;

    public UnityEvent onHoldComplete; // Botón Guardar
    public UnityEvent onUseComplete;  // Botón Usar

    private GameObject currentObject;

    [Header("UI Visual")]
    public Image progresoVisual; // Asigna el Image circular en el inspector

    [Header("Botón tipo")]
    public bool esBotonUsar = false; // Si es true, se comporta como Usar; si no, como Guardar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Draggable"))
        {
            isHolding = true;
            timer = 0f;
            currentObject = other.gameObject;

            if (progresoVisual != null)
            {
                progresoVisual.gameObject.SetActive(true);
                progresoVisual.fillAmount = 0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isHolding && other.CompareTag("Draggable") && other.gameObject == currentObject)
        {
            timer += Time.deltaTime;

            if (progresoVisual != null)
                progresoVisual.fillAmount = timer / holdTime;

            if (timer >= holdTime)
            {
                if (esBotonUsar)
                {
                    ActivarUso();
                }
                else
                {
                    onHoldComplete.Invoke();
                    ResetHold();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Draggable") && other.gameObject == currentObject)
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        timer = 0f;
        currentObject = null;

        if (progresoVisual != null)
        {
            progresoVisual.fillAmount = 0f;
            progresoVisual.gameObject.SetActive(false);
        }
    }

    // Nuevo método para el botón Usar
    private void ActivarUso()
    {
        if (currentObject != null)
        {
            // Desaparece el objeto
            currentObject.SetActive(false);

            // Invoca el evento para actualizar medidores
            onUseComplete.Invoke();

            // Resetear hold
            ResetHold();
        }
    }


}
