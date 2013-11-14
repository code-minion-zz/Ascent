using System;
using System.Collections;
using System.Collections.Generic;


namespace InControl
{
	[AutoDiscover]
	public class KeyboardProfile : UnityInputDeviceProfile
	{
		public KeyboardProfile()
		{
			Name = "Keyboard";
			Meta = "";

			SupportedPlatforms = new[]
			{
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			DeadZone = 0.0f;

			ButtonMappings = new[]
			{
				new InputControlButtonMapping()
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = "q"
				},
				new InputControlButtonMapping()
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = "w"
				},
				new InputControlButtonMapping()
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = "a"
				},
				new InputControlButtonMapping()
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = "r"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = "s"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = "d"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = "f"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = "e"
				},
				new InputControlButtonMapping()
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = "g"
				},
				new InputControlButtonMapping()
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = "t"
				},
				new InputControlButtonMapping()
				{
					Handle = "Back",
					Target = InputControlType.Select,
					Source = "'"
				},
				new InputControlButtonMapping()
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = "return"
				}
			};

			AnalogMappings = new InputControlAnalogMapping[]
			{
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys X",
					Target = InputControlType.LeftStickX,
					Source = "left right"
				},
				new InputControlAnalogMapping()
				{
					Handle = "Arrow Keys Y",
					Target = InputControlType.LeftStickY,
					Source = "down up"
				}
			};
		}
	}
}

