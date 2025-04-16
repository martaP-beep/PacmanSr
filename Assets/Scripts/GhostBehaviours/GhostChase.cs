using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehaviour
{
    private void OnDisable()
    {
        if (Ghost.Scatter) Ghost.Scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // je�li zachowanie jest aktywne i zderzyli�my si� z obiektem posiadaj�cym skrypt Node
        if (enabled && collision.TryGetComponent(out Node node))
        {
            // domy�lny kierunek to (0,0)
            Vector2 direction = Vector2.zero;
            // min dystans - ustawiamy na najwi�kszy mo�liwy
            // tak aby pierwszy kierunek by� od razu dotychczasow� najlepsz� opcj�
            float minDistance = float.MaxValue;

            // dla ka�dego kierunku w obecnym w�le
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // ustalamy now� pozycj� jako obecn� przesuni�t� o sprawdzany kierunek
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
                // obliczamy dystans - u�ywamy sqrtMagnitude zamiast magnitude dla performance
                float distance = (Ghost.Target.position - newPosition).sqrMagnitude;

                // je�li obecny dystans jest mniejszy ni� dotychczasowy najmniejszy
                // to ten kierunek powinen zbli�y� ducha do Pacmana bardziej
                if (distance < minDistance)
                {
                    // ustawiamy kierunek na sprawdzany kierunek
                    direction = availableDirection;
                    // ustawiamy min dystans na obecny dystans
                    minDistance = distance;
                }
            }

            // ustalenie kierunku na najlepszy

        }
    }
}
