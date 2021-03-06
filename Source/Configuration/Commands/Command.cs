﻿using R5.RunInfoBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace R5.RunInfoBuilder
{
	/// <summary>
	/// The command configuration object. All other configurations like arguments start from here.
	/// </summary>
	/// <typeparam name="TRunInfo">The RunInfo type that's built from the command.</typeparam>
	public class Command<TRunInfo> : CommandBase<TRunInfo>
		where TRunInfo : class
	{
		/// <summary>
		/// A unique key representing the command.
		/// </summary>
		/// <remarks>
		/// The key only needs to be unique within the same level of command. 
		/// A command and one of its' subcommands can share the same key.
		/// </remarks>
		public string Key { get; set; }

		/// <summary>
		/// List of subcommands that are associated to this command.
		/// </summary>
		public List<Command<TRunInfo>> SubCommands { get; set; } = new List<Command<TRunInfo>>();

		internal override List<Action<int>> Rules() => ValidationRules.Commands.Command.Rules(this);
	}
}
