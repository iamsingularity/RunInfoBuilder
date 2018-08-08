﻿using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Command.Models
{
	public abstract class CommandBase<TRunInfo> where TRunInfo : class
	{
		public string Key { get; set; }
		public string Description { get; set; }
		public string HelpText { get; set; }
		public Callback<TRunInfo> Callback { get;  } = new Callback<TRunInfo>();

		public List<CommandBase<TRunInfo>> SubCommands { get; } = new List<CommandBase<TRunInfo>>();
		public List<ArgumentBase<TRunInfo>> Arguments { get; } = new List<ArgumentBase<TRunInfo>>();
		public List<OptionBase<TRunInfo>> Options { get;} = new List<OptionBase<TRunInfo>>();

		internal abstract void Validate(Type parentType, string parentKey);

		protected void ValidateBase(Type derivedType, Type parentType, string parentKey)
		{
			if (string.IsNullOrWhiteSpace(Key))
			{
				throw new ConfigurationException("Key must be provided.",
					derivedType, parentType, parentKey);
			}

			Arguments.ForEach(a => a.Validate(derivedType, Key));
		}
	}
}
