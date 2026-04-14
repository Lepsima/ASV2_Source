using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ClassGenerator;

[Generator]
public class ClassGenerator : IIncrementalGenerator {
	private static readonly List<(string, List<(string, string, double)>)> unitScales = [
		// Utils
		("Direction", [
			("dir", "Normal", 1),
		]),
		("Magnitude", [
			("mag", "Number", 1),
		]),
		
		// Base units
		("Mass", [
			("mg", "Milligram", 0.000001),
			("g", "Gram", 0.001),
			("kg", "Kilogram", 1),
			("t", "Ton", 1000),
			("kt", "Kiloton", 1000000),
		]),
		("Time", [
			("μs", "Microsecond", 0.000001),
			("ms", "Millisecond", 0.001),
			("s", "Second", 1),
			("m", "Minute", 60),
			("h", "Hour", 3600),
		]),
		
		// Physical units
		("Position", [
			("ft", "Feet", 3.280839895),
			("mi", "Mile", 1609.344),
			("mm", "Millimeter", 0.001),
			("cm", "Centimeter", 0.01),
			("m", "Meter", 1),
			("km", "Kilometer", 1000),
			("Mm", "Megameter", 1000000),
		]),
		("Force", [
			("mN", "Millinewton", 0.001),
			("N", "Newton", 1),
			("kN", "Kilonewton", 1000),
			("MN", "Meganewton", 1000000),
		]),
		("Angle", [
			("deg", "Degree", 1),
			("rad", "Radian", 0.01745329238474369),
			("trn", "Turn", 360),
			("'", "MinuteDegree", 0.016666666666666666),
			("''", "SecondDegree", 0.0002777777777777778),
		]),
		("Torque", [
			("mNm", "MillinewtonMeter", 0.001),
			("Nm", "NewtonMeter", 1),
			("kNm", "KilonewtonMeter", 1000),
			("MNm", "MeganewtonMeter", 1000000),
		]),
	];

	private static readonly Dictionary<string, string> standardUnits = new() {
		{"Direction", "dir"},
		{"Magnitude", "mag"},
		
		{"Mass", "kg"},
		{"Time", "s"},
		
		{"Position", "m"},
		{"Velocity", "m/s"},
		{"Accel", "m/s2"},
		{"Force", "N"},
		{"ForceAccel", "N/s"},
		
		{"Angle", "deg"},
		{"AngleVel", "deg/s"},
		{"AngleAccel", "deg/s2"},
		{"Torque", "Nm"},
		{"TorqueAccel", "Nm/s"}
	};
	
	private static readonly Dictionary<string, string[]> unitInspectorValues = new() {
		{"Position", ["ft", "mi", "cm", "m", "km"] },
		{"Mass", ["g", "kg", "t"]},
		{"Force", ["N", "kN", "MN"]},
		{"Time", ["ms", "s", "m", "h"]},
		{"Velocity", ["m/s", "km/h", "ft/s", "mi/h"]},
		{"Accel", ["m/s2", "km/h2", "ft/s2", "mi/h2"]},
		{"ForceAccel", ["N/s", "kN/s"]},
		{"Angle", ["deg", "rad"]},
		{"AngleVel", ["deg/s", "deg/m", "rad/s", "rad/m"]},
		{"AngleAccel", ["deg/s2", "deg/m2", "rad/s2", "rad/m2"]},
		{"Direction", ["dir"]},
		{"Magnitude", ["mag"]},
	};
	
	private static readonly Dictionary<string, int> unitDimensions = new() {
		{"Position", 3},
		{"Velocity", 3},
		{"Accel", 3},
		{"Force", 3},
		{"ForceAccel", 3},
		{"Angle", 3},
		{"AngleVel", 3},
		{"AngleAccel", 3},
		{"Direction", 3},
		{"Magnitude", 1},
		{"Time", 1},
		{"Mass", 1},
	};

	// How units convert to each other (handles all reorderings and combinations)
	private static readonly List<(string, string, string)> unitConversions = [
		("Position", "Velocity", "Time"),
		("Velocity", "Accel", "Time"),
		
		("Force", "Accel", "Mass"),
		("Force", "ForceAccel", "Time"),
		
		("Angle", "AngleVel", "Time"),
		("AngleVel", "AngleAccel", "Time"),
	];
	
