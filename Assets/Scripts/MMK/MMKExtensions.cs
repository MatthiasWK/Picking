using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using MiddleVR_Unity3D;

public static class MMKExtensions
{
    #region Clustering

    /// <summary>
    /// Determines for a clustered setup whether the current instance is the master instance of the cluster.
    /// Defaults to <code>true</code> when in Editor mode.
    /// </summary>
    /// <returns></returns>
    public static bool IsMasterOrEditor()
    {
        if (Application.isEditor)
            return true;
        return MiddleVR.VRClusterMgr.IsServer();

        /** Unity-internal ClusterNetwork has been disabled since we switched to MiddleVR
        #if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                return ClusterNetwork.IsMasterOfCluster();
        #else
                return ClusterNetwork.isMasterOfCluster;
        #endif
        */
    }

    /// <summary>
    /// This is a safe way to update the world position of an object that may or may not be part of the
    /// MiddleVR VRSystem object tree. If it belongs to the objects managed by MiddleVR, we have to use
    /// their own functions to update the position
    /// </summary>
    public static void SetPositionClusterSafe(this Transform obj, Vector3 unityPosition) {
        var node = MVRNodesMapper.GetInstance().GetNode(obj.gameObject);
        if (node == null) {
            obj.position = unityPosition;
        } else {
            node.SetPositionWorld(MiddleVRTools.FromUnity(unityPosition));
        }
    }

    public static void RotateAroundClusterSafe(this Transform obj, Vector3 fixPoint, Vector3 axis, float angle) {
        // perform the rotation once so we know the final rotation
        obj.RotateAround(fixPoint, axis, angle);
        var node = MVRNodesMapper.GetInstance().GetNode(obj.gameObject);
        if (node != null) {
            node.SetOrientationWorld(MiddleVRTools.FromUnity(obj.rotation));
        }
    }

    #endregion

