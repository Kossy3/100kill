using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(TheShake(duration, magnitude));
    }

    private IEnumerator TheShake(float duration, float magnitude)
    {
        Vector3 pos = transform.localPosition;

        float elapsed = 0f; // 経過時間

        // duration・・・＞どのくらいの時間揺らすかをコントロール
        while(elapsed < duration)
        {
            // magnitude・・・＞どのくらいの大きさで揺らすかをコントロール
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = pos;
    }
}