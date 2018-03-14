using MMK.Inp;
using UnityEngine;
using System.Runtime.InteropServices;

public static class VRPN {
	[DllImport ("unityVrpn")]
	private static extern double vrpnAnalogExtern(string address, int channel, int frameCount);

    [DllImport("unityVrpn")]
	private static extern bool vrpnButtonExtern(string address, int channel, int frameCount);

    [DllImport("unityVrpn")]
	private static extern double vrpnTrackerExtern(string address, int channel, int component, int frameCount);

    const float AnalogRotationSimMultiplier = 180f;
    static readonly Vector3 AnalogRotationSimValueOffset = new Vector3(0.5f, 1f, 1f);

    public static double vrpnAnalog(string address, int channel){
		return vrpnAnalogExtern (address, channel, Time.frameCount);
	}
	
	public static bool vrpnButton(string address, int channel){
		return vrpnButtonExtern (address, channel, Time.frameCount);	
	}
	
	public static Vector3 vrpnTrackerPos(string address, int channel){
		return new Vector3(
			(float) vrpnTrackerExtern(address, channel, 0, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 1, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 2, Time.frameCount));
	}

    public static Quaternion vrpnTrackerQuat(string address, int channel) {
		return new Quaternion(
			(float) vrpnTrackerExtern(address, channel, 3, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 4, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 5, Time.frameCount),
			(float) vrpnTrackerExtern(address, channel, 6, Time.frameCount));
    }

    public static Vector3 vrpnTrackerPos(MMKClusterInput input)
    {
        return new Vector3(
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 0, Time.frameCount),
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 1, Time.frameCount),
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 2, Time.frameCount));
    }

    public static Quaternion vrpnTrackerQuat(MMKClusterInput input)
    {
        return new Quaternion(
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 3, Time.frameCount),
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 4, Time.frameCount),
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 5, Time.frameCount),
            (float)vrpnTrackerExtern(input.DeviceUrl(), input.Index, 6, Time.frameCount));
    }

    public static double vrpnAnalog(MMKClusterInput input)
    {
        return vrpnAnalogExtern(input.DeviceUrl(), input.Index, Time.frameCount);
    }

    public static bool vrpnButton(MMKClusterInput input)
    {
        return vrpnButtonExtern(input.DeviceUrl(), input.Index, Time.frameCount);
    }

    public static Vector3 vrpnSimulateTrackerPosFromAnalog(string address, int? channelX, int? channelY, int? channelZ)
    {
        return new Vector3(
            channelX.HasValue ? (float)vrpnAnalog(address, channelX.GetValueOrDefault()) : 0f,
            channelY.HasValue ? (float)vrpnAnalog(address, channelY.GetValueOrDefault()) : 0f,
            channelZ.HasValue ? (float)vrpnAnalog(address, channelZ.GetValueOrDefault()) : 0f);
    }

    public static Quaternion vrpnSimulateTrackerQuatFromAnalog(string address, int? channelX, int? channelY, int? channelZ)
    {
        return Quaternion.Euler(
            channelX.HasValue ? ((float)vrpnAnalog(address, channelX.GetValueOrDefault()) + AnalogRotationSimValueOffset.x) * AnalogRotationSimMultiplier : 0f,
            channelY.HasValue ? ((float)vrpnAnalog(address, channelY.GetValueOrDefault()) + AnalogRotationSimValueOffset.y) * AnalogRotationSimMultiplier : 0f,
            channelZ.HasValue ? ((float)vrpnAnalog(address, channelZ.GetValueOrDefault()) + AnalogRotationSimValueOffset.z) * AnalogRotationSimMultiplier : 0f);
    }
}