	// How can the user create units (fixed formula)
	private static readonly List<(string, string)> compoundUnitInputs = [
		("Velocity", "Position / Time"),
		("Accel", "Velocity / Time"),
		
		("Accel", "Force * Mass"),
		("ForceAccel", "Force / Time"),
		
		("AngleVel", "Angle / Time"),
		("AngleAccel", "AngleVel / Time"),
	];
	
	private const string NAMESPACE = "Lepsima.ASV";
	
	private static readonly Dictionary<string, Unit> units = new();

	public class Unit {
		public readonly bool isSimple;
		public readonly string name;
		public readonly string unitName;
		public readonly List<(string, string, double)> scales = [];
		public readonly List<(string, bool, string)> conversions = [];
		
		public Unit(string name) {
			this.name = name;
			isSimple = false;
			
			string formula = compoundUnitInputs.FirstOrDefault(t => t.Item1 == name).Item2;
			if (formula == null) return;
			
			string[] parts = formula.Split(' ');
			unitName = units[parts[0]].unitName + units[parts[2]].unitName;
		}
		
		public Unit(string name, List<(string, string, double)> scales) {
			this.name = name;
			AddScales(scales);
			isSimple = true;

			string stdUnit = standardUnits[name];
			string stdUnitName = scales.FirstOrDefault(t => t.Item1 == stdUnit).Item2;
			unitName = stdUnitName;
		}

		public string GetName(int dim) {
			return name + GetVectorFloatDimension(dim);
		}

		public void AddComplexInput(string a, bool isMultiplication, string b) {
			List<(string, string, double)> scalesA = units[a].GetScales();
			List<(string, string, double)> scalesB = units[b].GetScales();

			List<(string, string, double)> newScales = [];

			foreach ((string unitA, string nameA, double valueA) in scalesA) {
				foreach ((string unitB, string nameB, double valueB) in scalesB) {
					double scale = isMultiplication 
						? valueA * valueB 
						: valueA / valueB;

					string symbol = isMultiplication ? "*" : "/";
					string unit = $"{unitA}{symbol}{unitB}";
					string unitFull = nameA.EndsWith(nameB) ? nameA + "2" : $"{nameA}sPer{nameB}";
					
					newScales.Add((unit, unitFull, scale));
				}	
			}
			
			AddScales(newScales);
		}

		public List<(string, string, double)> GetScales() {
			if (isSimple) return scales;

			string siUnit = standardUnits[name];
			return [(siUnit, unitName, 1f)];
		}
		
		public void AddConversion(string a, bool isMultiplication, string b) {
			conversions.Add((a, isMultiplication, b));
		}

		public void AddScales(List<(string, string, double)> scales) {
			this.scales.AddRange(scales);
		}
	}
	
	private static void CreateSimpleUnits() {
		foreach ((string name, List<(string, string, double)> scales) in unitScales) {
			Unit unit = new(name, scales);
			units.Add(name, unit);
		}
		
		HashSet<string> unscaledUnits = [];
		
		foreach ((string name, string a, string b) in unitConversions) {
			unscaledUnits.Add(name);
			unscaledUnits.Add(a);
			unscaledUnits.Add(b);
		}

		foreach (string newUnit in unscaledUnits) {
			Unit unit = new(newUnit);

			if (!units.ContainsKey(newUnit)) {
				units.Add(newUnit, unit);
			}
		}
	}

	private static void CreateCompoundUnits() {
		foreach ((string name, string a, string b) in unitConversions) {
			units[name].AddConversion(a, true, b);
			units[a].AddConversion(name, false, b);
			units[b].AddConversion(name, false, a);
		}
		
		foreach ((string name, string op) in compoundUnitInputs) {
			string[] parts = op.Split(' ');
			bool mult = parts[1] == "*";
			units[name].AddComplexInput(parts[0], mult, parts[2]);
		}
	}
	
	public void Initialize(IncrementalGeneratorInitializationContext context) {
		var structs = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (node, _) => node is StructDeclarationSyntax,
				transform: static (ctx, _) => (StructDeclarationSyntax)ctx.Node)
			.Collect();

