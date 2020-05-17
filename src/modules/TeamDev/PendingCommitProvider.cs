using System.Collections.Generic;
using common;
using gxcli.common;
using TeamDev.Utils;

namespace TeamDev
{
	public class PendingCommitProvider : IGXCliVerbProvider
	{
		public string Name => "pending-commit";

		public string Description => "Gets a list of the objects with changes pending for commit";

		public string Target => "PendingCommit";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ShowModifiedOn", Description = "Show modified date and time while listing changes to output" }
		};

		public List<Example> Examples => new List<Example>()
		{
			new Example { Command = "gx pending-commit kbpath=C:\\Models\\MyKB showmodifiedon", Description = "Shows a list of objects pending for commit along with the date they were last modified." }
		};
	}
}
