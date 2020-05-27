using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using common;
using gxcli.common;
using gxcli.Misc;
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

			RegisterAutocomplete();
		}

		private static void RegisterAutocomplete()
		{
			Process p = Process.GetCurrentProcess();
			PerformanceCounter parent = new PerformanceCounter("Process", "Creating Process ID", p.ProcessName);
			int ppid = (int)parent.NextValue();

			if (Process.GetProcessById(ppid).ProcessName != "powershell")
				return;

			Console.WriteLine("Generating powershell auto-complete script");
			string psScript = Path.Combine(Application.StartupPath, "gxcli-autocomplete.ps1");

			string header = @"$gxCompleter = {

	param($wordToComplete, $commandAst, $cursorPosition)

	$tokens = $commandAst.Extent.Text.Trim() -split '\s+'
	$completions = switch ($tokens[1]) {
";
			string footer = @"
	}

	$completions | Where-Object {$_ -like ""${wordToComplete}*""} | ForEach-Object {
		[System.Management.Automation.CompletionResult]::new($_, $_, 'ParameterValue', $_)
		}
	}

Register-ArgumentCompleter -CommandName gx -Native -ScriptBlock $gxCompleter";

			StringBuilder def = new StringBuilder("\t\tdefault {");

			StringBuilder builder = new StringBuilder();
			foreach (string verb in Config.Default.Providers.Keys)
			{
				def.Append($"\"{verb}\",");
				builder.Append($"\t\t'{verb}' {{");
				foreach (var param in Config.Default.Providers[verb].Parameters)
				{
					builder.Append($"\"{param.Name}\",");
				}
				builder.Replace(",", ";", builder.Length - 1,1);
				builder.AppendLine("break }");
			}

			foreach (var option in GlobalOption.GetAll())
			{
				def.Append($"\"{option.Name}\",");
			}
			def.Replace(",", "}", def.Length - 1, 1);


			using (StreamWriter file = File.CreateText(psScript))
			{
				file.Write(header);
				file.Write(builder.ToString());
				file.Write(def.ToString());
				file.Write(footer);
			}
		}

		private void Save()
		{
			File.WriteAllText(ConfigFilePath, JsonConvert.SerializeObject(this));
		}
	}
}
