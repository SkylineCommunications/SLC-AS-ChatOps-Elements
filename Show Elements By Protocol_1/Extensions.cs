namespace Show_Elements_By_Protocol_1
{
	using System;
	using Skyline.DataMiner.Automation;

	public static class Extensions
	{
		public static bool TryGetScriptParam(this IEngine engine, string paramName, out ScriptParam param, bool valueCheck = false)
		{
			param = engine.GetScriptParam(paramName);
			if (valueCheck && String.IsNullOrWhiteSpace(param?.Value))
				return false;
			return param != null;
		}

		public static bool TryGetScriptParam(this IEngine engine, int paramId, out ScriptParam param, bool valueCheck = false)
		{
			param = engine.GetScriptParam(paramId);
			if (valueCheck && String.IsNullOrWhiteSpace(param?.Value))
				return false;
			return param != null;
		}
	}
}