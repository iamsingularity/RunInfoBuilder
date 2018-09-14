﻿using R5.RunInfoBuilder.Commands;
using R5.RunInfoBuilder.FunctionalTests.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace R5.RunInfoBuilder.FunctionalTests.Tests.Processing.Options
{
	public class OptionFailTests
	{
		private static RunInfoBuilder GetBuilder()
		{
			return new RunInfoBuilder();
		}

		public class InSingleCommand
		{
			[Theory]
			[InlineData("invalid")]
			[InlineData("invalid=invalid=")]
			[InlineData("invalid=")]
			[InlineData("--=invalid")]
			[InlineData("-=i")]
			[InlineData("-ii")]
			// This tests that an invalid option program argument simply skips
			// the option stage and hits the invalid program argument stage.
			public void InvalidOption_TokenizeFail_Throws(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						}
					});

					builder.Build(new string[] { "command", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidProgramArgument, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}


			[Theory]
			[InlineData("-ib")]
			[InlineData("-bi")]
			[InlineData("-cib")]
			[InlineData("-bci")]
			public void StackedOption_MappedToNonBoolProperty_Throws_1(string option)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool1 | b",
								Property = ri => ri.Bool1
							},
							new Option<TestRunInfo, bool>
							{
								Key = "bool2 | c",
								Property = ri => ri.Bool2
							}
						}
					});

					builder.Build(new string[] { "command", option });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.InvalidStackedOption, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Fact]
			public void NonBoolOption_MissingValueProgramArgument_Throws()
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							}
						}
					});

					builder.Build(new string[] { "command", "--int" });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(ProcessError.ExpectedProgramArgument, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}

			[Theory]
			[InlineData("--int1", ProcessError.ExpectedValueFoundOption)]
			[InlineData("subcommand", ProcessError.ExpectedValueFoundSubCommand)]
			public void Expected_OptionArgumentValue_AsNext_ButOptionOrSubCommand_Throws(
				string next, ProcessError expectedErrorType)
			{
				Action testCode = () =>
				{
					RunInfoBuilder builder = GetBuilder();

					builder.Commands.Add(new Command<TestRunInfo>
					{
						Key = "command",
						Options =
						{
							new Option<TestRunInfo, int>
							{
								Key = "int | i",
								Property = ri=> ri.Int1
							},
							new Option<TestRunInfo, int>
							{
								Key = "int1 | j",
								Property = ri=> ri.Int2
							}
						},
						SubCommands =
						{
							new Command<TestRunInfo>
							{
								Key = "subcommand"
							}
						}
					});

					builder.Build(new string[] { "command", "--int", next });
				};

				Exception exception = Record.Exception(testCode);

				var processException = exception as ProcessException;

				Assert.NotNull(processException);
				Assert.Equal(expectedErrorType, processException.ErrorType);
				Assert.Equal(0, processException.CommandLevel);
			}
		}

		public class InNestedSubCommand
		{

		}
	}
}