		context.RegisterSourceOutput(structs, Generate);
	}

	private static void Generate(SourceProductionContext context, ImmutableArray<StructDeclarationSyntax> structs) {
		CreateSimpleUnits();
		CreateCompoundUnits();
		
		GenerateInterface(context);
		GenerateClasses(context);
	}
	
	private static void GenerateInterface(SourceProductionContext context) {
		string file = """
					  namespace NAMESPACE {
					      public interface IAutoUnit {}
					      public interface IAutoUnit2 {}
					      public interface IAutoUnit3 {}
					      
					      public interface IAutoUnitUI {}
					      public interface IAutoUnitUI2 {}
					      public interface IAutoUnitUI3 {}
					  }
					  """.Replace("NAMESPACE", NAMESPACE);
		
		SourceText source = SourceText.From(file, Encoding.UTF8);
		context.AddSource("ASV/Utils/Interfaces.cs", source);
	}

	private static void GenerateClasses(SourceProductionContext context) {
		foreach (KeyValuePair<string, Unit> keyValuePair in units) {
			Unit unit = keyValuePair.Value;
			int dim = unitDimensions[unit.name];

			for (int i = 0; i < dim; i++) {
				if (i == 0 && unit.name.Equals("Direction")) {
					continue;
				}
				
				SourceText source = GenerateUnitClass(unit, i);
				string name = keyValuePair.Key + GetVectorFloatDimension(i);

				string folder = i switch {
					0 => "Values1D/",
					1 => "Values2D/",
					2 => "Values3D/",
					_ => "ERROR/"
				};
				
				context.AddSource($"ASV/{folder}{name}.cs", source);
			}
			
			SourceText sourceUI = GenerateUnitUIClass(unit, dim);
			context.AddSource($"ASV/ValuesUI/{keyValuePair.Key}UI.cs", sourceUI);
		}
	}
	
	private static SourceText GenerateUnitClass(Unit unit, int d) {
		StringBuilder sb = new();
		
		GenerateUnitData(sb, unit, d);
		GenerateUnitSpecials(sb, unit, d);
		GenerateUnitScales(sb, unit, d);
		GenerateUnitConversions(sb, unit, d);
		
		sb.AppendLine("}");
		sb.AppendLine("}");
		
		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
	
	private static SourceText GenerateUnitUIClass(Unit unit, int d) {
		StringBuilder sb = new();
		
		const string PATTERN0 = """
		                        using System;
		                        using UnityEngine;
		                        
		                        namespace NAMESPACE {
		                        """;
		
		const string PATTERN1 = """
		                        
		                        [System.Serializable]
		                        public struct UNITUI : INTERFACE {
		                            public float x;
		                            public UNITUIType type;
		                            
		                            public UNIT Value => this;
		                            
		                            public UNITUI(float x) {
		                                this.x = x;
		                            }
		                            
		                            public static implicit operator float(UNITUI v) => v.x;
		                            public static implicit operator UNIT(UNITUI v) => new(v.x);
		                            public SPECIAL magnitude => new(x);
		                        }
		                        
		                        """;
		
		const string PATTERN2 = """       
		                        
		                        [System.Serializable]
		                        public struct UNIT2UI : INTERFACE2 {
		                            public float x, y;
		                            public UNITUIType type;
		                            
		                            public UNIT2 Value => this;
		                            
		                            public UNIT2UI(float x, float y) {
		                                this.x = x;
		                                this.y = y;
		                            }
		                            
		                            public UNIT2UI(Vector2 v2) {
		                              x = v2.x;
		                              y = v2.y;
		                            }
		                          
		                            public static implicit operator Vector2(UNIT2UI v) => new(v.x, v.y);
		                            public static implicit operator UNIT2(UNIT2UI v) => new(v.x, v.y);
		                            public SPECIAL magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
		                        }
		                        """;
        
		const string PATTERN3 = """      
		                        
		                        
		                        [System.Serializable]
		                        public struct UNIT3UI : INTERFACE3 {
		                            public float x, y, z;
		                            public UNITUIType type;
		                            
		                            public UNIT3 Value => this;
		                            
		                            public UNIT3UI(float x, float y, float z) {
		                                this.x = x;
		                                this.y = y;
		                                this.z = z;
		                            }
		                        
		                            public UNIT3UI(Vector3 v3) {
		                              x = v3.x;
		                              y = v3.y;
		                              z = v3.z;
		                            }
		                        
		                            public static implicit operator Vector3(UNIT3UI v) => new(v.x, v.y, v.z);
		                            public static implicit operator UNIT3(UNIT3UI v) => new(v.x, v.y, v.z);
		                            public SPECIAL magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
		                        }
		                        """;

		string PATTERN = PATTERN0;

		bool isDir = unit.name.Equals("Direction");
		if (!isDir) PATTERN += PATTERN1;
		if (d > 1) PATTERN += PATTERN2;
		if (d > 2) PATTERN += PATTERN3;
		
		string UNIT = unit.name;
		string SPECIAL = isDir ? "Magnitude" : UNIT;
		
		sb.AppendLine(PATTERN
			.Replace("UNIT", UNIT)
			.Replace("SPECIAL", SPECIAL)
			.Replace("NAMESPACE", NAMESPACE)
			.Replace("INTERFACE", "IAutoUnitUI"));

		const string PATTERN4 = """
		                            [InspectorName("UNIT")]
		                            UNIT_NAME,
		                        """;
		
		StringBuilder allPatterns = new();
		
		foreach (string scale in unitInspectorValues[unit.name]) {
			string scaleName = unit.scales.FirstOrDefault(t => t.Item1.Equals(scale)).Item2;
			if (scaleName == null) continue;
			
			allPatterns.AppendLine(
				PATTERN4
					.Replace("UNIT_NAME", scaleName)
					.Replace("UNIT", scale)
				);
		}

		const string PATTERN5 = """

		                        public enum UNITUIType {
		                        PATTERN4
		                        }
		                        """;

		sb.AppendLine(PATTERN5
			.Replace("PATTERN4", allPatterns.ToString())
			.Replace("UNIT", UNIT));
		
		sb.AppendLine("}");
		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
	
	private static void GenerateUnitData(StringBuilder sb, Unit unit, int d) {
		string UNIT = unit.GetName(d);
		const string PATTERN1 = """
		                            public float x;
		                            
		                            public UNIT(float x) {
		                                this.x = x;
		                            }
		                            
		                            public float vector => x;
		                            
		                            public static implicit operator float(UNIT v) => v.x;
		                            public SPECIAL magnitude => new(x);
		                        """;

		const string PATTERN2 = """
		                            public float x, y;
		                            
		                            public UNIT(float x, float y) {
		                                this.x = x;
		                                this.y = y;
		                            }
		                            
		                            public UNIT(Vector2 v2) {
		                                x = v2.x;
		                                y = v2.y;
		                            }
		                        
		                            public Vector2 vector => this;
		                        
		                            public static implicit operator Vector2(UNIT v) => new Vector2(v.x, v.y);
		                            public SPECIAL magnitude => new((float)Math.Sqrt((double)x * x + (double)y * y));
		                        """;
		
		const string PATTERN3 = """
		                            public float x, y, z;
		                            
		                            public UNIT(float x, float y, float z) {
		                                this.x = x;
		                                this.y = y;
		                                this.z = z;
		                            }
		                            
		                            public UNIT(Vector3 v3) {
		                                x = v3.x;
		                                y = v3.y;
		                                z = v3.z;
		                            }
		                            
		                            public Vector3 vector => this;
		                            
		                            public static implicit operator Vector3(UNIT v) => new Vector3(v.x, v.y, v.z);
		                            public SPECIAL magnitude => new((float)Math.Sqrt((double)x*x + (double)y*y + (double)z*z));
		                        """;


		sb.Append("""
		          using System;
		          using UnityEngine;
		          
		          namespace NAMESPACE {
		          public struct UNIT : INTERFACE {
		          
		          """
			.Replace("UNIT", UNIT)
			.Replace("NAMESPACE", NAMESPACE)
			.Replace("INTERFACE", "IAutoUnit" + GetVectorFloatDimension(d))
		);
		
		bool isDir = unit.name.Equals("Direction");
		string SPECIAL = isDir ? "Magnitude" : unit.name;
		
		sb.Append((d switch {
			1 => PATTERN2,
			2 => PATTERN3,
			_ => PATTERN1
		})
			.Replace("UNIT", UNIT)
			.Replace("SPECIAL", SPECIAL)
			.Replace("NAME", unit.name)
			);
	}

	private static void GenerateUnitSpecials(StringBuilder sb, Unit unit, int d) {
		const string PATTERN1 = """
		                        
		                        
		                            public static UNIT operator *(UNIT left, Magnitude right) => new(left.vector * right.vector);
		                            public static UNIT operator *(Magnitude left, UNIT right) => new(left.vector * right.vector);
		                        """;

		const string PATTERN2 = """
		                        
		                        
		                            public static UNIT operator *(SECOND left, DIRECTION right) => new(left.vector * right.vector);
		                            public static UNIT operator *(DIRECTION left, SECOND right) => new(left.vector * right.vector);
		                        """;


		switch (unit.name) {
			case "Magnitude": {
				foreach (KeyValuePair<string,Unit> pair in units) {
					Unit other = pair.Value;
					int dim = unitDimensions[other.name];

					if (other.name.Equals("Magnitude") || other.name.Equals("Direction")) {
						continue;
					}
					
					for (int i = 0; i < dim; i++) {
						sb.Append(PATTERN1.Replace("UNIT", other.GetName(i)));
					}
				}
				break;
			}
			
			case "Direction": {
				if (d == 0) return;
				
				foreach (KeyValuePair<string,Unit> pair in units) {
					Unit other = pair.Value;
					int dim = unitDimensions[other.name];

					if (other.name.Equals("Magnitude") || other.name.Equals("Direction")) {
						continue;
					}

					if (dim > d) {
						sb.Append(PATTERN2
							.Replace("UNIT", other.GetName(d))
							.Replace("SECOND", other.GetName(0))
							.Replace("DIRECTION",  unit.GetName(d))
						);
					}
				}
				break;
			}
			
		}
	}
	
	private static void GenerateUnitScales(StringBuilder sb, Unit unit, int d) {
		string PATTERN = """

		                 
		                     public VECTOR ToSCALE_NAME() => PATTERN4;
		                     public static UNIT SCALE_NAME(PATTERN1) => new UNIT(PATTERN2);
		                 """;

		if (d != 0) {
			PATTERN += """
			           
			               public static UNIT SCALE_NAME(VECTOR v) => new UNIT(PATTERN3);
			           """;
		}

		const string PATTERN1 = "float x";
		const string PATTERN2 = "x * SCALE_VALUE";
		const string PATTERN3 = "v.x * SCALE_VALUE";
		const string PATTERN4 = "x * INV_SCALE_VALUE";
		
		string UNIT = unit.GetName(d);
		
		foreach ((string name, string fullName, double value)  in unit.scales) {
			string SUB_PATTERN = PATTERN
				.Replace("VECTOR", GetVector(d))
				.Replace("PATTERN1", RepeatPatternXYZ(d, PATTERN1))
				.Replace("PATTERN2", RepeatPatternXYZ(d, PATTERN2))
				.Replace("PATTERN3", RepeatPatternXYZ(d, PATTERN3))
				.Replace("PATTERN4", NewVector(d, RepeatPatternXYZ(d, PATTERN4)));
			
			string SCALE_NAME = fullName;
			string SCALE_VALUE = value.ToString("G10", CultureInfo.InvariantCulture) + "f";
			string INV_SCALE_VALUE = (1 / value).ToString("G10", CultureInfo.InvariantCulture) + "f";
			
			sb.Append(SUB_PATTERN
				.Replace("UNIT", UNIT)
				.Replace("INV_SCALE_VALUE", INV_SCALE_VALUE)
				.Replace("SCALE_NAME", SCALE_NAME)
				.Replace("SCALE_VALUE", SCALE_VALUE)
			);
		}
	}

	private static void GenerateUnitConversions(StringBuilder sb, Unit unit, int d) {
		string UNIT = unit.GetName(d);
		sb.AppendLine($"""
		               
		               
		                   {GetOperator(UNIT, "+", d)}
		                   {GetOperator(UNIT, "-", d)}
		                   {GetOperator(UNIT, "*", d)}
		                   {GetOperator(UNIT, "/", d)}
		               """);
		
		foreach ((string _a, bool isMult, string b) in unit.conversions) {
			string a = units[_a].GetName(d);
			
			if (isMult && b.Equals("Time")) {
				sb.AppendLine($"""
				               
				                   public static {UNIT} operator +({a} a, {UNIT} b) => b + a.{UNIT}(VTime.deltaTime);
				                   public static {UNIT} operator +({UNIT} b, {a} a) => b + a.{UNIT}(VTime.deltaTime);
				                   public static {UNIT} operator -({a} a, {UNIT} b) => a.{UNIT}(VTime.deltaTime) - b;
				                   public static {UNIT} operator -({UNIT} b, {a} a) => b - a.{UNIT}(VTime.deltaTime);
				                   
				               """);
			}

			int maxD = Math.Min(unitDimensions[_a], unitDimensions[b]);
			
			string main = unitConversions.FirstOrDefault(t => t.Item1.Equals(unit.name)).Item2;
			bool flip = a.Equals(main);

			string U1 = flip ? b : a;
			string U2 = flip ? a : b;

			if (maxD == 1) {
				int repA, repB;
				string patternA, patternB;
				string A, B;
				
				if (!isMult) {
					int d1 = Math.Min(unitDimensions[flip ? b : _a], d + 1);
					int d2 = Math.Min(unitDimensions[flip ? _a : b], d + 1);
					
					patternA = d2 == 1 ? "v * x" : d1 == 1 ? "v.magnitude * magnitude" : "v.x * x";
					patternB = d1 == 1 ? "v / x" : d2 == 1 ? "v.magnitude / magnitude" : "v.x / x";
					
					repA = d1 == 1 && d2 != 1 ? 0 : d; 
					repB = d1 != 1 && d2 == 1 ? 0 : d;

					A = U1;
					B = U2;
				}
				else {
					int da = Math.Min(unitDimensions[_a], d + 1);
					int db = Math.Min(unitDimensions[b], d + 1);

					patternA = db == 1 ? "x / v" : da == 1 ? "magnitude / v.magnitude" : "x / v.x";
					patternB = da == 1 ? "x / v" : db == 1 ? "magnitude / v.magnitude" : "x / v.x";
					
					repA = da == 1 && db != 1 ? 0 : d; 
					repB = da != 1 && db == 1 ? 0 : d;

					A = a;
					B = b;
				}
				
				sb.AppendLine(GenerateConverter(A, B, RepeatPatternXYZ(repA, patternA)));
				sb.AppendLine(GenerateConverter(B, A, RepeatPatternXYZ(repB, patternB)));
			}
			else {
				if (!isMult) {
					sb.AppendLine(GenerateConverter(U1, U2, RepeatPatternXYZ(d, "v.x * x")));
					sb.AppendLine(GenerateConverter(U2, U1, RepeatPatternXYZ(d, "v.x / x")));
				}
				else {
					string pattern1 = unitDimensions[_a] == 1 ? "DONT_CHANGE / v.x" : "x / v.x";
					string pattern2 = RepeatPatternXYZ(d, pattern1).Replace("DONT_CHANGE", "x");
					
					sb.AppendLine(GenerateConverter(a, b, pattern2));
					sb.AppendLine(GenerateConverter(b, a, pattern2));
				}
			}

			sb.AppendLine($"    public {UNIT}({a} a, {b} b) => a.{UNIT}(b);");
			sb.AppendLine($"    public {UNIT}({b} b, {a} a) => a.{UNIT}(b);");
		}
	}

	private static string GenerateConverter(string u1, string u2, string pattern) {
		return $"    public {u1} {u1}({u2} v) => new({pattern});";
	}

	private static string GetOperator(string unit, string op, int d) {
		string XYZ = RepeatPatternXYZ(d, $"a.x {op} b.x");
		return $"public static {unit} operator {op}({unit} a, {unit} b) => new({XYZ});";
	}

	private static string GetVector(int dim) {
		return dim switch {
			2 => "Vector3",
			1 => "Vector2",
			_ => "float"
		};
	}

	private static string RepeatPatternXYZ(int dim, string pattern) {
		return dim switch {
			2 => pattern + ", " + pattern.Replace("x", "y") + ", " + pattern.Replace("x", "z"),
			1 => pattern + ", " + pattern.Replace("x", "y"),
			_ => pattern
		};
	}
	
	private static string NewVector(int dim, string pattern) {
		return dim switch {
			0 => pattern,
			1 => $"new Vector2({pattern})",
			_ => $"new Vector3({pattern})"
		};
	}

	private static string GetVectorFloatDimension(int dim) {
		return dim == 0 ? "" : dim + 1 + "";
	}
}