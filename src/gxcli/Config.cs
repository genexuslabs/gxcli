using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using common;
using gxcli.common;
using Newtonsoft.Json;

namespace gxcli
{
	public class Config
	{
		readonly static string GXCLI_CONFIG = "gxcli.config";
		public const string GXENEXUS_EXE = "genexus.exe";
		public const string GXCLI_MODULES = "gxclimodules";

		public Dictionary<string, ConfigProvider> Providers = new Dictionary<string, ConfigProvider>();

		public string GeneXusPath { get; set; }
		public string GeneXusVersion { get; set; }
		public string CurrentVersion { get; set; }

		private static string ConfigFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GXCLI_CONFIG);

		static Config s_instance;
		public static Config Default
		{
			get
			{
				if (s_instance == null)
				{
					if (!File.Exists(ConfigFilePath))
						throw new ApplicationException("gxcli not installed. Execute 'gx install <GeneXus Path>'");

					s_instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFilePath));
				}

				if (s_instance.CurrentVersion != Application.ProductVersion)
					throw new ApplicationException("This is a newer version of gxcli. Execute 'gx install <GeneXus Path>'");

				return s_instance;
			}
		}

		public static void Install(string gxPath)
		{
			if (string.IsNullOrEmpty(gxPath))
				throw new Exception("You must enter the path to a GeneXus installation");

			if (!Directory.Exists(gxPath))
				throw new DirectoryNotFoundException($"'{gxPath}' does not look like a valid directory");

			string gxExe = Path.Combine(gxPath, Config.GXENEXUS_EXE);
			if (!File.Exists(gxExe))
				throw new Exception($"'{gxPath}' does not look like a valid GeneXus installation folder");

			AssemblyInformationalVersionAttribute afv = Assembly.LoadFrom(gxExe).GetCustomAttribute<AssemblyInformationalVersionAttribute>();
			s_instance = new Config
			{
				GeneXusPath = gxPath,
				GeneXusVersion = afv.InformationalVersion,
				CurrentVersion = Application.ProductVersion
			};

			var dir = AppDomain.CurrentDomain.BaseDirectory;
			string modulesPath = Path.Combine(dir, GXCLI_MODULES);

			foreach (string dllPath in Directory.GetFiles(modulesPath, "*.dll"))
			{
				Assembly ass = Assembly.LoadFrom(dllPath);
				Console.WriteLine($"Analyzing {ass.GetName().Name}...");

				Attribute att = ass.GetCustomAttribute(typeof(GXCliVerbProviderAttribute));
				if (att == null)
					continue;

				Console.WriteLine($"{ass.GetName().Name} is a gxcli verb provider");
				Console.WriteLine($"Adding {ass.GetName().Name} verbs...");

				foreach (Type t in ass.GetExportedTypes())
				{
					if (typeof(IGXCliVerbProvider).IsAssignableFrom(t))
					{
						IGXCliVerbProvider obj = Activator.CreateInstance(t) as IGXCliVerbProvider;
						ConfigProvider configProvider = new ConfigProvider(dllPath, t.FullName, obj)
						{
							HasValidator = typeof(ParameterValidator).IsAssignableFrom(t)
						};
						if (!s_instance.Providers.ContainsKey(obj.Name))
						{
							Console.WriteLine($"- {obj.Name}");
							s_instance.Providers.Add(obj.Name.ToLower(), configProvider);
						}
					}
				}
			}

			s_instance.Save();
		}

		private void Save()
		{
			File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(this));
		}
	}
}
