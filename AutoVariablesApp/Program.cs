using System;
using Generated.Units;

namespace AutoVariablesApp {
internal abstract class Program {
	private static void Main() {
		Position3 pos = new(1f, 2f, 3f);
		Velocity3 vel = new(1, 2, 3);
		Accel3 accel = new(1, 5, 2);
		Accel a = new(1);

		pos = pos + vel;
		vel = vel + accel;

		Position distance = Position.Millimeter(62_865);
		float km = distance.ToKilometer();
	}
}
}
