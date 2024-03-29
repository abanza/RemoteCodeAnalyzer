﻿// IRuleAndAction.cs - Interfaces & abstract bases for rules and actions 

/*
 * Module Operations:
 * ------------------
 * This module defines the following classes:
 *   IRule   - interface contract for Rules
 *   ARule   - abstract base class for Rules that defines some common ops
 *   IAction - interface contract for rule actions
 *   AAction - abstract base class for actions that defines common ops
 */
/* Required Files:
 *   IRuleAndAction.cs
 *   
 * Build command:
 *   Interfaces and abstract base classes only so no build
 *   
 * Maintenance History:
 * --------------------
 * ver 1.2 : 2  Feb 2018
 * - Adapted for use in Code Maintainabilty Analyzer
 * ver 1.1 : 11 Sep 2011
 * - added properties displaySemi and displayStack
 * ver 1.0 : 28 Aug 2011
 * - first release
 *
 * Note:
 * This package does not have a test stub as it contains only interfaces
 * and abstract classes.
 *
 */

using System;
using System.Collections.Generic;

namespace Parser.Parser
{
	/////////////////////////////////////////////////////////
	// contract for actions used by parser rules

	public interface IAction
	{
		void doAction(CSemiExp semi);
	}
	/////////////////////////////////////////////////////////
	// abstract action base supplying common functions

	public abstract class AAction : IAction
	{
		static bool displaySemi_;   // default
		static bool displayStack_;  // default

		protected Repository repo_;

		static public Action<string> actionDelegate;

		public abstract void doAction(CSemiExp semi);

		public static bool displaySemi
		{
			get { return displaySemi_; }
			set { displaySemi_ = value; }
		}
		public static bool displayStack
		{
			get { return displayStack_; }
			set { displayStack_ = value; }
		}

		public virtual void display(CSemiExp semi)
		{
			if (displaySemi)
				for (int i = 0; i < semi.count; ++i)
					Console.Write("{0} ", semi[i]);
		}
	}
	/////////////////////////////////////////////////////////
	// contract for parser rules

	public interface IRule
	{
		bool test(CSemiExp semi);
		void add(IAction action);
	}
	/////////////////////////////////////////////////////////
	// abstract rule base implementing common functions

	public abstract class ARule : IRule
	{
		private List<IAction> actions;
		static public Action<string> actionDelegate;

		public ARule()
		{
			actions = new List<IAction>();
		}
		public void add(IAction action)
		{
			actions.Add(action);
		}
		abstract public bool test(CSemiExp semi);
		public void doActions(CSemiExp semi)
		{
			foreach (IAction action in actions)
				action.doAction(semi);
		}
		public int indexOfType(CSemiExp semi)
		{
			int indexCL = semi.Contains("class");
			int indexIF = semi.Contains("interface");
			int indexST = semi.Contains("struct");
			int indexEN = semi.Contains("enum");
			int indexDE = semi.Contains("delegate");

			int index = Math.Max(indexCL, indexIF);
			index = Math.Max(index, indexST);
			index = Math.Max(index, indexEN);
			index = Math.Max(index, indexDE);
			return index;
		}
	}
}

