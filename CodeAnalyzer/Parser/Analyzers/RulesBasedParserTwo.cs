// RulesAndActions.cs                                                

/*
 * Package Operations:
 * -------------------
 * RulesAndActions package contains all of the Application specific
 * code required for most analysis tools.
 *
 * It defines the following Four rules which each have a
 * grammar construct detector and also a collection of IActions:
 *   - DetectNameSpace rule
 *   - DetectClass rule
 *   - DetectFunction rule
 *   - DetectScopeChange
 *   
 *   Three actions - some are specific to a parent rule:
 *   - Print
 *   - PrintFunction
 *   - PrintScope
 * 
 * The package also defines a Repository class for passing data between
 * actions and uses the services of a ScopeStack, defined in a package
 * of that name.
 *
 * Note:
 * This package does not have a test stub since it cannot execute
 * without requests from RulesBasedParser.
 *  
 */
/* Required Files:
 *   IRuleAndAction.cs, RulesAndActions.cs, RulesBasedParser.cs, ScopeStack.cs,
 *   Semi.cs, Toker.cs
 *   
 * Build command:
 *   csc /D:TEST_PARSER RulesBasedParser.cs IRuleAndAction.cs RulesAndActions.cs \
 *                      ScopeStack.cs Semi.cs Toker.cs
 *   
 * Maintenance History:
 * --------------------
 * ver 2.3 : 30 Sep 2014
 * - added scope-based complexity analysis
 *   Note: doesn't detect braceless scopes yet
 * ver 2.2 : 24 Sep 2011
 * - modified Semi package to extract compile directives (statements with #)
 *   as semiExpressions
 * - strengthened and simplified DetectFunction
 * - the previous changes fixed a bug, reported by Yu-Chi Jen, resulting in
 * - failure to properly handle a couple of special cases in DetectFunction
 * - fixed bug in PopStack, reported by Weimin Huang, that resulted in
 *   overloaded functions all being reported as ending on the same line
 * - fixed bug in isSpecialToken, in the DetectFunction class, found and
 *   solved by Zuowei Yuan, by adding "using" to the special tokens list.
 * - There is a remaining bug in Toker caused by using the @ just before
 *   quotes to allow using \ as characters so they are not interpreted as
 *   escape sequences.  You will have to avoid using this construct, e.g.,
 *   use "\\xyz" instead of @"\xyz".  Too many changes and subsequent testing
 *   are required to fix this immediately.
 * ver 2.1 : 13 Sep 2011
 * - made FirstPassAnalyzer a public class
 * ver 2.0 : 05 Sep 2011
 * - removed old stack and added scope stack
 * - added Repository class that allows actions to save and 
 *   retrieve application specific data
 * - added rules and actions specific to Project #2, Fall 2010
 * ver 1.1 : 05 Sep 11
 * - added Repository and references to ScopeStack
 * - revised actions
 * - thought about added folding rules
 * ver 1.0 : 28 Aug 2011
 * - first release
 *
 * Planned Modifications (not needed for Project #2):
 * --------------------------------------------------
 * - add folding rules:
 *   - CSemiExp returns for(int i=0; i<len; ++i) { as three semi-expressions, e.g.:
 *       for(int i=0;
 *       i<len;
 *       ++i) {
 *     The first folding rule folds these three semi-expression into one,
 *     passed to parser. 
 *   - CToker returns operator[]( as four distinct tokens, e.g.: operator, [, ], (.
 *     The second folding rule coalesces the first three into one token so we get:
 *     operator[], ( 
 */

using System.Collections.Generic;
using Parser.Actions;
using Parser.Parser;
using Parser.Rules;

namespace Parser.Analyzers
{
	public class RulesBasedParserTwo : CodeAnalyzer
	{
		private readonly List<Elem> _locations;
		private readonly Dictionary<string, List<string>> _classMembers;

		public RulesBasedParserTwo(CSemiExp semi, List<Elem> locations, Dictionary<string, List<string>> classMembers)
		{
			_locations = locations;
			_classMembers = classMembers;
			Repo.semi = semi;
		}
		public virtual RulesBasedParser build()
		{
			RulesBasedParser rulesBasedParser = new RulesBasedParser();

			// decide what to show
			AAction.displaySemi = false;
			AAction.displayStack = false;  // false is default

			// action used for namespaces, locations, and functions
			PushStack push = new PushStack(Repo);

			// capture class dependencies
			var detectDeps = new DetectClassDependency(Repo, _locations);
			var addClassDep = new AddClassDependency(Repo);
			detectDeps.add(addClassDep);
			rulesBasedParser.add(detectDeps);

			var detectMemberUsage = new DetectMemberUsage(Repo, _classMembers);
			var addMemberUsage = new AddMemberUsage(Repo);
			detectMemberUsage.add(addMemberUsage);
			rulesBasedParser.add(detectMemberUsage);

			// capture namespace info
			DetectNamespace detectNS = new DetectNamespace();
			detectNS.add(push);
			rulesBasedParser.add(detectNS);

			// capture class info
			DetectClass detectCl = new DetectClass();
			detectCl.add(push);
			rulesBasedParser.add(detectCl);

			// capture function info
			DetectFunction detectFN = new DetectFunction(Repo);
			detectFN.add(push);
			rulesBasedParser.add(detectFN);

			// handle leaving scopes
			DetectLeavingScope leave = new DetectLeavingScope();
			PopStack pop = new PopStack(Repo);
			leave.add(pop);
			rulesBasedParser.add(leave);

			// rulesBasedParser configured
			return rulesBasedParser;
		}
	}
}

