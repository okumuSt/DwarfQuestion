using System;
using System.Collections.Generic;
using System.Linq;
					
public class Program
{
  private static bool _blackIsOdd;
	public static void Main()
	{ 
		int count;
		do
		{
			Console.WriteLine("Dwarf count:");
			var input = Console.ReadLine();
			if (int.TryParse(input, out count))
				break;
			Console.WriteLine(input + " is not valid integer");
		} while (true);
		
		var rand = new Random();
		var dwarfs = GiveHats(count, rand);

		var nextHats = dwarfs.Select(s => s.Hat).ToList();
		var heardBlackCount = 0;
		var answers = new List<HatColor>();
		foreach (var dwarf in dwarfs)
		{
			nextHats = nextHats.Skip(1).ToList();
			var answer = dwarf.Talk(count, nextHats, heardBlackCount);
			answers.Add(answer);

			if (nextHats.Count < count - 1 && answer == HatColor.Black)
				heardBlackCount++;
		}

		Console.WriteLine("hats   answers  status");
		for (var i = 0; i < count; i++)
		{
			var status = dwarfs[i].Hat == answers[i] ? "alive" : "dead";
			Console.WriteLine(dwarfs[i].Hat+"  "+answers[i]+"    "+status);
		}
	}

	private class Dwarf
	{
		public Dwarf(HatColor hat)
		{
			Hat = hat;
		}

		public HatColor Hat { get; set; }

		public HatColor Talk(int count, List<HatColor> nextHats, int heardBlackCount)
		{
			var seenBlackCount = SeenBlackCount(nextHats);

			if (nextHats.Count == count - 1)
			{
				_blackIsOdd = Math.Abs(seenBlackCount % 2) == 1;
				return _blackIsOdd ? HatColor.Black : HatColor.White;
			}

			if (_blackIsOdd)
			{
				return (seenBlackCount + heardBlackCount) % 2 == 1 ? HatColor.White : HatColor.Black;
			}
			else
			{
				return (seenBlackCount + heardBlackCount) % 2 == 1 ? HatColor.Black : HatColor.White;
			}
		}

		private int SeenBlackCount(List<HatColor> nextHats)
		{
			return nextHats.Count(s => s == HatColor.Black);
		}
	}

	public enum HatColor
	{
		Black,
		White
	}

	private static List<Dwarf> GiveHats(int count, Random rand)
	{
		var dwarfs = new List<Dwarf>();
		for (var i = 0; i < count; i++)
			dwarfs.Add(rand.Next(0, 2) == 0
					   ? new Dwarf(HatColor.White)
					   : new Dwarf(HatColor.Black));

		return dwarfs;
	}
}
