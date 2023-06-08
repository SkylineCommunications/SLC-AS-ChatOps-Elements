namespace Show_elements_with_parameter_value_1
{
	using System;
	using Skyline.DataMiner.Automation;

	public class InputData
	{
		public InputData(IEngine engine)
		{
			ProtocolName = engine.GetScriptParam("Protocol Name").Value;
			Parameter = Convert.ToString(engine.GetScriptParam("Parameter").Value);
			ParameterValue = engine.GetScriptParam("Parameter Value").Value;
		}

		public string ProtocolName { get; set; }

		public string Parameter { get; set; }

		public string ParameterValue { get; set; }
	}
}