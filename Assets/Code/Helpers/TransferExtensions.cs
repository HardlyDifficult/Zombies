using System.Collections.Generic;
using UnityEngine;

public static class TransferExtensions
{
    private static void AddChildrenToList(this Transform transform, List<Transform> list)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            list.Add(child);
            child.AddChildrenToList(list);
        }
    }

    public static List<Transform> CreateListOfChildren(this Transform transform, bool includeSelf)
    {
        var list = new List<Transform>();
        if (includeSelf)
        {
            list.Add(transform);
        }
        transform.AddChildrenToList(list);
        return list;
    }

    private static void CloneChildrenTo(
        this Transform sourceTransform,
        Transform targetParent,
        List<Transform> list
    )
    {
        for (var i = 0; i < sourceTransform.childCount; i++)
        {
            var sourceChild = sourceTransform.GetChild(i);
            list.Add(sourceChild);
            var targetChild = GameObjectHelpers.Create(
                sourceChild.name,
                sourceChild.localPosition,
                sourceChild.localRotation,
                sourceChild.localScale,
                targetParent,
                useWorldSpace: false
            );
            sourceChild.CloneChildrenTo(targetChild.transform, list);
        }
    }

    public static List<Transform> CloneChildrenTo(
        this Transform sourceTransform,
        bool includeSelf,
        Transform targetParent
    )
    {
        var target = targetParent;
        var list = new List<Transform>();
        if (includeSelf)
        {
            list.Add(sourceTransform);
            target = GameObjectHelpers
                .Create(
                    sourceTransform.name,
                    sourceTransform.localPosition,
                    sourceTransform.localRotation,
                    sourceTransform.localScale,
                    targetParent,
                    useWorldSpace: false
                )
                .transform;
        }
        sourceTransform.CloneChildrenTo(target, list);
        return list;
    }
}