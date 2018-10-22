using System.Threading;
using WindowsInput;
using SharpDX.XInput;

namespace XBoxAsMouse
{
	internal class ControllerToMouseProxy
	{
		private readonly IMouseSimulator _mouseSimulator;
		private readonly Controller _controller;
		private readonly Timer _timer;

		private bool _wasDownA;
		private bool _wasDownB;

		public ControllerToMouseProxy()
		{
			_controller = new Controller(UserIndex.One);
			_timer = new Timer(Loop);
			_mouseSimulator = new InputSimulator().Mouse;
		}

		public void Start()
		{
			_timer.Change(0, 1000 / 60);
		}

		private void Loop(object obj)
		{
			_controller.GetState(out var state);
			Movement(state);
			Scroll(state);
			LeftMouseButton(state);
			RightMouseButton(state);
		}

		private void Scroll(State state)
		{
			var x = state.Gamepad.RightThumbX / 10_000;
			var y = state.Gamepad.RightThumbY / 10_000;
			_mouseSimulator.HorizontalScroll(x);
			_mouseSimulator.VerticalScroll(y);
		}

		private void Movement(State state)
		{
			var x = state.Gamepad.LeftThumbX / 2000;
			var y = state.Gamepad.LeftThumbY / 2000;
			_mouseSimulator.MoveMouseBy(x, -y);
		}

		private void LeftMouseButton(State state)
		{
			var isDownA = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
			if (isDownA && !_wasDownA) _mouseSimulator.LeftButtonDown();
			if (!isDownA && _wasDownA) _mouseSimulator.LeftButtonUp();
			_wasDownA = isDownA;
		}

		private void RightMouseButton(State state)
		{
			var isDownB = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
			if (isDownB && !_wasDownB) _mouseSimulator.RightButtonDown();
			if (!isDownB && _wasDownB) _mouseSimulator.RightButtonUp();
			_wasDownB = isDownB;
		}
	}
}