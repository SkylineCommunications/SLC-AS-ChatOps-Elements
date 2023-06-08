namespace Show_Elements_By_Protocol_1
{
	using System;
	using System.Linq;
	using System.Text.RegularExpressions;

	internal class ProtocolMatch
	{
		private readonly string input;
		private readonly string protocolName;

		public ProtocolMatch(string input, string protocolName)
		{
			this.input = input;
			this.protocolName = protocolName;
		}

		public string Input { get => this.input; }

		public string ProtocolName { get => this.protocolName; }

		/// <summary>
		/// How close the input matches to the given protocolName
		/// </summary>
		public int Score
		{
			get
			{
				var result = 0;
				result += Match() ? 1000 : 0;
				result += protocolName.Split(' ', '.').Select(part => LevenshteinDistance(part, input)).Max();

				return result;
			}
		}

		/// <summary>
		/// Search for an exact match in the protocol.
		/// </summary>
		/// <returns><see langword="true"/> if it finds a match, <see langword="true"/> if it doesn't.</returns>
		private bool Match()
		{
			return Regex.IsMatch(protocolName.ToLower(), input.ToLower());
		}

		/// <summary>
		/// Compute the distance between two strings.
		/// </summary>
		private int LevenshteinDistance(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// Step 1
			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			// Step 2
			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			// Step 3
			for (int i = 1; i <= n; i++)
			{
				//Step 4
				for (int j = 1; j <= m; j++)
				{
					// Step 5
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					// Step 6
					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}

			// Step 7
			return 100 - d[n, m] <= 0 ? 0 : 100 - d[n, m];
		}
	}
}
