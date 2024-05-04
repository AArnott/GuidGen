// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft;

namespace GuidGen;

public class SimpleCommand : CommandBase
{
    private readonly Action command;

    public SimpleCommand(Action command)
    {
        Requires.NotNull(command, "command");
        this.command = command;
    }

    public override void Execute(object parameter)
    {
        this.command();
    }
}
