using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace gxcli
{
	public class Config
	{
		readonly string ConfigFile = "gxcli.config";

		public Dictionary<string, ConfigProvider> Providers = new Dictionary<string, ConfigProvider>();

		static Config s_instance;
		public static Config Default
		{
			get
			{
				if (s_instance == null)
					s_instance = new Config();

				return s_instance;
			}
		}

		public void Save()
		{
			File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFile), JsonConvert.SerializeObject(this));
		}
	}
}
