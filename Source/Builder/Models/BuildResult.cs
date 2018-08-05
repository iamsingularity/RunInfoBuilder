﻿using System;
using System.Collections.Generic;

namespace OLD
{
	public class BuildResult<TRunInfo>
		where TRunInfo : class
	{
		public TRunInfo RunInfo { get; }

		public BuildResultType Type { get; }

		public string FailMessage { get; }

		public Exception Exception { get; }

		public List<ProgramArgumentError> ProgramArgumentErrors { get; }

		private BuildResult(TRunInfo runInfo, BuildResultType resultType,
			string failMessage = null, Exception exception = null,
			List<ProgramArgumentError> programArgumentErrors = null)
		{
			this.RunInfo = runInfo;
			this.Type = resultType;
			this.FailMessage = failMessage;
			this.Exception = exception;
			this.ProgramArgumentErrors = programArgumentErrors;
		}

		internal static BuildResult<TRunInfo> Success(TRunInfo runInfo)
		{
			return new BuildResult<TRunInfo>(runInfo, BuildResultType.Success);
		}

		internal static BuildResult<TRunInfo> Fail(string failMessage, Exception exception)
		{
			return new BuildResult<TRunInfo>(null, BuildResultType.Fail, 
				failMessage, exception);
		}

		internal static BuildResult<TRunInfo> Help()
		{
			return new BuildResult<TRunInfo>(null, BuildResultType.Help);
		}

		internal static BuildResult<TRunInfo> NotProcessed()
		{
			return new BuildResult<TRunInfo>(null, BuildResultType.NotProcessed);
		}

		internal static BuildResult<TRunInfo> Version()
		{
			return new BuildResult<TRunInfo>(null, BuildResultType.Version);
		}
	}
}
