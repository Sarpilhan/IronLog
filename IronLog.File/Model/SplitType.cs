using System;
using System.Collections.Generic;
using System.Text;

namespace IronLog.File.Model
{ 
	public enum SplitType
	{
		Infinite,
		Minute,
		Hourly,
		QuarterlyDaily,
		HalfDay,
		Daily,
		Weekly,
		Monthly
	}
}
