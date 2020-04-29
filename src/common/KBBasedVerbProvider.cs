using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;

namespace gxcli.common
{
	public abstract class KBBasedVerbProvider
	{
		public static List<VerbParameter> KBParameters => new List<VerbParameter>
		{
			new VerbParameter { Name = "KBPath", Description = "Path to the Knowledge Base directory", Required = true},
			new VerbParameter { Name = "Version", Description = "Name of the Knowledge Base version to build" },
			new VerbParameter { Name = "Environment", Description = "Name of the Environment to build"},
		};
	}
}
