///////////////////////////////////////////////////////////////////////
// RulesBasedParser.cs                                               //
// ver 1.5                                                           //
// Language:    C#, 2008, .Net Framework 4.0                         //
// Platform:    Dell Precision T7400, Win7, SP1                      //
// Application: Demonstration for CSE681, Project #2, Fall 2011      //
// Author:      Jim Fawcett, CST 4-187, Syracuse University          //
//              (315) 443-3948, jfawcett@twcny.rr.com                //
///////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ------------------
 * This module defines the following class:
 *   RulesBasedParser  - a collection of IRules
 */
/* Required Files:
 *   IRulesAndActions.cs, RulesAndActions.cs, RulesBasedParser.cs, Semi.cs, Toker.cs
 *   Display.cs
 *   
 * Maintenance History:
 * --------------------
 * ver 1.5 : 14 Oct 2014
 * - added bug fix to tokenizer to avoid endless loop on
 *   multi-line strings
 * ver 1.4 : 30 Sep 2014
 * - modified test stub to display scope counts
 * ver 1.3 : 24 Sep 2011
 * - Added exception handling for exceptions thrown while parsing.
 *   This was done because Toker now throws if it encounters a
 *   string containing @".
 * - RulesAndActions were modified to fix bugs reported recently
 * ver 1.2 : 20 Sep 2011
 * - removed old stack, now replaced by ScopeStack
 * ver 1.1 : 11 Sep 2011
 * - added comments to parse function
 * ver 1.0 : 28 Aug 2011
 * - first release
 */

using System.Collections.Generic;

namespace Parser.Parser
{
  /////////////////////////////////////////////////////////
  // rule-based parser used for code analysis

  public class RulesBasedParser
  {
    private List<IRule> Rules;

    public RulesBasedParser()
    {
      Rules = new List<IRule>();
    }
    public void add(IRule rule)
    {
      Rules.Add(rule);
    }
    public void parse(CSemiExp semi)
    {
      // Note: rule returns true to tell parser to stop
      //       processing the current semiExp
      
      Display.Display.displaySemiString(semi.displayStr());

      foreach (IRule rule in Rules)
      {
        if (rule.test(semi))
          break;
      }
    }
  }
}
