/*
****************************************************************************
*  Copyright (c) 2023,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

08/06/2023	1.0.0.1		TVP, Skyline	Initial version
****************************************************************************
*/

namespace Show_elements_with_parameter_value_1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AdaptiveCards;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				InputData inputData = new InputData(engine);
				var dms = engine.GetDms();

				var Elements = dms.GetElements().Where(x => x.Protocol.Name.ToLower() == inputData.ProtocolName);
				if (!Elements.Any())
				{
					HandleNoElementsFound(engine, dms, inputData);
				}

				List<IDmsElement> matchingElements = new List<IDmsElement>();
				foreach (IDmsElement element in Elements)
				{
					try
					{
						// To Support Discreet parameters we will also retrieve the displayed parameter.
						string elementParamDisplayedValue;
						if (Int32.TryParse(inputData.Parameter, out int parameterId))
						{
							elementParamDisplayedValue = element.GetStandaloneParameter<string>(parameterId).GetDisplayValue();
						}
						else
						{
							var tempElement = engine.FindElementsByName(element.Name).Single();
							string elementParamValue = Convert.ToString(tempElement.GetParameter(inputData.Parameter));
							elementParamDisplayedValue = tempElement.GetDisplayValue(inputData.Parameter, elementParamValue);
						}

						if (elementParamDisplayedValue == inputData.ParameterValue)
						{
							matchingElements.Add(element);
						}
					}
					catch (Exception)
					{
						// We don't want to stop the execution if some elements give us issues:
						// it can happen that the parameter didn't exist yet in an earlier version of the protocol.
						// Or that the element is in timeout
						continue;
					}
				}

				engine.GenerateInformation("Matching elements list:" + String.Join(".", matchingElements));

				CreateResponse(engine, inputData, matchingElements);
			}
			catch (Exception ex)
			{
				engine.Log(ex.ToString());
				engine.ExitFail(ex.Message);
			}
		}

		private void CreateResponse(IEngine engine, InputData inputData, List<IDmsElement> matchingElements)
		{
			List<AdaptiveElement> card;
			if (!matchingElements.Any())
			{
				HandleNoMatchingElementsFound(engine, inputData);
				return;
			}

			card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"Below you can find the list of all the {inputData.ProtocolName} elements, with parameter : {inputData.Parameter}, and value: {inputData.ParameterValue}") { Wrap = true },
			};

			foreach (var element in matchingElements)
			{
				card.Add(new AdaptiveFactSet
				{
					Facts = new List<AdaptiveFact>
					{
						new AdaptiveFact("Element:", $"{element.Name} ({element.DmsElementId.Value})"),
					},
				});
			}

			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
		}

		private static void HandleNoMatchingElementsFound(IEngine engine, InputData inputData)
		{
			List<AdaptiveElement> card;
			card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"No elements were detected using: {inputData.ProtocolName}, with parameter : {inputData.Parameter}, and value: {inputData.ParameterValue}") { Wrap = true },
			};
			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
			return;
		}

		private void HandleNoElementsFound(IEngine engine, IDms dms, InputData inputData)
		{
			var protocols = dms.GetProtocols().GroupBy(proto => proto.Name).Select(g => g.First());
			var protocol = protocols.FirstOrDefault(proto => proto.Name == inputData.ProtocolName);
			if (protocol == default)
			{
				ProtocolNotFoundResponse(engine, inputData.ProtocolName);
				return;
			}

			NoElementsAssignedToThisProtocol(engine, inputData.ProtocolName);
		}

		private void NoElementsAssignedToThisProtocol(IEngine engine, string protocolName)
		{
			var card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"No Elements found running {protocolName}") { Wrap = true },
			};

			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
		}

		private void ProtocolNotFoundResponse(IEngine engine, string protocolName)
		{
			var card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"Protocol, {protocolName} not found") { Wrap = true },
			};

			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
		}
	}
}