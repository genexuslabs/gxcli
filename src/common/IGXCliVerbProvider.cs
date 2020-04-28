using System.Collections.Generic;

namespace common
{
	public interface IGXCliVerbProvider
	{
		string Name { get;  }
		string Description { get;  }
		string Target { get; }
		List<VerbParameter> Parameters { get; }
	}

	public class VerbParameter
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public bool Optional { get; set; }
	}
}
