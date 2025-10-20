using UnityEngine;

public class LanzadorPelotas : MonoBehaviour
{
    // ... tu código igual ...

    [Header("Sistema de puntaje")]
    public TennisScoreManager scoreManager; // arrastrá el ScoreManager desde el inspector

    private string ladoActivo = ""; // "Izquierda" o "Derecha"

    // ... tu código igual ...

    void IluminarMitadObjetivo()
    {
        if (zonaIzquierda != null)
            zonaIzquierda.Apagar();

        if (zonaDerecha != null)
            zonaDerecha.Apagar();

        if (alternarLados)
        {
            if (proximaIzquierda)
            {
                if (zonaIzquierda != null)
                    zonaIzquierda.Iluminar(tiempoIluminado);
                ladoActivo = "Izquierda";
            }
            else
            {
                if (zonaDerecha != null)
                    zonaDerecha.Iluminar(tiempoIluminado);
                ladoActivo = "Derecha";
            }

            proximaIzquierda = !proximaIzquierda;
        }
        else
        {
            if (Random.value > 0.5f)
            {
                if (zonaIzquierda != null)
                    zonaIzquierda.Iluminar(tiempoIluminado);
                ladoActivo = "Izquierda";
            }
            else
            {
                if (zonaDerecha != null)
                    zonaDerecha.Iluminar(tiempoIluminado);
                ladoActivo = "Derecha";
            }
        }

        // Le avisamos al sistema de zonas cuál está activa
        ActiveZoneManager.Instance.SetActiveZone(ladoActivo);
    }

}
