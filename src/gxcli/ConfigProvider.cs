using System.Collections.Generic;
using common;

namespace gxcli
{
	public class ConfigProvider : IGXCliVerbProvider
	{
		public ConfigProvider() { }

		public ConfigProvider(string assemblyPath, string classImplmentation, IGXCliVerbProvider provider)
		{
			AssemblyLocation = assemblyPath;
			ClassName = classImplmentation;
			Name = provider.Name;
			Description = provider.Description;
			Target = provider.Target;
			Parameters = provider.Parameters;
			Examples = provider.Examples;
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public string Target { get; set; }

		public List<VerbParameter> Parameters { get; private set; } = new List<VerbParameter>();

		public List<Example> Examples { get; private set; } = new List<Example>();

		public string AssemblyLocation { get; set; }

		public string ClassName { get; set; }
	}
}
