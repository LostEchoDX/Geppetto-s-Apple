using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    public GameObject flechePrefab;      // Préfabriqué de la flèche
    public float forceTir = 10f;         // Force de tir de la flèche
    public BoxCollider zoneDetection;

    public float timeLeft = 6f;
    // Référence au BoxCollider de la zone de détection

    private bool estActive = true;       // Indique si le piège est actif

    AudioSource audioData;

    private void Start() {
        audioData = this.gameObject.GetComponent<AudioSource>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!estActive)
        {
            return;
        }

        // Instancier une nouvelle flèche à la position du piège
        GameObject nouvelleFleche = Instantiate(flechePrefab, transform.position, Quaternion.identity);

        // Orienter la flèche pour pointer dans la direction souhaitée
        nouvelleFleche.transform.Rotate(0f, -90.0f, 0f, Space.Self);

        // Appliquer une force à la flèche dans la direction souhaitée (par exemple, vers l'avant du piège)
        Rigidbody flecheRigidbody = nouvelleFleche.GetComponent<Rigidbody>();
        flecheRigidbody.AddForce(transform.up * forceTir, ForceMode.Impulse);

        audioData.Play(0);

        estActive = false;
        timeLeft = 6f;

    }

    private void Update()
    {


        if (timeLeft > 0 && estActive == false)
        {
            timeLeft -= Time.deltaTime;

        }

        if (timeLeft <= 0 && estActive == false)
        {
            estActive = true;
        }

    }

    // Méthode pour attribuer le BoxCollider externe à la variable zoneDetection
    public void SetZoneDetection(BoxCollider boxCollider)
    {
        zoneDetection = boxCollider;
    }
}
