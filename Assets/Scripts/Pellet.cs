using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer 
            == LayerMask.NameToLayer("Pacman"))
        {
            GameManager.Instance.PelletEaten(this);
        }
    }

}
