﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.Azure.Devices.Edge.Agent.Service.Modules
{
    using System;
    using Autofac;
    using Autofac.Core;
    using Microsoft.Azure.Devices.Edge.Util;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Autofac module for iothubConnected mode.
    /// There are no external dependencies required, except Docker
    /// and a connection string to connect to the twin.
    /// </summary>
    public class IotHubConnectedModule : Module
    {
        readonly IModule agent;
        readonly IModule configSource;
        readonly IModule logging;

        public IotHubConnectedModule(Uri dockerHost, string dockerLoggingDriver, string connectionString, IConfiguration configuration)
        {
            this.agent = new AgentModule(Preconditions.CheckNotNull(dockerHost, nameof(dockerHost)));
            this.configSource = new TwinConfigSourceModule(connectionString, Preconditions.CheckNotNull(configuration, nameof(configuration)));
            this.logging = new LoggingModule(Preconditions.CheckNonWhiteSpace(dockerLoggingDriver, nameof(dockerLoggingDriver)));
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(this.agent);
            builder.RegisterModule(this.configSource);
            builder.RegisterModule(this.logging);
        }
    }
}