    /// <summary>
    /// Extracts all parameters behind the given command line argument identifier and returns as array.
    /// Example command line: <code>Test.exe -config cfg.xml -ql Fast</code>
    /// Example return for GetCmdArguments("-config"):
    ///     ["cfg.xml", "-ql", "Fast"]
    /// Example return for GetCmdArguments("-ql"):
    ///     ["Fast"]
    /// </summary>
    /// <param name="arg">exact identifier of the command line argument(s), including possible prefix (dash, slash)</param>
    /// <returns>Array of all command line args behind the given identifier string</returns>
    public static string[] GetCmdArguments(string arg)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        for (int i = 0; i < arguments.Length; i++)
        {
            if (arguments[i] == arg)
            {
                if (i + 1 < arguments.Length)
                {
                    string[] cmdArgs = new string[arguments.Length - (i + 1)];
                    Array.Copy(arguments, i + 1, cmdArgs, 0, arguments.Length - (i + 1)); // Copy all params behind "arg"
                    return cmdArgs;
                }
            }
        }
        // default to null
        return null;
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (/*type != other.GetType() &&*/ !type.IsAssignableFrom(other.GetType())) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        
        return comp as T;
    }

    public static T AddComponentCopy<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    public static void EnableAllBehaviours(this GameObject go)
    {
        foreach (var comp in go.GetComponents(typeof(MonoBehaviour)))
        {
            (comp as MonoBehaviour).enabled = true;
        }
    }

    public static void DisableAllBehaviours(this GameObject go)
    {
        foreach (var comp in go.GetComponents(typeof(MonoBehaviour)))
        {
            (comp as MonoBehaviour).enabled = false;
        }
    }

    /// <summary>
    /// Breadth-first search for child by name
    /// </summary>
    /// <param name="aParent"></param>
    /// <param name="aName"></param>
    /// <returns></returns>
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    public static float LimitToRange(
        this float value, float inclusiveMinimum, float inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }

    public static Vector3 LimitToRange(
        this Vector3 value, Vector3 inclusiveMinimum, Vector3 inclusiveMaximum)
    {
        if (value.x < inclusiveMinimum.x) { value.x = inclusiveMinimum.x; }
        if (value.x > inclusiveMaximum.x) { value.x = inclusiveMaximum.x; }
        if (value.y < inclusiveMinimum.y) { value.y = inclusiveMinimum.y; }
        if (value.y > inclusiveMaximum.y) { value.y = inclusiveMaximum.y; }
        if (value.z < inclusiveMinimum.z) { value.z = inclusiveMinimum.z; }
        if (value.z > inclusiveMaximum.z) { value.z = inclusiveMaximum.z; }
        return value;
    }

    /// <summary>
    /// Differs from the Unity-included casting from Vector3 to Vector2 in that
    /// it discards the y-component and not the z-component. 
    /// I.e.: Vector3(x,y,z) => Vector2(x,z)
    /// </summary>
    public static Vector2 ToVector2(this Vector3 value)
    {
        return new Vector2(value.x, value.z);
    }

    /// <summary>
    /// Differs from the Unity-included casting from Vector2 to Vector3 in that
    /// it sets the y-component to zero and not the z-component. 
    /// I.e.: Vector2(x,y) => Vector3(x,0,y)
    /// </summary>
    public static Vector3 ToVector3(this Vector2 value)
    {
        return new Vector3(value.x, 0f, value.y);
    }

    public static Vector2 IntersectionPoint(float p1x1, float p1y1, float p1x2, float p1y2, float p2x1, float p2y1, float p2x2, float p2y2)
    {
        float A1 = p1y2 - p1y1;
        float B1 = p1x1 - p1x2;
        float C1 = A1 * p1x1 + B1 * p1y1;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = p2y2 - p2y1;
        float B2 = p2x1 - p2x2;
        float C2 = A2 * p2x1 + B2 * p2y1;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            throw new ArgumentException("Lines are parallel");

        // now return the Vector2 intersection point
        return new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );
    }

    public static Vector2 IntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            throw new ArgumentException("Lines are parallel");

        // now return the Vector2 intersection point
        return new Vector2(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta
        );
    }
    
    public static int ClosestSegmentStartPoint(Vector3 refPoint, List<Transform> polygonPoints, int startingIndex)
    {
        if (polygonPoints.IsNullOrEmpty())
            return startingIndex;
        startingIndex = (int)Mathf.Clamp(startingIndex, 0f, polygonPoints.Count - 1f);
        var currentMinDistance = float.MaxValue;
        var currentMinStartIndex = startingIndex;
        var refPoint2 = refPoint.ToVector2();

        // Walk through all line segments, starting from given start index. Keep going as long as new distance is shorter than last. After X fails, stop algorithm.
        var failCount = 0;
        for (int i = 0; i < polygonPoints.Count; i++)
        {
            var currentSource = polygonPoints[(startingIndex + i) % polygonPoints.Count].position;
            var currentTarget = polygonPoints[(startingIndex + i + 1) % polygonPoints.Count].position;
            var dist = Mathf.Abs(DistanceLineToPoint(currentSource.ToVector2(), currentTarget.ToVector2(), refPoint2, true));
            if (dist <= currentMinDistance)
            {
                currentMinDistance = dist;
                currentMinStartIndex = (startingIndex + i) % polygonPoints.Count;
                failCount = 0;
                continue;
            } else
            {
                failCount++;
            }
            if (failCount > 2)
                break;
        }
        return currentMinStartIndex;
    }

    /// <summary>
    /// Compute distance from line (or segment) AB to point C. 
    /// </summary>
    /// <param name="isSegment">if <code>true</code>, only the segment between A and B is considered (not the whole line)</param>
    /// <returns></returns>
    public static float DistanceLineToPoint(Vector2 lineA, Vector2 lineB, Vector2 pointC, bool isSegment)
    {
        var AB = lineB - lineA;
        var AC = pointC - lineA;
        float dist = AB.Cross(AC) / Vector2.Distance(lineA, lineB);
        if (isSegment)
        {
            var BC = pointC - lineB;
            if (Vector2.Dot(BC,AB) > 0)
                return Vector2.Distance(lineB, pointC);
            if (Vector2.Dot(AC, lineA - lineB) > 0)
                return Vector2.Distance(lineA, pointC);
        }
        return dist;
    }

    public static bool IsBetweenIncl<T>(this T item, T start, T end)
    {
        return Comparer<T>.Default.Compare(item, start) >= 0
            && Comparer<T>.Default.Compare(item, end) <= 0;
    }

    public static bool IsBetweenExcl<T>(this T item, T start, T end)
    {
        return Comparer<T>.Default.Compare(item, start) > 0
            && Comparer<T>.Default.Compare(item, end) < 0;
    }

    /// <summary>
    /// Converts from right-handed to left-handed coordinate system (and vice-versa) by flipping the Z-component
    /// </summary>
    /// <param name="source">Input Vector</param>
    /// <returns>Same Vector in the opposite coordinate system</returns>
    public static Vector3 ConvertCoordinateSystem(this Vector3 source)
    {
        source.z = -source.z;
        return source;
    }

    public static float Cross (this Vector2 vecA, Vector2 vecB)
    {
        return vecA.x * vecB.y - vecA.y * vecB.x;
    }

    public static Quaternion ConvertCoordinateSystem(this Quaternion source)
    {
        var angle = 0f;
        var axis = Vector3.zero;
        source.ToAngleAxis(out angle, out axis);
        //axis.z = -axis.z;
        axis.x = -axis.x;
        axis.y = -axis.y;
        return Quaternion.AngleAxis(angle, axis);
    }

    public static string ToString<T>(this IEnumerable<T> list, string separator)
    {
        if (list == null)
            return string.Empty;
        return "[" + string.Join(separator, list.Select(i => i.ToString()).ToArray()) + "]";
    }

    public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string separator)
    {
        if (dictionary == null)
            return string.Empty;
        return "{" + string.Join(separator, dictionary.Select(kv => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + "}";
    }

    public static bool EqualsTransformsOrder(this RaycastHit[] newHits, RaycastHit[] oldHits)
    {
        if (oldHits == null)
            return false;
        if (newHits.Length != oldHits.Length)
            return false;
        for (int i=0; i < newHits.Length; i++)
        {
            if (!newHits[i].transform.GetHashCode().Equals(oldHits[i].transform.GetHashCode()))
                return false;
        }
        return true;
    }

    public static int IndexOf<T>(this T[] array, T element)
    {
        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(element))
                return i;
        return -1;
    }

    public static void Fill<T>(this T[] originalArray, T with)
    {
        for (int i = 0; i < originalArray.Length; i++)
        {
            originalArray[i] = with;
        }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (action == null) throw new ArgumentNullException("action");

        foreach (T item in source)
        {
            action(item);
        }
    }

    public static readonly DateTime UnixTimeStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static double ToUnixMilliseconds(this DateTime date)
    {
        return date.ToUniversalTime().Subtract(UnixTimeStart).TotalMilliseconds;
    }

    public static DateTime FromUnixTime(this long unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(unixTime);
    }

    public static long ToUnixTime(this DateTime date)
    {
        return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
    }


    public static List<GameObject> FindGameObjectsWithLayer(int layer) {
        var goArray = GameObject.FindObjectsOfType<GameObject>();
        return goArray.Where(d => d.layer == layer).ToList();
    }

    public static void SetLayerRecursively(this GameObject obj, int newLayer) {
        if (null == obj) {
            return;
        }
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) {
            if (null == child) {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    /// <summary>
    /// Determines whether the collection is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The IEnumerable type.</typeparam>
    /// <param name="enumerable">The enumerable, which may be null or empty.</param>
    /// <returns>
    ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) {
        if (enumerable == null) {
            return true;
        }
        /* If this is a list, use the Count property for efficiency. 
         * The Count property is O(1) while IEnumerable.Count() is O(N). */
        var collection = enumerable as ICollection<T>;
        if (collection != null) {
            return collection.Count < 1;
        }
        return !enumerable.Any();
    }
}