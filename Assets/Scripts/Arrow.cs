using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float lifeSpan = 0f;
    private void OnCollisionEnter(Collision collision)
    {

        // Récupérer l'objet en collision
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.tag == "Teleportable")
        {
            Debug.Log(collidedObject.tag);
            //  coordonnées 
            Vector3 teleportPosition = new Vector3(206f, 17f, 295f);

            // Téléporter l'objet en collision aux coordonnées spécifiées
            collidedObject.transform.position = teleportPosition;

            Destroy(gameObject);

        }
    }

    private void Update()
    {
        lifeSpan += Time.deltaTime;

        if (lifeSpan > 20)
        {
            Destroy(gameObject);
        }
    }
}
