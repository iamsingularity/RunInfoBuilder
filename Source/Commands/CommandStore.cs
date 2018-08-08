﻿using R5.RunInfoBuilder.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Commands
{
	public interface ICommandStore
	{
		ICommandStore Add<TRunInfo>(Command<TRunInfo> command)
			where TRunInfo : class;

		ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand)
			where TRunInfo : class;
	}

	internal interface ICommandStoreInternal : ICommandStore
	{
		object Get(string key);

		object GetDefault();
	}

	internal class CommandStore : ICommandStore, ICommandStoreInternal
	{
		private ICommandConfigurationValidator _validator { get; }
		private IRestrictedKeyValidator _keyValidator { get; }

		// keep values as object because we dont know their generic types until runtime
		private Dictionary<string, object> _commandMap { get; }
		private object _defaultCommand { get; set; }

		public CommandStore(
			ICommandConfigurationValidator validator,
			IRestrictedKeyValidator keyValidator)
		{
			_validator = validator;
			_keyValidator = keyValidator;
			_commandMap = new Dictionary<string, object>();
		}

		public ICommandStore Add<TRunInfo>(Command<TRunInfo> command) where TRunInfo : class
		{
			_validator.Validate(command);

			if (_commandMap.ContainsKey(command.Key))
			{
				throw new InvalidOperationException($"Command with key '{command.Key}' has already been configured.");
			}

			_commandMap.Add(command.Key, command);

			_keyValidator.Add(command.Key);

			return this;
		}

		public ICommandStore AddDefault<TRunInfo>(DefaultCommand<TRunInfo> defaultCommand) where TRunInfo : class
		{
			_validator.Validate(defaultCommand);

			if (_defaultCommand != null)
			{
				throw new InvalidOperationException("Default command has already been configured.");
			}

			_defaultCommand = defaultCommand;

			return this;
		}

		public object Get(string key)
		{
			if (!_commandMap.ContainsKey(key))
			{
				throw new InvalidOperationException($"Command with key '{key}' hasn't been configured.");
			}

			return _commandMap[key];
		}

		public object GetDefault()
		{
			if (_defaultCommand == null)
			{
				throw new InvalidOperationException("Default command hasn't been configured.");
			}

			return _defaultCommand;
		}
	}

}