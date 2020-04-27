using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
	public interface IGXCliVerbProvider
	{
		string Name { get;  }
		string Description { get;  }
		string Target { get; }
		string[] Parameters { get; }
	}
}
