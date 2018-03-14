using MMK.Inp;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class MMKConfig
{
    public static string ConfigFilename = "MMK_config.xml";
    public string ApplicationName = string.Empty; // Use this to manually set the name of the applications window title (which is used to manipulate window pos/res)
    public string UserID = "0"; // Should be used for user studies etc. where we need to log data for different sessions
    public string QualityLevel = "MMKCave";
    public Vector3 CaveStartPosition = Vector3.zero; // If cave object should have an initial (local) offset. Useful in MMK driving simulator to correct the car's position within the cave
    public bool ForceVerticalBars = false;
    public bool OverrideMasterNodeIntegrated = false;
    public bool MasterNodeIntegrated = false;
    public bool MasterCameraForceIndex = false;
    public string MasterCameraNodeIndex = "8";
    public float EyeDistance = 0.06f;
    public float BodyHeight = 1.80f;
    public int DebugLevel = 0; // Currently: 0 = nothing, 1 = Show VRPN Tracker Data
    public int LookAxes = 0; // 0 = None, 1 = XandY, 2 = X only, 3 = Y only
    public bool EnableClusterInput = true; // true = use Unity-ClusterInput system, false = use MMK solution (unsynchronized!)
    public bool EnableMMKLogger = false; // currently not relevant => each project should take care of its own logging systems
    public List<MMKClusterInput> ClusterInputs = new List<MMKClusterInput>(); // now only for backwards compatibility
    public List<MiddleVRInput> MVRInputs = new List<MiddleVRInput>(); // now only for backwards compatibility


    #region SINGLETON
    static volatile MMKConfig Instance;
    static object SyncRoot = new Object();

    private MMKConfig() {
    }

    public static MMKConfig GetInstance()
    {
        if (Instance == null)
        {
            lock (SyncRoot)
            {
                if (Instance == null) // double-checked locking pattern
                {
                    // READ FROM CONFIG FILE
                    if (System.IO.File.Exists(ConfigFilename))
                    {
                        var serializer = new XmlSerializer(typeof(MMKConfig));
                        using (var reader = XmlReader.Create(ConfigFilename))
                        {
                            Instance = serializer.Deserialize(reader) as MMKConfig;
                        }
                    } else
                    {
                        Instance = new MMKConfig(); // USE DEFAULT VALUES IF CONFIG FILE DOES NOT EXIST
                    }
                }
            }
        }
        return Instance;
    }
    #endregion

    /// <summary>
    /// Read & Parse MMK config file
    /// </summary>
    static MMKConfig()
    {
        var confFile = MMKExtensions.GetCmdArguments("-config");
        if (confFile != null && confFile.Length > 0)
            ConfigFilename = confFile[0];
        // === ENABLE THIS ONCE TO OUTPUT AN EXAMPLE XML-FILE WITH ALL POSSIBLE OPTIONS ===
        //DebugWriteConfigExample();

        GetInstance();
    }

    static void DebugWriteConfigExample()
    {
        var debugConf = new MMKConfig() {
            /*VrpnDevices = new List<VRPNDevice>()
            {
                new VRPNTracker() { Name="HeadTracker", Address="DTrack@dtrack", Channel=0 },
                new VRPNWiiMote() { Name="WiiMote", Address="DTrack@dtrack", Channel=1, ControllerAddress="WiiMote0@dtrack" },
                new VRPNSpaceExplorer() { Name="SpaceExplorer", Address="DTrack@dtrack", Channel=2, ControllerAddress="SpaceEx0@dtrack" },
                new VRPNJoywin32() { Name="Joywin", Address="DTrack@dtrack", Channel=3, ControllerAddress="JoyWin0@dtrack" },
                new VRPNXInput() { Name="Gamepad", Address="DTrack@dtrack", Channel=3, ControllerAddress="XInput0@dtrack" }
            },*/
            /*ClusterInputs = new List<MMKClusterInput>()
            {
                new MMKClusterInput() { Name="HeadTracker", DeviceName="DTrack", ServerUrl="dtrack", Index=0, Type=ClusterInputType.Tracker  },
                new MMKClusterInput() { Name="RightHandWand", DeviceName="DTrack", ServerUrl="dtrack", Index=8, Type=ClusterInputType.Tracker  },
                new MMKClusterInput() { Name="LeftHandWand", DeviceName="DTrack", ServerUrl="dtrack", Index=9, Type=ClusterInputType.Tracker  },
                new MMKClusterInput() { Name="Horizontal", DeviceName="XInput0", ServerUrl="dtrack", Index=0, Type=ClusterInputType.Axis, AxisValueType=AxisValueTypes.ZeroCentered  }, 
                new MMKClusterInput() { Name="Vertical", DeviceName="XInput0", ServerUrl="dtrack", Index=1, Type=ClusterInputType.Axis, AxisValueType=AxisValueTypes.ZeroCentered  },
                new MMKClusterInput() { Name="Jump", DeviceName="XInput0", ServerUrl="dtrack", Index=0, Type=ClusterInputType.Button},
                new MMKClusterInput() { Name="DebugButton", DeviceName="XInput0", ServerUrl="dtrack", Index=1, Type=ClusterInputType.Button}
            }*/
            MVRInputs = new List<MiddleVRInput>() {
                new MiddleVRInput() { Name="Accelerate", Type = MiddleVRInput.InputType.Axis, MVRName="Wheel.Axis", MVRIndex = 0 }
            }
        };
        var serializer = new XmlSerializer(typeof(MMKConfig));
        using (var writer = XmlWriter.Create("MMK_config.example.xml"))
        {
            serializer.Serialize(writer, debugConf);
        }
    }
}
