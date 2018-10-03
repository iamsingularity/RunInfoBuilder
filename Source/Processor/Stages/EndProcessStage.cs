﻿using R5.RunInfoBuilder.Processor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.RunInfoBuilder.Processor.Stages
{
	internal class EndProcessStage<TRunInfo> : Stage<TRunInfo>
		where TRunInfo : class
	{
		internal EndProcessStage()
		{
		}

		internal override ProcessStageResult ProcessStage(ProcessContext<TRunInfo> context,
			Action<CommandBase<TRunInfo>> resetContextFunc)
		{
			if (context.ProgramArguments.HasMore())
			{
				var invalidArgument = context.ProgramArguments.Peek();
				throw new ProcessException($"Invalid program argument found: {invalidArgument}",
					ProcessError.InvalidProgramArgument, context.CommandLevel);
			}

			return ProcessResult.End;
		}
	}
}
