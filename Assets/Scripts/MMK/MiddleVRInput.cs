using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMK.Inp
{
    [System.Serializable]
    public class MiddleVRInput
    {
        [System.Serializable]
        public enum InputType { Button, Axis, Tracker }

        /// <summary>
        /// Internal/custom name of the input. This should be the same as is used in custom (MMK) scripts when querying inputs.
        /// </summary>
        public string Name;
        /// <summary>
        /// Specifies whether this input represents a single button, a single axis or a single tracker.
        /// </summary>
        public InputType Type;
        /// <summary>
        /// The name of the input as it is used by MiddleVR (thus, must correspond to the used MiddleVR config file).
        /// </summary>
        public string MVRName;
        /// <summary> 
        /// The input index of the specific button/axis/tracker as it is used by MiddleVR.
        /// </summary>
        public int MVRIndex;

        // AXIS only values:
        public AxisValueTypes AxisValueType;
        public AxisValueConversions AxisValueConversion;
        public AxisMapFunctions AxisMapFunction;
        public bool InvertDirection;
    }
}