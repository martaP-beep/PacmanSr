/********************************

Najwa�niejsze punkty zachowania GhostHome to:
1. Po aktywacji duch odbija si� od �cian i zostaje w bazie do jego 
   zako�czenia niezale�nie od tego jaki inny stan jest aktywowany.
2. Po zako�czeniu tego stanu wy��czamy poruszanie si� gracza na czas 
   sekwencji wyj�cia z bazy i jest uruchamiana sekwencja wyj�cia 
   sk�adaj�ca si� z dw�ch etap�w:
      - przemieszczenia si� do w�z�a Inside b�d�cego w �rodku bazy
      - przemieszczenia si� do w�z�a Outside b�d�cego zaraz na zewn�trz bazy
3. Po zako�czeniu sekwencji wyj�cia z bazy sterowanie jest oddawane do skryptu 
   Movement z losowo ustawionym kierunkiem prawo/lewo i stanem, i stanem kt�ry 
   jest aktualnie aktywny (b�dzie to stan GhostScatter albo GhostChase).

********************************/

using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    [SerializeField] private Transform inside;
    public Transform Inside => inside;
    [SerializeField] private Transform outside;
    public Transform Outside => outside;

    // uruchamianie sekwencji wyj�cia z bazy w momencie wyj�cia ze stanu GhostHome
    private void OnDisable()
    {
        if(Ghost && Ghost.Movement && gameObject.activeSelf)
            StartCoroutine(ExitTransition());
    }

    private IEnumerator ExitTransition()
    {
        // wy��czenie sterowania i fizyki Rigidbody dla obiektu
        // (isKinematic sprawia �e rb si�y nie wp�ywaj� na obiekt)
        Ghost.Movement.rb.isKinematic = true;
        Ghost.Movement.SetDirection(Vector2.up);
        Ghost.Movement.enabled = false;

        // pozycja startowa
        Vector3 position = transform.position;

        // czas animacji od pozycji startowej w bazie do Inside
        float duration = 0.5f;
        // czas kt�ry up�yn��
        float elapsed = 0.0f;

        // wykonanie tej funkcji b�dzie roz�o�one w czasie
        while (elapsed < duration)
        {
            // ustalamy now� pozycj� jako po�o�enie mi�dzy pozycj� startow� a pozycj� Inside
            // w zale�no�ci od czasu kt�ry up�yn�� elapsed przez d�ugo�� animacji
            Vector3 newPosition = Vector3.Lerp(position, inside.position, elapsed/duration);
            // przywracamy domy�ln� pozycj� z (u nas b�dzie ona wynosi�a 0)
            newPosition.z = position.z;
            // przesuwamy ducha na okre�lon� pozycj�
            Ghost.transform.position = newPosition;
            // zwi�kszamy czas kt�ry up�yn��
            elapsed += Time.deltaTime;
            yield return null; // tutaj b�dziemy wznawia� od kolejnej klatki
        }

        // zerujemy czas kt�ry up�yn��
        elapsed = 0f;

        // poni�sza p�tla jest taka sama jak powy�sza tylko teraz startem jest pozycja Inide a ko�cowa to Outside
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(inside.position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            Ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // tutaj b�dziemy wznawia� od kolejnej klatki
        }

        // losujemy kierunek, uruchamiamy Movement, ustawiamy mu kierunek i w��czamy fizyk� w Rigidbody
        // (isKinematic sprawia �e rb si�y nie wp�ywaj� na obiekt)
        Vector2 randomDirection = new Vector2(Random.value < 0.5f ? -1 : 1, 0f);
        Ghost.Movement.enabled = true;
        Ghost.Movement.SetDirection(randomDirection);
        Ghost.Movement.rb.isKinematic = false;
    }


    // Odbijanie si� od �cian w stanie GhostHome
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled)
        {
            Ghost.Movement.SetDirection(-Ghost.Movement.direction);
        }
    }
}
