namespace Show_Elements_By_Protocol_1
{
	using System;
	using System.Collections.Generic;
	using AdaptiveCards;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public class AdaptiveDataMinerElement
	{
		private IDmsElement element;

		public AdaptiveDataMinerElement(IDmsElement element)
		{
			this.element = element;
		}

		public AdaptiveElement ToAdaptiveElement()
		{
			var container = new AdaptiveContainer
			{
				Spacing = AdaptiveSpacing.Medium,
				Items = new List<AdaptiveElement>
				{
					new AdaptiveColumnSet
					{
						new AdaptiveColumn
						{
							Width = "auto",
							Items = new List<AdaptiveElement>
							{
								new AdaptiveTextBlock
								{
									Text = "⬤",
									Size = AdaptiveTextSize.ExtraLarge,
									Color = TranslateAlarm(element.GetAlarmLevel()),
								},
							},
						},
						new AdaptiveColumn
						{
							Width = "strech",
							Items = new List<AdaptiveElement>
							{
								new AdaptiveTextBlock
								{
									Text = element.Name,
									Size = AdaptiveTextSize.Large,
									Wrap = true,
									Style = AdaptiveTextBlockStyle.Heading,
								},
								new AdaptiveTextBlock
								{
									Text = $"{element.Protocol.Name}:{element.Protocol.Version}",
									IsSubtle = true,
									Spacing = AdaptiveSpacing.None,
									Wrap = true,
									Size = AdaptiveTextSize.Medium,
								},
							},
						},
					},
					new AdaptiveTextBlock
					{
						Text = element.Description,
						Wrap = true,
					},
					new AdaptiveColumnSet
					{
						Columns = new List<AdaptiveColumn>
						{
							new AdaptiveColumn
							{
								Width = "stretch",
								Items = new List<AdaptiveElement>
								{
									new AdaptiveTextBlock
									{
										Text = element.State.ToString(),
										Size = AdaptiveTextSize.Large,
										Wrap = true,
									},
									new AdaptiveTextBlock
									{
										Text = $"{element.AgentId}/{element.Id}",
										Spacing = AdaptiveSpacing.None,
										Wrap = true,
									},
								},
							},
							new AdaptiveColumn
							{
								Width = "auto",
								Items = new List<AdaptiveElement>
								{
									new AdaptiveFactSet
									{
										Facts = new List<AdaptiveFact>
										{
											new AdaptiveFact
											{
												Title = "Critical",
												Value = Convert.ToString(element.GetActiveCriticalAlarmCount()),
											},
											new AdaptiveFact
											{
												Title = "Major",
												Value = Convert.ToString(element.GetActiveMajorAlarmCount()),
											},
											new AdaptiveFact
											{
												Title = "Minor",
												Value = Convert.ToString(element.GetActiveMinorAlarmCount()),
											},
										},
									},
								},
							},
						},
					},
				},
			};

			return container;
		}

		private AdaptiveTextColor TranslateAlarm(AlarmLevel state)
		{
			switch (state)
			{
				case AlarmLevel.Normal:
					return AdaptiveTextColor.Good;
				case AlarmLevel.Minor:
					return AdaptiveTextColor.Accent;
				case AlarmLevel.Major:
				case AlarmLevel.Warning:
				case AlarmLevel.Timeout:
					return AdaptiveTextColor.Warning;
				case AlarmLevel.Critical:
				case AlarmLevel.Error:
					return AdaptiveTextColor.Attention;
				default:
					return AdaptiveTextColor.Default;
			}
		}
	}
}
