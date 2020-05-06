using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using TeamDev.Utils;

namespace TeamDev
{
	public class PendingCommitProvider : IGXCliVerbProvider
	{
		public string Name => "pending-commit";

		public string Description => "N/A";

		public string Target => "PendingCommit";

		public List<VerbParameter> Parameters => new List<VerbParameter>(GXServerProperties.GXserverParameters);

		public List<Example> Examples => new List<Example>();
	}
}
