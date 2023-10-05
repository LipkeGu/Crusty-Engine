﻿using Crusty.Engine.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;

namespace Crusty.Engine
{
	public class Shader : IShader
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

					if (line.Contains("##GL_VERSION##"))
						line = line.Replace("##GL_VERSION##", string.Format("{0}{1}",
							EngineLayer.GLVerMajor, EngineLayer.GLVerMinor));

					if (line.Contains("#bind "))
						line = line.Replace("#bind ", string.Empty);

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

		void Get_Attributes(string src, bool getAttributes = false)
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
						
						if (attribute.EndsWith(";"))
							attribute = attribute.Substring(0, attribute.Length - 1);

						if (!attributeLocations.ContainsKey(attribute))
							attributeLocations.Add(attribute, int.Parse(parts[2].Replace(")", string.Empty)));
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

		public void Set_Vec4(string name, Vector4 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform4(uniformLocations[name], ref value);
		}

		public void Set_Light(IList<Light> light)
		{
			for (var i = 0; i < light.Count; i++)
			{
				Set_Vec3(string.Format("lightPosition[{0}]", i), light[i].Position);
				Set_Vec3(string.Format("lightColor[{0}]", i), light[i].LightColor);
				Set_Vec3(string.Format("attenuation[{0}]", i), light[i].Attenuation);
			}
		}

		public void Set_Vec3(string name, Vector3 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform3(uniformLocations[name], ref value);
		}

		public void Set_Vec2(string name, Vector2 value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform2(uniformLocations[name], ref value);
		}

		public void Set_Float(string name, float value)
		{
			if (!uniformLocations.ContainsKey(name))
				uniformLocations.Add(name, GL.GetUniformLocation(shaderProgram, name));

			GL.Uniform1(uniformLocations[name], value);
		}

		void Get_Error(ShaderType type)
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
			this.BindAttribute("Normal");
		}

		void BindAttribute(string variableName)
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
