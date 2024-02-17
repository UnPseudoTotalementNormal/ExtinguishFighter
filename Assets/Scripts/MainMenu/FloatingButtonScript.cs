using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 _basePosition;
    private Vector3 _baseEuler;
    private Quaternion _baseRotation;
    private Vector3 _baseScale;

    [SerializeField] private float _floatPositionMultiplier = 0.2f;
    [SerializeField] private float _floatRotationMultiplier = 0.2f;

    [SerializeField] private float _floatPositionSpeed = 0.3f;
    [SerializeField] private float _floatRotationSpeed = 0.3f;

    private Vector3 _rOffset;

    private bool _mouseInside = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseInside = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = transform.localScale * 0.95f;
    }

    private void OnDisable()
    {
        _mouseInside = false;
    }

    private void Awake()
    {
        _basePosition = transform.position;
        _baseEuler = transform.localEulerAngles;
        _baseRotation = transform.rotation;
        _baseScale = transform.localScale;
        _rOffset = new Vector3(Random.Range(-10000, 10000), Random.Range(-10000, 10000), Random.Range(-10000, 10000));
    }

    private void Update()
    {
        float xPos = _basePosition.x + Mathf.PerlinNoise(Time.time * _floatPositionSpeed, _rOffset.y) * _floatPositionMultiplier;
        float yPos = _basePosition.y + Mathf.PerlinNoise(_rOffset.x, Time.time * _floatPositionSpeed) * _floatPositionMultiplier;
        //float zPos = _basePosition.z + Mathf.PerlinNoise(Time.time, Time.time) * _floatPositionMultiplier;
        float zPos = _basePosition.z;
        transform.position = Vector3.Lerp(transform.position, new Vector3(xPos, yPos, zPos), 8 * Time.deltaTime);
        if (_mouseInside)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _baseScale * 1.15f, 8 * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, 8 * Time.deltaTime);
        }
    }
}
