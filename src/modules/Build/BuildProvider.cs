using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;

namespace Build
{
	public class BuildProvider : IGXCliVerbProvider
	{
		public string Name => "build-all";

		public string Description => "Builds a Knowledge Base environment";

		public string Target => "Build";

		public string[] Parameters => new string[] { "GX_PROGRAM_DIR", "WorkingDirectory" };
	}
}
