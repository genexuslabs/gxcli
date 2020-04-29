using System.Collections.Generic;

namespace gxcli.Misc
{
	internal class GlobalOption
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public static IEnumerable<GlobalOption> GetAll()
		{
			yield return new GlobalOption
			{
				Name = "verbosity",
				Description = @"Specifies the amount of information to display in the output.(q[uiet],m[inimal],n[ormal](default),d[etailed],diag[nostic])."
			};
		}
	}
}
