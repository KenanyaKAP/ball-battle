using UnityEngine;

public static class Utils {
    // Vector 3 Clamp
    public static Vector3 Vector3Clamp(Vector3 value, Vector3 min, Vector3 max) {
        float clampedX = Mathf.Clamp(value.x, min.x, max.x);
        float clampedY = Mathf.Clamp(value.y, min.y, max.y);
        float clampedZ = Mathf.Clamp(value.z, min.z, max.z);

        return new Vector3(clampedX, clampedY, clampedZ);
    }

    // Snap To Grid
    public static Vector3 SnapToGrid(Vector3 position)
    {
        Vector3 gridSize = new Vector3(2f, 2f, 2f);
        Vector3 center = new Vector3(-1f, 0f, -1f);

        float snappedX = Mathf.Round((position.x - center.x) / gridSize.x) * gridSize.x + center.x;
        float snappedY = Mathf.Round((position.y - center.y) / gridSize.y) * gridSize.y + center.y;
        float snappedZ = Mathf.Round((position.z - center.z) / gridSize.z) * gridSize.z + center.z;

        return new Vector3(snappedX, snappedY, snappedZ);
    }

}
