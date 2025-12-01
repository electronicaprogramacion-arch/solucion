using System;
using System.Collections.Generic;
using System.Text;

namespace CalibrationSaaS.Domain.Aggregates.Security
{
	public interface IPolicyService
	{
		string GetPolicy(string policyName, string pageHref = null, long? id = null);
	}
}
