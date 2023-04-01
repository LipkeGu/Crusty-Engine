using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenWorld.Engine.Video
{
	public class Shader : IDisposable
	{
		Dictionary<ShaderType, int> shaders;
		Dictionary<string, int> uniformLocations;
		Dictionary<string, int> attributeLocations;

		int shaderProgram = 0;

		public Shader()
		{
			shaders = new Dictionary<ShaderType, int>();
			uniformLocations = new Dictionary<string, int>();
			attributeLocations = new Dictionary<string, int>();
		}

		public void Create(string fileName)
		{
			Compile(fileName);
		}

		private Dictionary<ShaderType, string> PreProcess(string path)
		{
			var shadersources = new Dictionary<ShaderType, string>();
			var lines = new List<string>();

			using (var reader = new StreamReader(path))
			{
				var line = string.Empty;
				var shaderType = ShaderType.VertexShader;

				while (!reader.EndOfStream)
				{
					line = reader.ReadLine().Trim();
					if (string.IsNullOrEmpty(line))
						continue;

					#region Get type of shader 
					if (line.StartsWith("#type"))
					{
						lines.Clear();

						var types = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

						switch (types[types.Length - 1])
						{
							case "pixel":
							case "fragment":
								shaderType = ShaderType.FragmentShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							case "vertex":
								shaderType = ShaderType.VertexShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							case "geometry":
								shaderType = ShaderType.GeometryShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							case "compute":
								shaderType = ShaderType.ComputeShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							case "tesselCo":
								shaderType = ShaderType.TessControlShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							case "tesselEv":
								shaderType = ShaderType.TessEvaluationShader;
								shadersources.Add(shaderType, string.Empty);
								break;
							default:
								break;
						}

						continue;
					}
					else
					{
						shadersources[shaderType] += string.Concat(line, Environment.NewLine);
					}
					#endregion
				}
			}

			return shadersources;
		}

		public void Get_Attributes(string src, bool getAttributes = false)
		{
			using (var strm = new StreamReader(src))
			{
				while (!strm.EndOfStream)
				{
					var line = strm.ReadLine().Trim();
					if (string.IsNullOrEmpty(line))
						continue;

					if (line.StartsWith("layout"))
					{
						var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						var attribute = parts[parts.Length - 1];

						if (!attributeLocations.ContainsKey(attribute))
							attributeLocations.Add(attribute, -1);
						else
							attributeLocations[attribute] = GL.GetAttribLocation(shaderProgram, attribute);

						continue;
					}

				}

				strm.Close();
			}
		}

		public void Compile(string filename)
		{
			var sources = PreProcess(filename);

			#region Compiling the Shader
			foreach (var shadersource in sources)
				if (!string.IsNullOrEmpty(shadersource.Value))
				{
					CompileShader(shadersource.Key, shadersource.Value);
					Get_Attributes(filename);
				}
			#endregion

			#region Linking the Shaders

			LinkShader();
			#endregion

		}

		public void Set_Int(string name, int value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform1(uniformLocations[name], value);
		}

		public void Set_Mat4(string name, Matrix4 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.UniformMatrix4(uniformLocations[name], false, ref value);
		}

		public void Set_Vec4(string name, ref Vector4 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform4(uniformLocations[name], ref value);
		}

		public void Set_Vec3(string name, Vector3 value)
		{
			Set_Vec3(name, ref value);
		}

		public void Set_Vec3(string name, ref Vector3 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform3(uniformLocations[name], ref value);
		}

		public void Set_Vec2(string name, ref Vector2 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform2(uniformLocations[name], ref value);
		}

		public void Set_Vec1(string name, float value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform1(uniformLocations[name], value);
		}

		public void Get_Error(ShaderType type)
		{
			var lastError = GL.GetShaderInfoLog(shaders[type]);
			if (lastError != string.Empty)
				Console.WriteLine(lastError);
		}

		void CompileShader(ShaderType type, string src)
		{
			shaders.Add(type, GL.CreateShader(type));
			GL.ShaderSource(shaders[type], src);
			GL.CompileShader(shaders[type]);

			GL.GetShader(shaders[type], ShaderParameter.CompileStatus, out var code);
			if (code != (int)All.True)
				Get_Error(type);
		}

		void BindAttributes()
		{
			this.BindAttribute("Position");
			this.BindAttribute("TexCoord");
			this.BindAttribute("Nomal");
		}


		public void BindAttribute(string variableName)
		{
			if (!attributeLocations.ContainsKey(variableName))
				attributeLocations.Add(variableName, GL.GetAttribLocation(shaderProgram, variableName));

			var index = attributeLocations[variableName];

			if (index == -1)
			{
				attributeLocations[variableName] = GL.GetAttribLocation(shaderProgram, variableName);
			}

			GL.BindAttribLocation(shaderProgram, attributeLocations[variableName], variableName);
		}

		void LinkShader()
		{
			shaderProgram = GL.CreateProgram();

			foreach (var shader in shaders.Values)
				GL.AttachShader(shaderProgram, shader);

			GL.LinkProgram(shaderProgram);
			GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out var code);

			if (code != (int)All.True)
			{
				var error = GL.GetProgramInfoLog(shaderProgram);
				throw new Exception($"Error occurred whilst linking ShaderProgram ({shaderProgram}): {error}");
			}

			GL.ValidateProgram(shaderProgram);
			BindAttributes();
		}

		public void Use()
		{
			GL.UseProgram(shaderProgram);
		}

		public void Unuse()
		{
			GL.UseProgram(0);
		}

		public void Cleanup()
		{
			Unuse();

			foreach (var shader in shaders.Values)
			{
				GL.DetachShader(shaderProgram, shader);
				GL.DeleteShader(shader);
			}

			GL.DeleteProgram(shaderProgram);
			shaders.Clear();
			uniformLocations.Clear();
		}

		public void Dispose() => Cleanup();
	}
}
