namespace Show_elements_with_parameter_value_1
{
	using System;
	using Skyline.DataMiner.Automation;

	public class InputData
	{
		public InputData(IEngine engine)
		{
			engine.GenerateInformation("TVP" + "a1");
			ProtocolName = engine.GetScriptParam("Protocol Name").Value;
			engine.GenerateInformation("TVP" + "a2");
			Parameter = Convert.ToInt32(engine.GetScriptParam("Parameter").Value);
			engine.GenerateInformation("TVP" +"a3");
			ParameterValue = engine.GetScriptParam("Protocol Value").Value;
		}

		public string ProtocolName { get; set; }

		public int Parameter { get; set; }

		public string ParameterValue { get; set; }
	}
}