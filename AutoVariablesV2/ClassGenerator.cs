using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ClassGenerator;

[Generator]
public class ClassGenerator : IIncrementalGenerator {
	private const string NAMESPACE = "Lepsima.ASV";

	public static readonly Dictionary<string, UnitInfo> units = new() {
		{
			"Magnitude", new UnitInfo("Magnitude", "mag", 1) {
				scales = [
					("mag", "ZeroToOne", 1)
				],
				inspectorUnits = ["mag"],
			}
		}, {
			"Direction", new UnitInfo("Direction", "dir", 3) {
				scales = [
					("dir", "Normal", 1),
					("dir", "Dir", 1)
				],
				inspectorUnits = ["dir"],
			} 
		}, { 
			"Time", new UnitInfo("Time", "s", 1) { 
				scales = [
					("μs", "Microsecond", 0.000001),
					("ms", "Millisecond", 0.001),
					("s", "Second", 1),
					("m", "Minute", 60),
					("h", "Hour", 3600),
				],
				inspectorUnits = ["ms", "s", "m", "h"],
			}
		}, {
			"Mass", new UnitInfo("Mass", "kg", 1) {
				scales = [
					("mg", "Milligram", 0.000001),
					("g", "Gram", 0.001),
					("kg", "Kilogram", 1),
					("t", "Ton", 1000),
					("kt", "Kiloton", 1000000),
				],
				inspectorUnits = ["g", "kg", "t"],
			}
		}, { 
			"Position", new UnitInfo("Position", "m", 3) {
				scales = [
					("ft", "Feet", 3.280839895),
					("mi", "Mile", 1609.344),
					("mm", "Millimeter", 0.001),
					("cm", "Centimeter", 0.01),
					("m", "Meter", 1),
					("km", "Kilometer", 1000),
					("Mm", "Megameter", 1000000),
				],
				inspectorUnits = ["ft", "mi", "cm", "m", "km"],
				conversions = [("Velocity", "Time")]
			}
		}, {
			"Velocity", new UnitInfo("Velocity", "m/s", 3) {
				inspectorUnits = ["m/s", "km/h", "ft/s", "mi/h"],
				conversions = [("Accel", "Time")],
				compoundInputs = ["Position / Time"]
			}
		}, {
			"Accel", new UnitInfo("Accel", "m/s/s", 3) {
				inspectorUnits = ["m/s2", "km/h2", "ft/s2", "mi/h2"],
				compoundInputs = ["Velocity / Time", "Force * Mass"]
			}
		}, {
			"Force", new UnitInfo("Force", "N", 3) {
				scales = [
					("mN", "Millinewton", 0.001),
					("N", "Newton", 1),
					("kN", "Kilonewton", 1000),
					("MN", "Meganewton", 1000000),
				],
				conversions = [
					("Accel", "Mass"),
					("ForceAccel", "Time")
				],
				inspectorUnits = ["N", "kN", "MN"],
			}
		}, {
			"ForceAccel", new UnitInfo("ForceAccel", "N/s", 3) {
				inspectorUnits = ["N/s", "kN/s"],
				compoundInputs = ["Force / Time"]
			}
		}, {
			"Angle", new UnitInfo("Angle", "N", 3) {
				scales = [
					("deg", "Degree", 1),
					("rad", "Radian", 0.01745329238474369),
					("turn", "Turn", 360),
					("'", "MinuteDegree", 0.016666666666666666),
					("''", "SecondDegree", 0.0002777777777777778),
				],
				conversions = [("AngleVel", "Time")],
				inspectorUnits = ["deg", "rad"]
			}
		}, {
			"AngleVel", new UnitInfo("AngleVel", "deg/s", 3) {
				inspectorUnits = ["deg/s", "deg/m", "rad/s", "rad/m"],
				conversions = [("AngleAccel", "Time")],
				compoundInputs = ["Angle / Time"]
			}
		}, {
			"AngleAccel", new UnitInfo("AngleAccel", "deg/s2", 3) {
				inspectorUnits = ["deg/s2", "deg/m2", "rad/s2", "rad/m2"],
				compoundInputs = ["AngleVel / Time", "Torque * Mass"]
			}
		}, {
			"Torque", new UnitInfo("Torque", "N*m", 3) {
				scales = [
					("mN", "Millinewton", 0.001),
					("N", "Newton", 1),
					("kN", "Kilonewton", 1000),
					("MN", "Meganewton", 1000000),
				],
				conversions = [
					("AngleAccel", "Mass"),
					("TorqueAccel", "Time"),
					("Force", "Position")
				],
				inspectorUnits = ["N*m", "kN*m"],
				compoundInputs = ["Force * Position"]
			}
		}, {
			"TorqueAccel", new UnitInfo("TorqueAccel", "N*m/s", 3) {
				inspectorUnits = ["N*m/s", "kN*m/s"],
				compoundInputs = ["Torque / Time"]
			}
		}, 
	};
	
