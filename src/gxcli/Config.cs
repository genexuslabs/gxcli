﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using common;
using gxcli.common;
using Newtonsoft.Json;

namespace gxcli
{
	public class Config
	{
		readonly static string GXCLI_CONFIG = "gxcli.config";

		public Dictionary<string, ConfigProvider> Providers = new Dictionary<string, ConfigProvider>();
		private static string ConfigFilePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GXCLI_CONFIG);

		static Config s_instance;
		public static Config Default
		{
			get
			{
				if (s_instance == null)
				{
					if (!File.Exists(ConfigFilePath))
						throw new ApplicationException("gxcli not installed or no modules found.");

					s_instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigFilePath));
				}
				return s_instance;
			}
		}

		public static void Install()
		{
			var dir = AppDomain.CurrentDomain.BaseDirectory;
			string modulesPath = Path.Combine(dir, "gxclimodules");

			s_instance = new Config();

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
						ConfigProvider configProvider = new ConfigProvider(dllPath, t.FullName, obj);
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
