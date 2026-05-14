// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Log
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Reflection;
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Filter;
    using log4net.Layout;
    using log4net.Repository;
    using log4net.Repository.Hierarchy;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// log4net 日志对象提供者
    /// </summary>
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private const string DefaultLog4NetFileName = "log4net.config";
        private readonly ConcurrentDictionary<string, Log4NetLogger> loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        private readonly ILoggerRepository loggerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLoggerProvider"/> class.
        /// </summary>
        public Log4NetLoggerProvider()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultLog4NetFileName);
            Assembly assembly = Assembly.GetEntryAssembly() ?? GetCallingAssemblyFromStartup() ?? this.GetType().Assembly;
            this.loggerRepository = LogManager.CreateRepository(assembly, typeof(Hierarchy));

            if (File.Exists(file))
            {
                XmlConfigurator.ConfigureAndWatch(this.loggerRepository, new FileInfo(file));
            }
            else
            {
                RollingFileAppender appender = new RollingFileAppender
                {
                    Name = "root",
                    File = Path.Combine("App_Data", "log_"),
                    AppendToFile = true,
                    LockingModel = new FileAppender.MinimalLock(),
                    RollingStyle = RollingFileAppender.RollingMode.Date,
                    DatePattern = "yyyyMMdd-HH\".log\"",
                    StaticLogFileName = false,
                    MaxSizeRollBackups = 10,
                    Layout = new PatternLayout("[%d{HH:mm:ss.fff}] %-5p %c T%t %n%m%n"),
                };
                appender.ClearFilters();
                appender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Debug });
                BasicConfigurator.Configure(this.loggerRepository, appender);
                appender.ActivateOptions();
            }
        }

        /// <summary>
        /// 创建一个 <see cref="T:Microsoft.Extensions.Logging.ILogger" /> 的新实例
        /// </summary>
        /// <param name="categoryName">记录器生成的消息的类别名称。</param>
        /// <returns>日志实例</returns>
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, key => new Log4NetLogger(this.loggerRepository.Name, key));
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Disposes resources used by the <see cref="T:Microsoft.Extensions.Logging.ILoggerProvider" />.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            this.loggers.Clear();
        }

        private static Assembly? GetCallingAssemblyFromStartup()
        {
            var stackTrace = new System.Diagnostics.StackTrace(2);
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var type = frame?.GetMethod()?.DeclaringType;

                if (string.Equals(type?.Name, "Startup", StringComparison.OrdinalIgnoreCase))
                {
                    return type?.Assembly;
                }
            }

            return null;
        }
    }
}