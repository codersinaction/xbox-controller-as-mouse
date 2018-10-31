using System.Threading;
using WindowsInput;
using SharpDX.XInput;

namespace XBoxAsMouse
{
	public class XBoxControllerAsMouse
	{
		private const int MovementDivider = 2_000;
		private const int ScrollDivider = 10_000;
		private const int RefreshRate = 60;

		private Timer _timer;
		private Controller _controller;
		private IMouseSimulator _mouseSimulator;

		private bool _wasADown;
		private bool _wasBDown;

		public XBoxControllerAsMouse()
		{
			_controller = new Controller(UserIndex.One);
			_mouseSimulator = new InputSimulator().Mouse;
			_timer = new Timer(obj => Update());
		}

		public void Start()
		{
			_timer.Change(0, 1000 / RefreshRate);
		}

		private void Update()
		{
			_controller.GetState(out var state);
			Movement(state);
			Scroll(state);
			LeftButton(state);
			RightButton(state);
		}

		private void RightButton(State state)
		{
			var isBDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
			if (isBDown && !_wasBDown) _mouseSimulator.RightButtonDown();
			if (!isBDown && _wasBDown) _mouseSimulator.RightButtonUp();
			_wasBDown = isBDown;
		}

		private void LeftButton(State state)
		{
			var isADown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
			if (isADown && !_wasADown) _mouseSimulator.LeftButtonDown();
			if (!isADown && _wasADown) _mouseSimulator.LeftButtonUp();
			_wasADown = isADown;
		}

		private void Scroll(State state)
		{
			var x = state.Gamepad.RightThumbX / ScrollDivider;
			var y = state.Gamepad.RightThumbY / ScrollDivider;
			_mouseSimulator.HorizontalScroll(x);
			_mouseSimulator.VerticalScroll(y);
		}

		private void Movement(State state)
		{
			var x = state.Gamepad.LeftThumbX / MovementDivider;
			var y = state.Gamepad.LeftThumbY / MovementDivider;
			_mouseSimulator.MoveMouseBy(x, -y);
		}
	}
}