using System;
using System.Collections.Generic;
using common;
using gxcli.common;

namespace Test
{
	public class TestProvider : ParameterValidator, IGXCliVerbProvider
	{
		public string Name => "test";

		public string Description => "Run tests";

		public string Target => "RunTests";

		public List<VerbParameter> Parameters => new List<VerbParameter>(KBBasedVerbProvider.KBParameters)
		{
			new VerbParameter { Name = "ObjectNames", Description = "Comma separated list of test object names." },
			new VerbParameter { Name = "TestType", Description = "Type of test to run (All (default), Unit, Web, UI, SD)." }
		};

		public List<Example> Examples => new List<Example>()
		{
		};

		public override void Validate(Dictionary<string, string> props)
		{
			if (props.ContainsKey("ObjectNames") && !props.ContainsKey("TestType"))
				throw new Exception("When setting ObjectNames TestType must also be set.");

		}
	}
}
