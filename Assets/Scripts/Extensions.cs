using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    #region Transform

    public static void SetPositionX(this Transform transform, float x)
    {
        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }

    public static void SetPositionY(this Transform transform, float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    public static void SetPositionZ(this Transform transform, float z)
    {
        Vector3 pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }

    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 pos = transform.localPosition;
        pos.x = x;
        transform.localPosition = pos;
    }

    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 pos = transform.localPosition;
        pos.y = y;
        transform.localPosition = pos;
    }

    public static void SetLocalZ(this Transform transform, float z)
    {
        Vector3 pos = transform.localPosition;
        pos.z = z;
        transform.localPosition = pos;
    }

    /// <summary>
    /// Sets the position to 0, 0, 0.
    /// </summary>
    public static void ResetPosition(this Transform transform)
    {
        transform.position = Vector3.zero;
    }

    /// <summary>
    /// Sets the local position to 0, 0, 0.
    /// </summary>
    public static void ResetLocalPosition(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
    }

    public static void ResetAll(this Transform transform)
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        transform.localScale = Vector3.one;
    }

    public static void SetScale(this Transform transform, float scale)
    {
        transform.localScale = scale * Vector3.one;
    }

    #endregion

    #region Lists

    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 1; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    public static T ChooseRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T ChooseRandom<T>(this IList<T> list, out int index)
    {
        index = Random.Range(0, list.Count);
        return list[index];
    }

    public static T ChooseCircular<T>(this IList<T> list, int index)
    {
        return list[index % list.Count];
    }

    public static T Pop<T>(this IList<T> list)
    {
        T element = list[Random.Range(0, list.Count)];

        list.Remove(element);

        return element;
    }

    #endregion

    #region Arrays

    public static T ChooseRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static T ChooseRandom<T>(this T[] array, out int index)
    {
        index = Random.Range(0, array.Length);
        return array[index];
    }

    public static T ChooseCircular<T>(this T[] array, int index)
    {
        return array[index % array.Length];
    }

    #endregion

    #region HashSet

    public static T ChooseRandom<T>(this HashSet<T> set)
    {
        return set.ElementAt(Random.Range(0, set.Count));
    }

    public static T Pop<T>(this HashSet<T> set)
    {
        T element = set.ElementAt(Random.Range(0, set.Count));

        set.Remove(element);

        return element;
    }

    #endregion

    #region SpriteRenderer

    public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    #endregion

    #region Image

    public static void SetAlpha(this Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    #endregion

    #region TextMeshPro

    public static void SetAlpha(this TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    #endregion

    #region Component

    public static void Show(this Component component)
    {
        component.gameObject.SetActive(true);
    }

    public static void Hide(this Component component)
    {
        component.gameObject.SetActive(false);
    }

    public static Vector3 GetWorldPosition(this Component component)
    {
        return component.transform.position;
    }

    public static void SetWorldPosition(this Component component, Vector3 position)
    {
        component.transform.position = position;
    }

    #endregion

    #region Particle System

    public static void SetStartColor(this ParticleSystem particle, Color color)
    {
        var mainModule = particle.main;

        var minMaxGradient = mainModule.startColor;
        minMaxGradient.color = color;

        mainModule.startColor = minMaxGradient;
    }

    public static void SetGradient(this ParticleSystem particle, Gradient gradient)
    {
        var colorOverLifeTime = particle.colorOverLifetime;
        var minMaxGradient = colorOverLifeTime.color;

        minMaxGradient.gradient = gradient;
        colorOverLifeTime.color = minMaxGradient;
    }

    public static void SetTextureSheetSprite(this ParticleSystem particle, Sprite sprite)
    {
        var textureSheetModule = particle.textureSheetAnimation;

        textureSheetModule.SetSprite(0, sprite);
    }

    #endregion

    #region Canvas

    public static void SetCanvasLayer(this Canvas canvas, string layer)
    {
        canvas.sortingLayerName = layer;
    }

    #endregion


}