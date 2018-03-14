using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

namespace MMK.Inp
{
    [System.Serializable]
    public class MMKClusterInput
    {
        public string Name;
        public string DeviceName;
        public string ServerUrl;
        public int Index;
        public ClusterInputType Type;

        // AXIS&BUTTON values:
        [XmlIgnore]
        public float InitialValue; // needed to compensate a VRPN bug (before first real input, random values are returned by VRPN)
        [XmlIgnore]
        public bool HasBeenInit = false; // true after InitialValue has been set
        [XmlIgnore]
        public bool HasBeenUsed = false; // true after the first time that initialValue has changed

        // AXIS only values:
        public AxisValueTypes AxisValueType;
        public AxisValueConversions AxisValueConversion;
        public AxisMapFunctions AxisMapFunction;
        public bool InvertDirection;

        // TRACKER only values:
        public Vector3 TrackerPosOffset = Vector3.zero;
        [XmlIgnore]
        public Quaternion TrackerRotOffsetQuat { get; private set; }
        public Vector3 TrackerRotOffset
        {
            get { return TrackerRotOffsetQuat.eulerAngles; }
            set { TrackerRotOffsetQuat = Quaternion.Euler(value); }
        }

        public MMKClusterInput() {
            TrackerRotOffset = Vector3.zero;
            TrackerRotOffsetQuat = Quaternion.identity;
            AxisMapFunction = AxisMapFunctions.None;
        }

        [OnDeserialized()]
        internal void OnDeserializeMethod(StreamingContext context) {
            TrackerRotOffsetQuat = Quaternion.Euler(TrackerRotOffset);
        }

        public string DeviceUrl() {
            return string.Format("{0}@{1}", DeviceName, ServerUrl);
        }
    }
}