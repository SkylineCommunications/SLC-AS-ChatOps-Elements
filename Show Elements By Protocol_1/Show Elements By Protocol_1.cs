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

namespace Show_Elements_By_Protocol_1
{
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
		private IDms dms;

		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			var input = engine.GetScriptParam("Protocol Name");
			if (string.IsNullOrWhiteSpace(input?.Value))
			{
				engine.ExitFail("'Protocol Name' parameter is required");
				return;
			}

			dms = engine.GetDms();
			var protocols = dms.GetProtocols().GroupBy(proto => proto.Name).Select(g => g.First());

			// If the given protocol exists
			if (protocols.Select(proto => proto.Name).Contains(input.Value))
			{
				FindElements(engine, input.Value);
				return;
			}

			// If it doesn't give some suggestions, maybe it was misspelled.
			GiveSuggestions(engine, protocols, input.Value);
		}

		private void FindElements(IEngine engine, string input)
		{
			var card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"Below you can find the list of all the {input} elements") { Wrap = true },
			};

			var elements = engine.FindElementsByProtocol(input);
			foreach (var element in elements)
			{
				var elementCard = new AdaptiveDataMinerElement(dms.GetElement(new DmsElementId(element.DmaId, element.ElementId)));
				card.Add(elementCard.ToAdaptiveElement());
			}

			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
		}

		private void GiveSuggestions(IEngine engine, IEnumerable<IDmsProtocol> protocols, string input)
		{
			var card = new List<AdaptiveElement>
			{
				new AdaptiveTextBlock($"Could not find a protocol with name: '{input}'. Did you mean any of the following?") { Wrap = true },
			};

			var list = new Dictionary<string, ProtocolMatch>();
			foreach (var protocol in protocols)
			{
				list.Add(protocol.Name, new ProtocolMatch(input, protocol.Name));
			}

			var sorted = list.OrderByDescending(pair => pair.Value.Score);
			foreach (var pair in sorted.Take(3))
			{
				card.Add(new AdaptiveFactSet
				{
					Facts = new List<AdaptiveFact>
					{
						new AdaptiveFact("Protocol:", pair.Key),
					},
				});
			}

			engine.GenerateInformation(JsonConvert.SerializeObject(card));
			engine.AddScriptOutput("AdaptiveCard", JsonConvert.SerializeObject(card));
		}
	}
}