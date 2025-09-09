using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndDropUI : MonoBehaviour
{
    [Header("Raycast")]
    public Camera cam;
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    [Header("Progreso")]
    public Image progresoVisual;
    public float holdTime = 2f;

    private float timer = 0f;
    private GameObject currentButton = null;
    private bool isHolding = false;

    void Update()
    {

        if (cam == null) cam = Camera.main;

        // Posición del cubo en pantalla
        Vector2 screenPos = cam.WorldToScreenPoint(transform.position);

        // Raycast UI en esa posición
        PointerEventData ped = new PointerEventData(eventSystem);
        ped.position = screenPos;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(ped, results);

        if (results.Count > 0)
        {
            GameObject hitUI = results[0].gameObject;

            if (hitUI.name == "usar" || hitUI.name == "guardar")
            {
                if (currentButton != hitUI)
                {
                    // Entramos a un nuevo botón
                    currentButton = hitUI;
                    timer = 0f;
                    progresoVisual.gameObject.SetActive(true);
                }

                isHolding = true;
                timer += Time.deltaTime;
                progresoVisual.fillAmount = timer / holdTime;

                if (timer >= holdTime)
                {
                    if (hitUI.name == "usar")
                    {
                        Debug.Log("✅ Acción USAR ejecutada");
                        // Llamar a tu función UsarObjeto()
                    }
                    else if (hitUI.name == "guardar")
                    {
                        Debug.Log("📦 Acción GUARDAR ejecutada");
                        // Llamar a tu función GuardarObjeto()
                    }

                    Reset();
                }
            }
        }
        else
        {
            Reset();
        }
        
        if (isHolding) {
    Debug.Log("Cube en pantalla: " + Input.mousePosition);

    if (results.Count > 0) {
        foreach (var r in results) {
            Debug.Log("Detecté sobre: " + r.gameObject.name);
        }
    }
}
    }

    void Reset()
    {
        isHolding = false;
        timer = 0f;
        currentButton = null;
        progresoVisual.fillAmount = 0f;
        progresoVisual.gameObject.SetActive(false);
    }
}
