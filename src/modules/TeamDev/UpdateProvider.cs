using System.Collections.Generic;
using common;
using TeamDev.Utils;

namespace TeamDev
{
	public class UpdateProvider : IGXCliVerbProvider
	{
		public string Name => "update";

		public string Description => "Updates the local Knowledge Base with the latest changes from GXserver.";

		public string Target => "Update";

		public List<VerbParameter> Parameters => new List<VerbParameter>(GXServerProperties.GXserverParameters)
		{
			new VerbParameter { Name = "UpdateKbProperties", Description = "Update nondefault Knowledgebase properties." },
			new VerbParameter { Name = "Preview", Description = "When true it only shows which are the files to be updated. The Knowledge Base is not modified." },
			new VerbParameter { Name = "ChangesFromDate", Description = "This date will be used to obtain changes in GXserver that were done after this date." },
			new VerbParameter { Name = "ToRevision", Description = "This number will be used to update the local Knowledge Base to the number revision." },
			new VerbParameter { Name = "IncludeItems", Description = "Specifies which objects to import." },
			new VerbParameter { Name = "ExcludeItems", Description = "Specifies which objects to exclude." }
		};

		public List<Example> Examples => new List<Example>();
	}
}
