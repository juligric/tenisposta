using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    [Header("UI")]
    public GameObject actionPanel;
    public GameObject botonMiniatura; // Botón "+"
    public GameObject botonCerrarMiniatura;
    public GameObject panelMiniatura;
    public RawImage miniaturaUI; // RawImage para la miniatura
    public Sprite spritePorDefecto;

    [Header("Medidor barra")]
    public Image barra; // Asignar en inspector
    public float incrementoBarra = 0.2f; // cuánto sube por uso

    private GameObject selectedObject;
    private Rigidbody selectedRigidbody;

    private GameObject objetoGuardado;
    private Vector3 posicionOriginal;
    private Transform padreOriginal;

    private float zCoord;
    private Vector3 offset;
    private GameObject inventario;

    void Start()
    {
        actionPanel.SetActive(false);
        panelMiniatura.SetActive(false);
        botonCerrarMiniatura.SetActive(false);
        botonMiniatura.SetActive(false); // No aparece hasta que guardes algo

        if (barra != null)
            barra.fillAmount = 0.3f; // arranca vacía
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Draggable"))
                {
                    Debug.Log("Objeto draggable detectado: " + hit.transform.name);

                    selectedRigidbody = hit.transform.GetComponent<Rigidbody>();
                    selectedObject = hit.transform.gameObject;

                    if (selectedRigidbody != null)
                    {
                        selectedRigidbody.isKinematic = true;
                        zCoord = Camera.main.WorldToScreenPoint(hit.transform.position).z;
                        Vector3 mousePoint = Input.mousePosition;
                        mousePoint.z = zCoord;
                        offset = hit.transform.position - Camera.main.ScreenToWorldPoint(mousePoint);

                        actionPanel.SetActive(true);

                        // Ocultamos inventario mientras se agarra
                        panelMiniatura.SetActive(false);
                        botonMiniatura.SetActive(false);
                        botonCerrarMiniatura.SetActive(false);

                        Debug.Log("Panel de acciones mostrado.");
                    }
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedRigidbody != null)
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = zCoord;
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePoint) + offset;
            selectedRigidbody.transform.position = targetPosition;
        }

        if (Input.GetMouseButtonUp(0) && selectedRigidbody != null)
        {
            selectedRigidbody.isKinematic = false;
            selectedRigidbody = null;
        }
    }

    public void GuardarObjeto()
    {
        if (selectedObject != null)
        {
            objetoGuardado = selectedObject;
            posicionOriginal = objetoGuardado.transform.position;
            padreOriginal = objetoGuardado.transform.parent;

            objetoGuardado.SetActive(false);
            selectedObject = null;
            selectedRigidbody = null;

            actionPanel.SetActive(false);

            // Mostramos el botón "+"
            botonMiniatura.SetActive(true);

            // Guardamos miniatura
            Sprite spriteObjeto = ObtenerSpriteDeObjeto(objetoGuardado);
            miniaturaUI.texture = spriteObjeto != null ? spriteObjeto.texture : spritePorDefecto.texture;

            Debug.Log("Objeto guardado: " + objetoGuardado.name);
        }
        else
        {
            Debug.LogWarning("GuardarObjeto() llamado pero selectedObject es NULL.");
        }
    }

    private Sprite ObtenerSpriteDeObjeto(GameObject objeto)
    {
        SpriteRenderer sr = objeto.GetComponent<SpriteRenderer>();
        if (sr != null)
            return sr.sprite;

        return null;
    }

    public void MostrarPanelMiniatura()
    {
        if (objetoGuardado != null)
        {
            panelMiniatura.SetActive(true);
            botonCerrarMiniatura.SetActive(false);
            botonMiniatura.SetActive(false);

            StartCoroutine(ActivarBotonCerrarConDelay(3f));
        }
        else
        {
            Debug.LogWarning("No hay objeto guardado para mostrar.");
        }
    }

    private IEnumerator ActivarBotonCerrarConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        botonCerrarMiniatura.SetActive(true);
    }

    public void CerrarPanelMiniatura()
    {
        inventario.SetActive(false);
        botonCerrarMiniatura.SetActive(false);
        botonMiniatura.SetActive(true);
    }

    // Botón Usar
    public void UsarObjeto()
    {
        if (selectedObject != null)
        {
            Debug.Log("Usando objeto: " + selectedObject.name);

            Destroy(selectedObject);
            actionPanel.SetActive(false);

            // Subir barra del medidor
            if (barra != null)
            {
                barra.fillAmount += incrementoBarra;
                barra.fillAmount = Mathf.Clamp01(barra.fillAmount);
            }
        }
        else
        {
            Debug.LogWarning("No hay objeto seleccionado para usar.");
        }
    }
}
