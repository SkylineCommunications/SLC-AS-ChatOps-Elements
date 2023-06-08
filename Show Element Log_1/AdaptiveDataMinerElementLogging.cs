namespace Show_Element_Log_1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using AdaptiveCards;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public class AdaptiveDataMinerElementLogging
	{
		private IDmsElement element;

		public AdaptiveDataMinerElementLogging(IDmsElement element)
		{
			this.element = element;
		}

		private string LogPath => @"C:\Skyline DataMiner\Logging\" + element.Name + ".txt";

		private List<string> ReadLastLines(int lineCount)
		{
			using(FileStream fs = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{

			}
		}

		public AdaptiveElement ToAdaptiveElement()
		{
			var log = new AdaptiveRichTextBlock
			{
				Inlines = ReadLastLines(50).Select(line => (AdaptiveInline)new AdaptiveTextRun
				{
					Text = line,
				}).ToList(),
			};

			return log;
		}
	}
}
