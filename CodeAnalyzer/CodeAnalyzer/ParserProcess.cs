// ParserProcess.cs                                                               

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeAnalyzer.CodeAnalyzerObjects;
using Parser.Analyzers;
using Parser.Metrics;
using Parser.Parser;

namespace CodeAnalyzer
{
	public class ParserProcess
	{
		public static IEnumerable<ElementProperty> ParseFiles(IEnumerable<string> files, MIndexOptions mIndexOptions = null)
		{
			var firstPassLocations = new List<Elem>();
			var dependencies = new Dictionary<string, List<string>>();
			var classMembers = new Dictionary<string, List<string>>();
			var functionMembers = new Dictionary<(string Class, string Method), List<string>>();

			foreach (var file in files)
			{
				CSemiExp semi = new CSemiExp();
				semi.displayNewLines = false;
				if (!semi.open(file))
				{
					throw new ArgumentException($"Cannot open file {file}");
				}

				var firstPassCodeAnalyzer = new RulesBasedParserOne(semi);
				RulesBasedParser firstPassParser = firstPassCodeAnalyzer.build();

				while (semi.getSemi())
					firstPassParser.parse(semi);
				semi.close();

				var firstRepo = firstPassCodeAnalyzer?.GetRepository();
				firstPassLocations.AddRange(firstRepo.locations);
				foreach (var member in firstRepo.ClassMembers)
				{
					classMembers[member.Key] = member.Value;
				}
			}

			foreach (var file in files)
			{
				CSemiExp semi = new CSemiExp();
				semi.displayNewLines = false;
				if (!semi.open(file))
				{
					throw new ArgumentException($"Cannot open file {file}");
				}

				var secondPassCodeAnalyzer = new RulesBasedParserTwo(semi, firstPassLocations, classMembers);
				RulesBasedParser secondPassParser = secondPassCodeAnalyzer.build();

				while (semi.getSemi())
					secondPassParser.parse(semi);
				semi.close();

				var secondRepo = secondPassCodeAnalyzer?.GetRepository();
				foreach (var dependency in secondRepo.ClassDependencies)
				{
					dependencies[dependency.Key] = dependency.Value;
				}
				foreach (var member in secondRepo.FunctionMembers)
				{
					functionMembers[member.Key] = member.Value;
				}
			}

			var elements = firstPassLocations
			  .Select(e => new ElementProperty
			  {
				  Type = e.type,
				  Name = e.name,
				  FunctionClass = e.functionClass,
				  LinesOfCode = e.endLine - e.beginLine,
				  CyclomaticComplexity = e.endScopeCount - e.beginScopeCount + 1
			  })
			  .ToList();

			var newElements = new List<ElementProperty>();
			foreach (var e in elements)
			{
				if (e.Type == "class")
				{
					// Class members
					if (classMembers.ContainsKey(e.Name))
					{
						e.Members = classMembers[e.Name];
					}

					// Class dependencies
					if (dependencies.ContainsKey(e.Name))
					{
						foreach (var d in dependencies[e.Name])
						{
							var element = elements.First(el => el.Name == d);
							e.Dependencies.Add(element);
						}
					}
				}
				else if (e.Type == "function" && functionMembers.ContainsKey((e.FunctionClass, e.Name)))
				{
					e.Members = functionMembers[(e.FunctionClass, e.Name)];
				}

				newElements.Add(e);
			}

			// Metrics calculation
			var classes = newElements.Where(e => e.Type == "class");
			foreach (var c in classes)
			{
				c.Cohesion = Cohesion.Calculate(newElements, c);

				MIndex.Options = mIndexOptions ?? new MIndexOptions();
				c.MIndex = MIndex.Calculate(newElements, c);
			}

			return newElements;
		}

		public static IEnumerable<ElementProperty> ParseFiles(IEnumerable<Stream> files, MIndexOptions mIndexOptions = null)
		{
			var firstPassLocations = new List<Elem>();
			var dependencies = new Dictionary<string, List<string>>();
			var classMembers = new Dictionary<string, List<string>>();
			var functionMembers = new Dictionary<(string Class, string Method), List<string>>();

			foreach (var file in files)
			{
				CSemiExp semi = new CSemiExp();
				semi.displayNewLines = false;
				if (!semi.open(file))
				{
					throw new ArgumentException($"Cannot open file {file}");
				}

				var firstPassCodeAnalyzer = new RulesBasedParserOne(semi);
				RulesBasedParser firstPassParser = firstPassCodeAnalyzer.build();

				while (semi.getSemi())
					firstPassParser.parse(semi);
				semi.close();

				var firstRepo = firstPassCodeAnalyzer?.GetRepository();
				firstPassLocations.AddRange(firstRepo.locations);
				foreach (var member in firstRepo.ClassMembers)
				{
					classMembers[member.Key] = member.Value;
				}
			}

			foreach (var file in files)
			{
				CSemiExp semi = new CSemiExp();
				semi.displayNewLines = false;
				if (!semi.open(file))
				{
					throw new ArgumentException($"Cannot open file {file}");
				}

				var secondPassCodeAnalyzer = new RulesBasedParserTwo(semi, firstPassLocations, classMembers);
				RulesBasedParser secondPassParser = secondPassCodeAnalyzer.build();

				while (semi.getSemi())
					secondPassParser.parse(semi);
				semi.close();

				var secondRepo = secondPassCodeAnalyzer?.GetRepository();
				foreach (var dependency in secondRepo.ClassDependencies)
				{
					dependencies[dependency.Key] = dependency.Value;
				}
				foreach (var member in secondRepo.FunctionMembers)
				{
					functionMembers[member.Key] = member.Value;
				}
			}

			var elements = firstPassLocations
			  .Select(e => new ElementProperty
			  {
				  Type = e.type,
				  Name = e.name,
				  FunctionClass = e.functionClass,
				  LinesOfCode = e.endLine - e.beginLine,
				  CyclomaticComplexity = e.endScopeCount - e.beginScopeCount + 1
			  })
			  .ToList();

			var newElements = new List<ElementProperty>();
			foreach (var e in elements)
			{
				if (e.Type == "class")
				{
					// Class members
					if (classMembers.ContainsKey(e.Name))
					{
						e.Members = classMembers[e.Name];
					}

					// Class dependencies
					if (dependencies.ContainsKey(e.Name))
					{
						foreach (var d in dependencies[e.Name])
						{
							var element = elements.First(el => el.Name == d);
							e.Dependencies.Add(element);
						}
					}
				}
				else if (e.Type == "function" && functionMembers.ContainsKey((e.FunctionClass, e.Name)))
				{
					e.Members = functionMembers[(e.FunctionClass, e.Name)];
				}

				newElements.Add(e);
			}

			// Metrics calculation
			var classes = newElements.Where(e => e.Type == "class");
			foreach (var c in classes)
			{
				c.Cohesion = Cohesion.Calculate(newElements, c);

				MIndex.Options = mIndexOptions ?? new MIndexOptions();
				c.MIndex = MIndex.Calculate(newElements, c);
			}

			return newElements;
		}
	}
}
