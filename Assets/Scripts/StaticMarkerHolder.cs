using System.Collections.Generic;
using UnityEngine;

public class StaticMarkerHolder : MonoBehaviour
{
    public GameObject markerPrefab;
    public GameObject playerObject;
    public RectTransform markerParentRectTransform;
    public Camera minimapCamera;

    public Transform[] staticPoints; // <- Adiciona isso

    private List<(Transform worldPosition, RectTransform markerRectTransform)> staticMarkers;

    void Awake()
    {
        staticMarkers = new List<(Transform, RectTransform)>();

        // Instancia os marcadores logo ao iniciar
        foreach (Transform point in staticPoints)
        {
            AddStaticMarker(point);
        }
    }

    void Update()
    {
        foreach (var marker in staticMarkers)
        {
            Vector3 offset = Vector3.ClampMagnitude(marker.worldPosition.position - playerObject.transform.position, minimapCamera.orthographicSize);
            offset = offset / minimapCamera.orthographicSize * (markerParentRectTransform.rect.width / 2f);
            marker.markerRectTransform.anchoredPosition = new Vector2(offset.x, offset.z);
        }
    }

    public void AddStaticMarker(Transform worldPosition)
    {
        RectTransform markerUI = Instantiate(markerPrefab, markerParentRectTransform).GetComponent<RectTransform>();
        staticMarkers.Add((worldPosition, markerUI));
    }
}
