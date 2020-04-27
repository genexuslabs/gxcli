using common;

namespace gxcli
{
	public class ConfigProvider : IGXCliVerbProvider
	{
		public ConfigProvider(string assemblyPath, string classImplmentation, IGXCliVerbProvider provider)
		{
			AssemblyLocation = assemblyPath;
			ClassName = classImplmentation;
			Name = provider.Name;
			Description = provider.Description;
			Target = provider.Target;
			Parameters = provider.Parameters;
		}

		public string Name { get; private set; }

		public string Description { get; private set; }

		public string Target { get; private set; }

		public string[] Parameters { get; private set; }

		public string AssemblyLocation { get; set; }

		public string ClassName { get; set; }
	}
}