	public void Initialize(IncrementalGeneratorInitializationContext context) {
		var structs = context.SyntaxProvider
			.CreateSyntaxProvider(
				predicate: static (node, _) => node is StructDeclarationSyntax,
				transform: static (ctx, _) => (StructDeclarationSyntax)ctx.Node)
			.Collect();

		context.RegisterSourceOutput(structs, Generate);
	}

	private static void Generate(SourceProductionContext context, ImmutableArray<StructDeclarationSyntax> structs) {
		foreach (UnitInfo info in units.Values) 
			info.Init();
		
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
		
		context.AddSource("ASV/Utils/Interfaces.cs", SourceText.From(file, Encoding.UTF8));
		
		foreach (UnitInfo unit in units.Values) {
			for (int i = 0; i < unit.dimensions; i++) {
				if (i == 0 && unit.name.Equals("Direction")) {
					continue;
				}

				string folder = i switch {
					0 => "Values1D/",
					1 => "Values2D/",
					2 => "Values3D/",
					_ => "ERROR/"
				};

				SourceText source = GenerateUnitClass(unit, i);
				context.AddSource($"ASV/{folder}{unit.GetName(i)}.cs", source);
			}
			
			SourceText sourceUI = GenerateUnitUIClass(unit);
			context.AddSource($"ASV/ValuesUI/{unit.name}UI.cs", sourceUI);
		}
	}
	
	private static SourceText GenerateUnitClass(UnitInfo unit, int d) {
		StringBuilder sb = new();
		
		GenerateUnitData(sb, unit, d);
		GenerateUnitSpecials(sb, unit, d);
		GenerateUnitScales(sb, unit, d);
		GenerateUnitConversions(sb, unit, d);
		
		sb.AppendLine("}");
		sb.AppendLine("}");
		
		return SourceText.From(sb.ToString(), Encoding.UTF8);
	}
	
	private static SourceText GenerateUnitUIClass(UnitInfo unit) {
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
		                                type = UNITUIType.DEFAULT_TYP;
		                            }
		                            
