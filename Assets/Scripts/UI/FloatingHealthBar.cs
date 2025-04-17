using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;

    public void UpdateHealthBar(float partialValue, float totalValue)
    {
        slider.value = partialValue / totalValue;
    }

    void Update()
    {
        Vector3 cameraRotation = camera.transform.rotation.eulerAngles;
        slider.transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
        slider.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }
}