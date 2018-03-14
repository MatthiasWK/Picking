using UnityEngine;
using System.Collections.Generic;
using MiddleVR_Unity3D;

namespace MMK.Inp
{
    #region INPUT_ENUMS

    /// <summary>
    /// Determines whether values are delivered normalized [0;1] or zero-centered [-1;1]
    /// </summary>
    public enum AxisValueTypes { Normalized, ZeroCentered }
    /// <summary>
    /// Determines whether a value conversion between different <see cref="AxisValueTypes"/> should take place
    /// </summary>
    public enum AxisValueConversions { None, NormalizedToZeroCentered, ZeroCenteredToNormalized }
    /// <summary>
    /// Determines whether an additional function should be applied to the input values
    /// </summary>
    public enum AxisMapFunctions { None, Quadratic }
    /// <summary>
    /// Used to distinguish between Button Down - Pressed - Up events in some functions
    /// </summary>
    public enum ButtonValueTypes { Pressed, Down, Up }

    #endregion


    /// <summary>
    /// A wrapper class for Unity's <code>ClusterInput</code>, providing additional features to define devices
    /// </summary>
    public class MMKClusterInputManager : MonoBehaviour
    {
        const int DefaultIndex = 0;
        const bool ButtonDefaultValue = false;
        const float AxisDefaultValue = 0f;

        public static IDictionary<string, MMKClusterInput> RegisteredVRPNInputs { get; private set; }
        public static IDictionary<string, MiddleVRInput> RegisteredMVRInputs { get; private set; }

        static readonly Vector3 DefaultTrackerPos = new Vector3(0f, 1f, 0f);
        static MMKConfig Config;

        #region PUBLIC_INTERFACE
         
        public static Vector3 GetTrackerPosition(string inputName) {
            var name = inputName;
            var index = DefaultIndex;
            DetermineMVRInput(ref name, ref index);
            var tracker = MiddleVR.VRDeviceMgr.GetTracker(name);
            if (tracker != null) {
                return MVRTools.ToUnity(tracker.GetPosition());
            }
            return DefaultTrackerPos;
        }

        public static Quaternion GetTrackerRotation(string inputName) {
            var name = inputName;
            var index = DefaultIndex;
            DetermineMVRInput(ref name, ref index);
            var tracker = MiddleVR.VRDeviceMgr.GetTracker(name);
            if (tracker != null) {
                return MVRTools.ToUnity(tracker.GetOrientation());
            }
            return Quaternion.identity;
        }

        public static bool GetButton(string inputName, int index) {
            var buttons = MiddleVR.VRDeviceMgr.GetButtons(inputName);
            if (buttons == null || buttons.GetButtonsNb() <= index) return ButtonDefaultValue;
            return buttons.IsPressed((uint)index);
        }

        public static bool GetButtonDown(string inputName, int index) {
            var buttons = MiddleVR.VRDeviceMgr.GetButtons(inputName);
            if (buttons == null || buttons.GetButtonsNb() <= index) return ButtonDefaultValue;
            return buttons.IsToggled((uint)index, true);
        }

        public static bool GetButtonUp(string inputName, int index) {
            var buttons = MiddleVR.VRDeviceMgr.GetButtons(inputName);
            if (buttons == null || buttons.GetButtonsNb() <= index) return ButtonDefaultValue;
            return buttons.IsToggled((uint)index, false);
        }

        public static float GetAxis(string inputName, int index) {
            var axis = MiddleVR.VRDeviceMgr.GetAxis(inputName);
            if (axis == null) return AxisDefaultValue;
            return axis.GetValue((uint)index);
        }

        public static bool Exists(string inputName) {
            if (RegisteredMVRInputs.ContainsKey(inputName))
                inputName = RegisteredMVRInputs[inputName].MVRName;
            return MiddleVR.VRDeviceMgr.GetDevice(inputName) != null;
        }

        //// BACKWARDS-COMPATIBILITY FUNCTIONS ////
        public static bool GetButton(string inputName) {
            var name = inputName;
            var index = DefaultIndex;
            DetermineMVRInput(ref name, ref index);
            return GetButton(name, index);
        }

        public static bool GetButtonDown(string inputName) {
            var name = inputName;
            var index = DefaultIndex;
            DetermineMVRInput(ref name, ref index);
            return GetButtonDown(name, index);
        }

        public static bool GetButtonUp(string inputName) {
            var name = inputName;
            var index = DefaultIndex;
            DetermineMVRInput(ref name, ref index);
            return GetButtonUp(name, index);
        }