		                            public static implicit operator float(UNITUI v) => v.Value;
		                            public static implicit operator UNIT(UNITUI v) {
		                        	    return v.type switch {
		                        PATTERN6
		                        		    _ => new UNIT(v.Value)
		                        	    };
		                            }
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
		                                type = UNITUIType.DEFAULT_TYP;
		                            }
		                            
		                            public UNIT2UI(Vector2 v2) {
		                                x = v2.x;
		                                y = v2.y;
		                                type = UNITUIType.DEFAULT_TYP;
		                            }
		                          
		                            public static implicit operator Vector2(UNIT2UI v) => v.Value;
		                            public static implicit operator UNIT2(UNIT2UI v) {
		                        	    return v.type switch {
		                        PATTERN7
		                        		    _ => new UNIT2(v.Value)
		                        	    };
		                            }
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
		                                type = UNITUIType.DEFAULT_TYP;
		                            }
		                        
		                            public UNIT3UI(Vector3 v3) {
		                                x = v3.x;
		                                y = v3.y;
		                                z = v3.z;
		                                type = UNITUIType.DEFAULT_TYP;
		                            }
		                        
		                            public static implicit operator Vector3(UNIT3UI v) => v.Value;
		                            public static implicit operator UNIT3(UNIT3UI v) {
		                        	    return v.type switch {
		                        PATTERN8
		                        		    _ => new UNIT3(v.Value)
		                        	    };
		                            }
		                        }
		                        """;

		string PATTERN = PATTERN0;
		
		
		bool isDir = unit.name.Equals("Direction");
		if (!isDir) PATTERN += PATTERN1;
		if (unit.dimensions > 1) PATTERN += PATTERN2;
		if (unit.dimensions > 2) PATTERN += PATTERN3;
		
		string UNIT = unit.name;
		string SPECIAL = isDir ? "Magnitude" : UNIT;
		string default_type = unit.inspectorUnits.Count > 0 ? unit.GetScaleName(unit.inspectorUnits[0]) : "ERROR";
		
		StringBuilder sb6 = new(), sb7 = new(), sb8 = new();
		
		foreach (string inspectorUnit in unit.inspectorUnits) {
			string inspectorName = unit.GetScaleName(inspectorUnit);
			if (inspectorName == null) continue;
			
			string pat6 = $"	        {UNIT}UIType.{inspectorName} => {UNIT}.{inspectorName}(v.x),";
			string pat7 = $"	        {UNIT}UIType.{inspectorName} => {UNIT}2.{inspectorName}(v.x, v.y),";
			string pat8 = $"	        {UNIT}UIType.{inspectorName} => {UNIT}3.{inspectorName}(v.x, v.y, v.z),";
			
			sb6.AppendLine(pat6);
			sb7.AppendLine(pat7);
			sb8.AppendLine(pat8);
		}
		
		sb.AppendLine(PATTERN
			.Replace("PATTERN6", sb6.ToString())
			.Replace("PATTERN7", sb7.ToString())
			.Replace("PATTERN8", sb8.ToString())
			.Replace("UNIT", UNIT)
			.Replace("SPECIAL", SPECIAL)
			.Replace("NAMESPACE", NAMESPACE)
			.Replace("INTERFACE", "IAutoUnitUI")
			.Replace("DEFAULT_TYP", default_type)
		);
		
		const string PATTERN4 = """
		                            [InspectorName("UNIT")]
		                            UNIT_NAME,
		                        """;
		
		StringBuilder allPatterns = new();
		
		foreach (string inspectorUnit in unit.inspectorUnits) {
			string inspectorName = unit.GetScaleName(inspectorUnit);
			if (inspectorName == null) continue;
			
			allPatterns.AppendLine(
				PATTERN4
					.Replace("UNIT_NAME", inspectorName)
					.Replace("UNIT", inspectorUnit.Replace("/", "\u2571"))
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
	
	private static void GenerateUnitData(StringBuilder sb, UnitInfo unit, int d) {
		string UNIT = unit.GetName(d);
		const string PATTERN1 = """
		                            public float x;
		                            
		                            public UNIT(float x) {
		                                this.x = x;
		                            }
		                            
		                            public float vector => x;
		                            
		                            public static implicit operator float(UNIT v) => v.x;
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
		                            public static implicit operator Vector2(UNIT v) => new(v.x, v.y);
		                        
		                            public Direction2 normalized => new(vector.normalized);
		                            public SPECIAL sqrMagnitude => new(vector.sqrMagnitude);
		                            public SPECIAL magnitude => new(vector.magnitude);
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
		                            public static implicit operator Vector3(UNIT v) => new(v.x, v.y, v.z);
		                            
		                            public Direction3 normalized => new(vector.normalized);
		                            public SPECIAL sqrMagnitude => new(vector.sqrMagnitude);
		                            public SPECIAL magnitude => new(vector.magnitude);
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

	private static void GenerateUnitSpecials(StringBuilder sb, UnitInfo unit, int d) {
		bool isDirection = unit.name.Equals("Direction");
		if (!unit.IsSpecialUnit() || (isDirection && d == 0)) return;

		
		const string PATTERN1 = """
		                        
		                        
		                            public static UNIT operator *(UNIT left, Magnitude right) => new(left.vector * right.vector);
		                            public static UNIT operator *(Magnitude left, UNIT right) => new(left.vector * right.vector);
		                        """;

		const string PATTERN2 = """
		                        
		                        
		                            public static UNIT operator *(SECOND left, DIRECTION right) => new(left.vector * right.vector);
		                            public static UNIT operator *(DIRECTION left, SECOND right) => new(left.vector * right.vector);
		                        """;

		foreach (UnitInfo other in units.Values.Where(other => !other.IsSpecialUnit())) {
			if (isDirection) {
				if (other.dimensions > d) {
					sb.Append(PATTERN2
						.Replace("UNIT", other.GetName(d))
						.Replace("SECOND", other.GetName(0))
						.Replace("DIRECTION", unit.GetName(d))
					);
				}
			}
			else {
				for (int i = 0; i < other.dimensions; i++) {
					sb.Append(PATTERN1.Replace("UNIT", other.GetName(i)));
				}
			}
		}
	}
	
	private static void GenerateUnitScales(StringBuilder sb, UnitInfo unit, int d) {
		string PATTERN = """

		                 
		                     public VECTOR ToSCALE_NAME() => PATTERN4;
		                     public static UNIT SCALE_NAME(PATTERN1) => new(PATTERN2);
		                 """;

		if (d != 0) {
			PATTERN += """
			           
			               public static UNIT SCALE_NAME(VECTOR v) => new(PATTERN3);
			           """;
		}

		const string PATTERN1 = "float x";
		const string PATTERN2 = "x * SCALE_VALUE";
		const string PATTERN3 = "v.x * SCALE_VALUE";
		const string PATTERN4 = "x * INV_SCALE_VALUE";

		if (unit.scales == null) return;
		foreach ((string name, string fullName, double value) in unit.scales) {
			string SUB_PATTERN = PATTERN
				.Replace("VECTOR", GetVector(d))
				.Replace("PATTERN1", RepeatPatternXYZ(d, PATTERN1))
				.Replace("PATTERN2", RepeatPatternXYZ(d, PATTERN2))
				.Replace("PATTERN3", RepeatPatternXYZ(d, PATTERN3))
				.Replace("PATTERN4", NewVectorEmpty(d, RepeatPatternXYZ(d, PATTERN4)));
			
			string SCALE_NAME = fullName;
			string SCALE_VALUE = value.ToString("G10", CultureInfo.InvariantCulture) + "f";
			string INV_SCALE_VALUE = (1 / value).ToString("G10", CultureInfo.InvariantCulture) + "f";
			
			sb.Append(SUB_PATTERN
				.Replace("UNIT", unit.GetName(d))
				.Replace("INV_SCALE_VALUE", INV_SCALE_VALUE)
				.Replace("SCALE_NAME", SCALE_NAME)
				.Replace("SCALE_VALUE", SCALE_VALUE)
			);
		}
	}

	private static void GenerateUnitConversions(StringBuilder sb, UnitInfo unit, int d) {
		string UNIT = unit.GetName(d);
		sb.AppendLine($"""
		               
		               
		                   {GetOperator(UNIT, "+", d)}
		                   {GetOperator(UNIT, "-", d)}
		                   
		                   {GetOperator(UNIT, "*", d)}
		                   {GetOperator(UNIT, "/", d)}
		                   
		               """);
		
		foreach ((string _a, bool isMult, string _b) in unit.conversionData) {
			UnitInfo a = units[_a];
			UnitInfo b = units[_b];
			bool isOneDim = a.dimensions == 1;

			if (isMult && _b.Equals("Time")) {
				string OTHR = a.GetName(d);
				sb.AppendLine($"""
				                   public static {UNIT} operator +({OTHR} a, {UNIT} b) => b + a.{UNIT}(VTime.deltaTime);
				                   public static {UNIT} operator +({UNIT} b, {OTHR} a) => b + a.{UNIT}(VTime.deltaTime);
				                   public static {UNIT} operator -({OTHR} a, {UNIT} b) => a.{UNIT}(VTime.deltaTime) - b;
				                   public static {UNIT} operator -({UNIT} b, {OTHR} a) => b - a.{UNIT}(VTime.deltaTime);
				                   
				               """);
			}

			int maxD = Math.Min(a.dimensions, b.dimensions);
			string alt_a = a.GetName(d);
			bool flip = unit.conversions.Count > 0 && alt_a.Equals(unit.conversions[0].Item1);
			
			string U1 = flip ? _b : alt_a;
			string U2 = flip ? alt_a : _b;

			if (maxD == 1) {
				int repA, repB;
				string patternA, patternB;
				string A, B;
				
				if (!isMult) {
					int d1 = Math.Min((flip ? b : a).dimensions, d + 1);
					int d2 = Math.Min((flip ? a : b).dimensions, d + 1);
					
					patternA = d2 == 1 ? "v * x" : d1 == 1 ? "v.magnitude * magnitude" : "v.x * x";
					patternB = d1 == 1 ? "v / x" : d2 == 1 ? "v.magnitude / magnitude" : "v.x / x";
					
					repA = d1 == 1 && d2 != 1 ? 0 : d; 
					repB = d1 != 1 && d2 == 1 ? 0 : d;

					A = U1;
					B = U2;
				}
				else {
					int da = Math.Min(a.dimensions, d + 1);
					int db = Math.Min(b.dimensions, d + 1);

					patternA = db == 1 ? "x / v" : da == 1 ? "magnitude / v.magnitude" : "x / v.x";
					patternB = da == 1 ? "x / v" : db == 1 ? "magnitude / v.magnitude" : "x / v.x";
					
					repA = da == 1 && db != 1 ? 0 : d; 
					repB = da != 1 && db == 1 ? 0 : d;

					A = alt_a;
					B = _b;
				}
				
				sb.AppendLine(GenerateConverter(A, B, RepeatPatternXYZ(repA, patternA)));
				sb.AppendLine(GenerateConverter(B, A, RepeatPatternXYZ(repB, patternB)));
			}
			else {
				if (!isMult) {
					string _a2 = U1, _b2 = U2;

					if (!isOneDim) {
						_a2 = GetMaxDimension(a, d);
						_b2 = GetMaxDimension(b, d);
					}
					
					sb.AppendLine(GenerateConverter(_a2, _b2, RepeatPatternXYZ(d, "v.x * x")));
					sb.AppendLine(GenerateConverter(_b2, _a2, RepeatPatternXYZ(d, "v.x / x")));
				}
				else {
					string pattern1 = isOneDim ? "DONT_CHANGE / v.x" : "x / v.x";
					string pattern2 = RepeatPatternXYZ(d, pattern1).Replace("DONT_CHANGE", "x");

					string _a2 = alt_a, _b2 = _b;
					
					if (!isOneDim) {
						_a2 = GetMaxDimension(a, d);
						_b2 = GetMaxDimension(b, d);
					}
					
					sb.AppendLine(GenerateConverter(_a2, _b2, pattern2));
					sb.AppendLine(GenerateConverter(_b2, _a2, pattern2));
				}
			}

			sb.AppendLine();

			string _a3 = alt_a, _b3 = _b;

			if (!isOneDim) {
				_a3 = GetMaxDimension(a, d);
				_b3 = GetMaxDimension(b, d);
			}
			
			sb.AppendLine($"    public {UNIT}({_a3} a, {_b3} b) : this(a.{UNIT}(b)) {{ }}");
			sb.AppendLine($"    public {UNIT}({_b3} b, {_a3} a) : this(a.{UNIT}(b)) {{ }}");
			sb.AppendLine();
		}
	}

	private static string GetMaxDimension(UnitInfo unit, int maxDim) {
		int max = Math.Min(unit.dimensions - 1, maxDim);
		return unit.GetName(max);
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
			69 => $"new({pattern})",
			_ => $"new Vector3({pattern})"
		};
	}
	
	
	private static string NewVectorEmpty(int dim, string pattern) {
		return dim switch {
			0 => pattern,
			_ => $"new({pattern})",
		};
	}

	public static string GetVectorFloatDimension(int dim) {
		return dim == 0 ? "" : dim + 1 + "";
	}
}