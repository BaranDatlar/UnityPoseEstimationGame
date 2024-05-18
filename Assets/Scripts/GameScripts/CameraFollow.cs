using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Takip edilecek oyuncu (Player)
    public Vector3 offset; // Kamera ile oyuncu arasındaki mesafe

    void Start()
    {
        // Offset'i başlatma pozisyonuna göre ayarla
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Kamera pozisyonunu oyuncunun pozisyonuna göre güncelle
        transform.position = player.position + offset;

        // Kameranın oyuncuya dönük olduğundan emin ol
        transform.LookAt(player);
    }
}
