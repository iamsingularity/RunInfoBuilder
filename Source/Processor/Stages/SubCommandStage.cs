﻿using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class SubCommandStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		private Dictionary<string, Queue<Stage<TRunInfo>>> _subCommandPipelineMap { get; }
		private Action<Queue<Stage<TRunInfo>>> _extendPipelineCallback { get; }
		private ProcessContext<TRunInfo> _context { get; }

		internal SubCommandStage(
			Dictionary<string, Queue<Stage<TRunInfo>>> subCommandPipelineMap,
			Action<Queue<Stage<TRunInfo>>> extendPipelineCallback,
			ProcessContext<TRunInfo> context)
			: base(context)
		{
			_subCommandPipelineMap = subCommandPipelineMap;
			_extendPipelineCallback = extendPipelineCallback;
			_context = context;
		}

		internal override ProcessStageResult ProcessStage()
		{
			if (!MoreProgramArgumentsExist())
			{
				return ProcessResult.End;
			}

			string subCommand = Dequeue();

			if (!_subCommandPipelineMap.TryGetValue(subCommand, out Queue<Stage<TRunInfo>> pipeline))
			{
				throw new InvalidOperationException($"'{subCommand}' is not a valid sub command.");
			}

			_extendPipelineCallback(pipeline);

			return ProcessResult.Continue;
		}
	}
}