        public static float GetAxis(string inputName) {
            if (RegisteredMVRInputs.ContainsKey(inputName)) {
                var input = RegisteredMVRInputs[inputName];
                var value = GetAxis(input.MVRName, input.MVRIndex);
                // Apply (possible) Axis Value Corrections
                value = GetCorrectedAxisValue(value, input);
                return value;
            } else if (RegisteredVRPNInputs.ContainsKey(inputName)) {
                var input = RegisteredVRPNInputs[inputName];
                var value = (float)VRPN.vrpnAnalog(input);
                // Apply (possible) Axis Value Corrections
                value = GetCorrectedAxisValue(value, input);
                return value;
            }
            return GetAxis(inputName, DefaultIndex);
        }

        /// <summary>
        /// Will first check local device buttons via regular Input API if we are in Editor; in all other cases,
        /// use regular VRPN input functions.
        /// </summary>
        public static bool TryGetButton(ButtonValueTypes type, KeyCode preferredInput, string alternativeInput) {
            if (Application.isEditor) {
                switch (type) {
                    case ButtonValueTypes.Down:
                        return Input.GetKeyDown(preferredInput);
                    case ButtonValueTypes.Up:
                        return Input.GetKeyUp(preferredInput);
                    case ButtonValueTypes.Pressed:
                    default:
                        return Input.GetKey(preferredInput); // || GetButton(altName, altIndex);

                }
            }
            // Use synchronized input (from MiddleVR) only for real builds
            var altName = alternativeInput;
            var altIndex = DefaultIndex;
            DetermineMVRInput(ref altName, ref altIndex);
            switch (type) {
                case ButtonValueTypes.Down:
                    return GetButtonDown(altName, altIndex);
                case ButtonValueTypes.Up:
                    return GetButtonUp(altName, altIndex);
                case ButtonValueTypes.Pressed:
                default:
                    return GetButton(altName, altIndex);
            }
        }

        #endregion

        #region VALUE_CONVERSIONS

        static float GetCorrectedAxisValue(float value, MiddleVRInput input) {
            if (input.InvertDirection)
                value = InvertAxisValue(value, input.AxisValueType);
            value = ApplyAxisValueConversion(value, input.AxisValueConversion);
            if (input.AxisMapFunction != AxisMapFunctions.None)
                value = ApplyAxisFunction(value, input.AxisMapFunction);
            return value;
        }
        static float GetCorrectedAxisValue(float value, MMKClusterInput input) {
            if (input.InvertDirection)
                value = InvertAxisValue(value, input.AxisValueType);
            value = ApplyAxisValueConversion(value, input.AxisValueConversion);
            if (input.AxisMapFunction != AxisMapFunctions.None)
                value = ApplyAxisFunction(value, input.AxisMapFunction);
            return value;
        }

        static float InvertAxisValue(float value, AxisValueTypes type) {
            if (type == AxisValueTypes.Normalized)
                return Mathf.Abs(value - 1f);
            else if (type == AxisValueTypes.ZeroCentered)
                return -1f * value;
            return value;
        }

        static float ApplyAxisValueConversion(float value, AxisValueConversions convType) {
            if (convType == AxisValueConversions.None)
                return value;
            else if (convType == AxisValueConversions.NormalizedToZeroCentered)
                return (value - 0.5f) * 2f;
            else if (convType == AxisValueConversions.ZeroCenteredToNormalized)
                return (value / 2f) + 0.5f;
            return value;
        }

        static float ApplyAxisFunction(float value, AxisMapFunctions func) {
            switch (func) {
                case AxisMapFunctions.Quadratic:
                    value = ((value < 0f) ? -1 : 1) * value * value;
                    break;
            }
            return value;
        }

        #endregion

        static void DetermineMVRInput(ref string name, ref int index) {
            if (RegisteredMVRInputs.ContainsKey(name)) {
                index = RegisteredMVRInputs[name].MVRIndex;
                name = RegisteredMVRInputs[name].MVRName;
            }
        }

        void Awake() {
            Config = MMKConfig.GetInstance();
            RegisteredMVRInputs = new Dictionary<string, MiddleVRInput>(Config.MVRInputs.Count);
            foreach (var input in Config.MVRInputs) {
                if (!RegisteredMVRInputs.ContainsKey(input.Name)) {
                    RegisteredMVRInputs.Add(input.Name, input);
                }
            }
            // For using VRPN devices directly without MiddleVR:
            RegisteredVRPNInputs = new Dictionary<string, MMKClusterInput>(Config.ClusterInputs.Count);
            foreach (var input in Config.ClusterInputs) {
                if (!RegisteredVRPNInputs.ContainsKey(input.Name)) {
                    RegisteredVRPNInputs.Add(input.Name, input);
                }
            }
        }
    }
}