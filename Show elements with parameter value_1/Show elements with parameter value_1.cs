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

dd/mm/2023	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

namespace Show_elements_with_parameter_value_1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using AdaptiveCards;
	using System.Linq;
	using System.Text;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Analytics.GenericInterface.QueryBuilder;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Net;

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
			InputData inputData = new InputData(engine);
			engine.GenerateInformation("TVP" + 1);
			var dms = engine.GetDms();

			engine.GenerateInformation("TVP" + 2);
			var Elements = dms.GetElements().Where(x=>x.Protocol.Name == inputData.ProtocolName);
			engine.GenerateInformation("TVP" + 3);
			if (!Elements.Any())
			{
				HandleNoElementsFound(engine, dms, inputData);
			}
			engine.GenerateInformation("TVP" + Elements.First());

			List<string> matchingElements = new List<string>();
			foreach (IDmsElement element in Elements)
			{
				matchingElements.Add(element.GetStandaloneParameter<string>(inputData.Parameter).GetValue());
			}
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
				new AdaptiveTextBlock($"No Elements found running {protocolName} ") { Wrap = true },
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