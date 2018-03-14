<?xml version="1.0" encoding="UTF-8"?>
<MiddleVR>
	<Kernel LogLevel="2" LogInSimulationFolder="0" EnableCrashHandler="0" Version="1.7.0.7" />
	<DeviceManager>
		<Driver Type="vrDriverDirectInput" />
		<Wand Name="PrimaryWand" Driver="0" Axis="Mouse.Axis" HorizontalAxis="0" HorizontalAxisScale="5" VerticalAxis="1" VerticalAxisScale="-5" AxisDeadZone="0" Buttons="Keyboard.Keys" Button0="2" Button1="3" Button2="4" Button3="5" Button4="6" Button5="7" />
	</DeviceManager>
	<DisplayManager Fullscreen="0" AlwaysOnTop="1" WindowBorders="0" ShowMouseCursor="1" VSync="0" GraphicsRenderer="2" AntiAliasing="0" ForceHideTaskbar="0" SaveRenderTarget="0" ChangeWorldScale="0" WorldScale="1">
		<Node3D Name="VRSystemCenterNode" Tag="VRSystemCenter" Parent="None" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Node3D Name="VRSystemOffset" Parent="VRSystemCenterNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,-1.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Node3D Name="Screens" Parent="VRSystemOffset" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,1.087500" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Screen Name="FrontScreen" Parent="Screens" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,2.720000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" Width="3.8666" Height="2.175" />
		<Camera Name="ObserverCam" Parent="VRSystemOffset" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="1.800000,4.450000,1.836000" OrientationLocal="-0.044943,-0.167731,0.951251,0.254887" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="1" AspectRatio="1.6" />
		<Node3D Name="HeadNode" Tag="Head" Parent="VRSystemOffset" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,1.600000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Node3D Name="EyeNode" Parent="HeadNode" Tracker="TrackerSimulatorKeyboard0.Tracker" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" UseTrackerX="0" UseTrackerY="0" UseTrackerZ="0" UseTrackerYaw="1" UseTrackerPitch="1" UseTrackerRoll="1" />
		<Node3D Name="LeftEyeNode" Parent="EyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="-0.030000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Camera Name="Cam_Front_L" Parent="LeftEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" VerticalFOV="60" Near="0.1" Far="1000" Screen="FrontScreen" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.77775" />
		<Camera Name="Cam_Left_L" Parent="LeftEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.573577,0.819152" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.33333" />
		<Camera Name="Cam_Right_L" Parent="LeftEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,-0.573577,0.819152" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.33333" />
		<Camera Name="Cam_Floor_L" Parent="LeftEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="-0.707107,0.000000,0.000000,0.707107" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.59172" />
		<Node3D Name="RightEyeNode" Parent="EyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.030000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Camera Name="Cam_Front_R" Parent="RightEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" VerticalFOV="60" Near="0.1" Far="1000" Screen="FrontScreen" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.77775" />
		<Camera Name="Cam_Left_R" Parent="RightEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.573577,0.819152" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.33333" />
		<Camera Name="Cam_Right_R" Parent="RightEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,-0.573577,0.819152" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.33333" />
		<Camera Name="Cam_Floor_R" Parent="RightEyeNode" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" VerticalFOV="60" Near="0.1" Far="1000" Screen="0" ScreenDistance="1" UseViewportAspectRatio="0" AspectRatio="1.59172" />
		<Node3D Name="HandNode" Tag="Hand" Parent="VRSystemOffset" Tracker="0" IsFiltered="0" Filter="0" PositionLocal="0.000000,0.000000,0.000000" OrientationLocal="0.000000,0.000000,0.000000,1.000000" />
		<Viewport Name="Front_L" Left="0" Top="0" Width="1920" Height="1080" Camera="Cam_Front_L" Stereo="0" StereoMode="3" CompressSideBySide="0" StereoInvertEyes="0" OculusRiftWarping="0" OffScreen="0" UseHomography="0" />
	</DisplayManager>
	<Scripts>
		<Script Type="TrackerSimulatorKeyboard" Name="TrackerSimulatorKeyboard0" SensitivityX="0.5" SensitivityY="0.5" SensitivityZ="0.5" SensitivityYaw="5" SensitivityPitch="5" SensitivityRoll="5" />
		<Script Type="TrackerSimulatorMouse" Name="TrackerSimulatorMouse0" />
	</Scripts>
	<ClusterManager NVidiaSwapLock="0" DisableVSyncOnServer="0" ForceOpenGLConversion="0" BigBarrier="0" SimulateClusterLag="0" MultiGPUEnabled="0" ImageDistributionMaxPacketSize="8000" ClientConnectionTimeout="60" />
</MiddleVR>
