using UnityEngine;

public class Shield : MonoBehaviour
{
    public float forceMagnitude = 10f; // Modifier selon l'intensité de la force souhaitée




    private void OnCollisionEnter(Collision collision)
    {
        // Vérifier si l'objet en collision possède un Rigidbody attaché
        Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            // Calculer la direction vers laquelle la caméra est orientée
            Vector3 cameraDirection = this.transform.forward;

            // Appliquer une force à l'objet en collision dans la direction de la caméra
            otherRigidbody.AddForce(cameraDirection * forceMagnitude, ForceMode.Impulse);

            // Renvoyer l'objet en collision
            Debug.Log("Objet renvoyé : " + collision.gameObject.name);
        }
    }
}
