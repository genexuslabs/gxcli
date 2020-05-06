using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common;
using gxcli.common;

namespace TeamDev.Utils
{
	public class GXServerProperties
	{
		public static List<VerbParameter> GXserverParameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ServerUserName", Description = "UserName of the authenticated user to create a Knwoeledge Base from Server.", Required = true},
			new VerbParameter { Name = "ServerPassword", Description = "Password of the authenticated user to create a Knwoeledge Base from Server.", Required = true }
		};
	}
}
