using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject objectToPress; // Game object (tombol) yang akan dipencet

    private bool canSpawn = false;

    void Update()
    {
        if (canSpawn && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            worldPosition.y = transform.position.y; // Tetapkan posisi y ke posisi objek yang ada

            Instantiate(prefabToSpawn, worldPosition, Quaternion.identity);
        }
    }

    void Start()
    {
        // Mengatur callback untuk mendeteksi ketika tombol ditekan
        if (objectToPress != null)
        {
            objectToPress.GetComponent<Collider>().isTrigger = true;
            canSpawn = true;
        }
    }

   // public void PressedObject()
    //{
      //  canSpawn = true;
    //}
}



    /*void Update()
    {
        if (Input.touchCount > 0) // Periksa jika ada sentuhan layar
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // Periksa jika sentuhan dimulai
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // Tetapkan posisi y ke posisi objek yang ada
                touchPosition.y = transform.position.y; // Ganti 'transform' dengan transform objek yang diinginkan

                Instantiate(prefabToSpawn, touchPosition, Quaternion.identity);
            }
        }
    }
    */


